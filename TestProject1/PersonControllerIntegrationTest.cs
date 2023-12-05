using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
    public class PersonControllerIntegrationTest
    {

        #region Index
        [Fact]
        public void Index_ToReturnView()
        {
            //  Arrange

            // Act 
            HttpResponseMessage response = _client.GetAsync("Person/Index");

            // Assert
            response.Should().BeSuccessful();
        }
        #endregion
    }
}
