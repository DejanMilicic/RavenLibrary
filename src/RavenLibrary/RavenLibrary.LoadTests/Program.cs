using System;
using System.Net;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using NBomber.Plugins.Network.Ping;

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
            string user = "users%2F5101859";
            string url = $"http://18.193.149.237:5000/annotations/user/0/10?userId={user}";

            var step = HttpStep.Create("get", context =>
                Http.CreateRequest("GET", url)
                    .WithCheck(async response =>
                    {
                        var rc = await response.Content.ReadAsStringAsync();

                        return response.StatusCode == HttpStatusCode.OK
                            ? Response.Ok()
                            : Response.Fail();
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
