using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SeekQ.Identity.Application.Profile.UserLanguageKnow.ViewModel;
using SeekQ.Identity.Models.Profile;
using Xunit;

namespace SeekQ.Identity.Test
{
    public class UserLanguageKnowControllerTest : BaseIntegrationTest<Startup>
    {
        public UserLanguageKnowControllerTest(WebApplicationFactory<Startup> factory)
            : base(factory)
        {

        }

        [Theory]
        [InlineData("/api/v1/identity/UserLanguageKnow")]
        public async void GetUserLanguageKnow_getExpectedUserLanguageKnow(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{url}/{SeedData.ID_USER_MOCK1}");

            // Assert
            response.EnsureSuccessStatusCode();
            var userLanguagesKnowList = JsonConvert
                               .DeserializeObject<IEnumerable<UserLanguageKnowViewModel>>
                                   (await response.Content.ReadAsStringAsync());

            Assert.True(userLanguagesKnowList.ToList().Count > 0, "Get all user language Know list");
        }

        [Theory]
        [InlineData("/api/v1/identity/UserLanguageKnow")]
        public async void CreateUserLanguageKnow_createExpectedUserLanguageKnow(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            UserLanguageKnow addLanguageKnow = new UserLanguageKnow
            {
                ApplicationUserId = SeedData.ID_USER_MOCK1.ToString(),
                LanguageKnowId = 1
            };

            // Content
            var httpContent = new StringContent(JsonConvert.SerializeObject(addLanguageKnow), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(url, httpContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var userLanguages = JsonConvert
                               .DeserializeObject<UserLanguageKnow>
                                   (await response.Content.ReadAsStringAsync());

            Assert.True(userLanguages != null, "Language added");
        }

        [Theory]
        [InlineData("/api/v1/identity/UserLanguageKnow")]
        public async void DeleteUserLanguageKnow_deleteExpectedUserLanguageKnow(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            UserLanguageKnow addLanguageKnow = new UserLanguageKnow
            {
                ApplicationUserId = SeedData.ID_USER_MOCK1.ToString(),
                LanguageKnowId = 1
            };

            // Act
            var response = await client.DeleteAsync($"{url}/{SeedData.ID_USER_MOCK1}/language/{addLanguageKnow.LanguageKnowId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var userLanguages = JsonConvert
                               .DeserializeObject<bool>
                                   (await response.Content.ReadAsStringAsync());

            Assert.True(userLanguages == true, "Language added");
        }
    }
}
