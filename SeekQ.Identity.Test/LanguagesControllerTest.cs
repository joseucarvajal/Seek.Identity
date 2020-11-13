using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SeekQ.Identity.Application.Profile.Language.ViewModel;
using Xunit;

namespace SeekQ.Identity.Test
{
    public class LanguagesControllerTest : BaseIntegrationTest<Startup>
    {
        public LanguagesControllerTest(WebApplicationFactory<Startup> factory)
            : base(factory)
        {

        }

        [Theory]
        [InlineData("/api/v1/identity/languages")]
        public async void GetLanguages_getExpectedLanguages(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var languageList = JsonConvert
                               .DeserializeObject<IEnumerable<LanguageViewModel>>
                                   (await response.Content.ReadAsStringAsync());

            Assert.True(languageList.ToList().Count > 0, "get language list");
        }
    }
}