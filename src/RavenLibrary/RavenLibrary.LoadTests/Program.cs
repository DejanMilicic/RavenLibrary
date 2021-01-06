using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FsToolkit.ErrorHandling;
using NBomber;
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
            //string url = $"http://18.193.149.237:5000/annotations/user/0/10?userId=";
            //var random = new Random(2323);
            //var users = JArray.Load(new JsonTextReader(File.OpenText("users.json")));

            var data = FeedData.FromJson<Tag>("tags.json");
            var tagFeed = Feed.CreateCircular("tagFeed", provider: data);

            var step = HttpStep.Create("step", tagFeed, context =>
            {
                //var userId = context.FeedItem;
                //var url = $"https://jsonplaceholder.typicode.com/users?id={userId}";

                string url = "http://localhost:52788/" + context.FeedItem.tag;

                return Http.CreateRequest("GET", url)
                    .WithCheck(async response =>
                        response.IsSuccessStatusCode
                            ? Response.Ok()
                            : Response.Fail()
                    );
            });


            var scenario = ScenarioBuilder
                .CreateScenario("GetAnnotationsByUser_Paged", step)
                .WithoutWarmUp()
                .WithLoadSimulations(new[]
                {
                    Simulation
                        .KeepConstant(10_000, during: TimeSpan.FromSeconds(20))
                        //.InjectPerSec(rate: 20000, during: TimeSpan.FromSeconds(10))
                });

            return scenario;
        }

        public class User
        {
            public string id { get; set; }

            public string books { get; set; }
        }

        public class Tag
        {

            public string tag { get; set; }
        }
    }
}
