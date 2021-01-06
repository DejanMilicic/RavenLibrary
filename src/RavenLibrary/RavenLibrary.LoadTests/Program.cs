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
        static TimeSpan Duration = TimeSpan.FromSeconds(30);
        static int Rate = 250;

        static void Main(string[] args)
        {
            var pingPluginConfig = PingPluginConfig.CreateDefault(new[] { "RavenCms" });
            var pingPlugin = new PingPlugin(pingPluginConfig);

            string host = "http://18.193.149.237:5000";
            var users = FeedData.FromJson<User>("users.json");
            var annotations = FeedData.FromJson<Annotation>("annotations.json");
            
            var items = new[]{
                    Url("GetAnnotationsByUserBook_Paged", annotations, $"{host}/annotations/userbook/0/10?userBookId="), 
                    Url("GetAnnotationsByUser_Paged", users, $"{host}/annotations/user/0/10?userId=")
            };

            foreach(var item in items){
                var stats = NBomberRunner
                    .RegisterScenarios(item)
                    .WithWorkerPlugins(pingPlugin)
                    .Run();

                //Console.WriteLine("90.0% : " + stats.ScenarioStats[0].StepStats[0].Percent90);
            }
        }

        public static Scenario Url<T>(string name, IFeedProvider<T> data, string url)
            where T : IHasId
        {
            // var random = new Random(2323);
            // var users = JArray.Load(new JsonTextReader(File.OpenText("users.json")));

            var tagFeed = Feed.CreateRandom("tagFeed", provider: data);

            var step = HttpStep.Create("step", tagFeed, context =>
            {
                //var userId = context.FeedItem;
                //var url = $"https://jsonplaceholder.typicode.com/users?id={userId}";

                string url2 = url + context.FeedItem.GetItemId();

                bool reported = false;
                return Http.CreateRequest("GET", url2)
                    .WithCheck(async response =>{
                        if(response.IsSuccessStatusCode == false && reported){
                            reported=true;
                            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                        }
                        return response.IsSuccessStatusCode
                            ? Response.Ok()
                            : Response.Fail();
                    }
                    );
            });


            var scenario = ScenarioBuilder
                .CreateScenario(name, step)
                .WithoutWarmUp()
                .WithLoadSimulations(new[]
                {
                    Simulation
                        .InjectPerSec(Rate, during: Duration)
                        //.InjectPerSec(rate: 20000, during: TimeSpan.FromSeconds(10))
                });

            return scenario;
        }

        public interface IHasId
        {
            string GetItemId();
        }

        public class User : IHasId
        {
            public string GetItemId() => id;
            
            public string id { get; set; }

            public string books { get; set; }
        }

        public class Annotation : IHasId
        {
            public string GetItemId() => id;
            
            public string id { get; set; }
        }

        public class Tag
        {

            public string tag { get; set; }
        }
    }
}
