using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace FinalProject.Models
{
    public class QuoteGenerator
    {
        readonly RestClient Client;
        readonly string APIKey;

        public QuoteGenerator()
        {
            Client = new RestClient("https://motivational-quotes1.p.rapidapi.com/motivation");
            APIKey = "a304792ddbmsheb316bb4dabae8cp1e4b69jsn38007b584913";
        }

        public Quote Generate()
        {
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-rapidapi-host", "motivational-quotes1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", APIKey);

            var response = Client.Execute(request).Content.Replace('"', '\'');

            var quote = new Regex(@"('.+')").Match(response).Groups.Values.ElementAt(1).ToString();
            var quotee = response.Remove(0, quote.Length+1);


            return new Quote()
            {
                Content = quote.Replace('\'', '"'),
                Quotee = quotee,
                Date = DateTime.Now,
                // category 1 represents the Motivation Quote category
                Category = "1"
            };

        }

    }
}
