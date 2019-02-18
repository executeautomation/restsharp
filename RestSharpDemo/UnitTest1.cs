using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Deserializers;
using RestSharpDemo.Model;
using RestSharpDemo.Utilities;

namespace RestSharpDemo
{
    [TestFixture]
    public class UnitTest1
    {



        [Test]
        public void TestMethod1()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts/{postid}", Method.GET);
            request.AddUrlSegment("postid", 1);

            var response = client.Execute(request);

            //Lib 1 - Dictionary based response
            //var deserialize = new JsonDeserializer();
            //var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            //var result = output["author"];

            //Lib 2 - JSON based response
            JObject obs = JObject.Parse(response.Content);
            Assert.That(obs["author"].ToString(), Is.EqualTo("Karthik KK"), "Author is not correct");
        }

        [Test]
        public void PostWithAnonymousBody()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts/{postid}/profile", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { name = "Raj" });

            request.AddUrlSegment("postid", 1);

            var response = client.Execute(request);

            var result = response.DeserializeResponse()["name"];
            

            Assert.That(result, Is.EqualTo("Raj"), "Author is not correct");

        }


        [Test]
        public void PostWithTypeClassBody()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new Posts() { id = "17", author="Execute Automation", title = "RestSharp demo course"  });

            var response = client.Execute<Posts>(request);

            //var deserialize = new JsonDeserializer();
            //var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            //var result = output["author"];

            Assert.That(response.Data.author, Is.EqualTo("Execute Automation"), "Author is not correct");

        }

        [Test]
        public void PostWithAsync()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new Posts() { id = "18", author = "Execute Automation", title = "RestSharp demo course" });

            //var response = client.Execute<Posts>(request);

            //Lib 3 - Using Async

            var response = client.ExecutePostTaskAsync<Posts>(request).GetAwaiter().GetResult();

            //var deserialize = new JsonDeserializer();
            //var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            //var result = output["author"];

            Assert.That(response.Data.author, Is.EqualTo("Execute Automation"), "Author is not correct");

        }

      }
}
