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
        // API Key from subscribing to https://rapidapi.com/bitbiscuit-bitbiscuit-default/api/motivational-quotes1/
        readonly string APIKey = "a304792ddbmsheb316bb4dabae8cp1e4b69jsn38007b584913";

        public QuoteGenerator()
        {
            Client = new RestClient("https://motivational-quotes1.p.rapidapi.com/motivation");
        }

        /// <summary>
        /// Return a new quote pulled from RapidAPI Motivational Quotes
        /// </summary>
        /// <returns></returns>
        public Quote Generate()
        {
            // Create a new instance of the RestRequest class using the RestSharp Extension for Visual Studios
            var request = new RestRequest(Method.POST);
            //request.AddHeader("content-type", "application/json");
            request.AddHeader("x-rapidapi-host", "motivational-quotes1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", APIKey);

            var response = Client.Execute(request).Content.Replace('"', '\'');
            /* Response from API is returned as " ... Quote ..."\n-Quotee
                - Regex did not like the "s so I turned those into 's. Then I captured everything inside the 's
                - Regex also did not like the \n, even when if I tried using \r
                - To get around this I grabbed the quotee by removing the quote and the \n char.
            */                       
            var quote = new Regex(@"('.+')").Match(response).Groups.Values.ElementAt(1).ToString();
            var quotee = response.Remove(0, quote.Length+1);
           
            return new Quote()
            {
                // Remove the first ' and ' that were grabbed with Regex and then add " in their place.
                Content = "\"" + quote.Remove(quote.Length - 1, 1).Remove(0, 1) + "\"",
                // If we get a null Quotee we will return "-Unknown"
                Quotee = quotee == "-null" ? "-Unknown" : quotee,
                Date = DateTime.Now,
                // category 1 represents the Motivation Quote category in the Database
                Category = "1"
            };

        }

    }
}
