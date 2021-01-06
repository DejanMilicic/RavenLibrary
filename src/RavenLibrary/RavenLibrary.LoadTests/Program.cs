using System;
using System.IO;
using System.Net;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using NBomber.Plugins.Network.Ping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RavenLibrary.LoadTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var pingPluginConfig = PingPluginConfig.CreateDefault(new[] { "RavenCms" });
            var pingPlugin = new PingPlugin(pingPluginConfig);

            NBomberRunner
                .RegisterScenarios(GetAnnotationsByUser_Paged())
                .WithWorkerPlugins(pingPlugin)
                .Run();
        }

        public static Scenario GetAnnotationsByUser_Paged()
        {
            string url = $"http://18.193.149.237:5000/annotations/user/0/10?userId=";
            var random = new Random(2323);
            var users = JArray.Load(new JsonTextReader(File.OpenText("users.json")));
            
            var step = HttpStep.Create("get", context =>
                Http.CreateRequest("GET", url + users[random.Next(users.Count)].Value<string>("id"))
                    .WithCheck(async response =>
                    {
                        var rc = await response.Content.ReadAsStringAsync();

                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            context.Logger.Information(rc);
                            return Response.Fail();
                        }

                        return Response.Ok();
                    })
            );

            var scenario = ScenarioBuilder
                .CreateScenario("GetAnnotationsByUser_Paged", step)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(rate: 100, during: TimeSpan.FromSeconds(30))
                });

            return scenario;
        }
    }
}
