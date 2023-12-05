using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
    // IClassFixture using this create new object of customwebapplicationfactory 
    public class PersonControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        // Predefine class make HTTP rquests and receive the  response 
        private readonly HttpClient _client;

        public PersonControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
                _client = factory.CreateClient();
        }

        #region Index
        [Fact]
        public async void Index_ToReturnView()
        {
            //  Arrange

            // Act 
              HttpResponseMessage response = await _client.GetAsync("~/Person/Index");

            // Assert
            response.Should().BeSuccessful();

          string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(responseBody);
            var document = htmlDocument.DocumentNode;

            document.QuerySelectorAll("table.persons").Should().NotBeNull();
         }
        #endregion
    }
}
