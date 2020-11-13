using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SeekQ.Identity.Application.Profile.Gender.ViewModel;
using Xunit;

namespace SeekQ.Identity.Test
{
    public class GendersControllerTest : BaseIntegrationTest<Startup>
    {
        public GendersControllerTest(WebApplicationFactory<Startup> factory)
            : base(factory)
        {

        }

        [Theory]
        [InlineData("/api/v1/identity/genders")]
        public async void GetGenders_getExpectedGenders(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var genderList = JsonConvert
                               .DeserializeObject<IEnumerable<UserGenderViewModel>>
                                   (await response.Content.ReadAsStringAsync());

            Assert.True(genderList.ToList().Count > 0, "get gender list");
        }
    }
}
