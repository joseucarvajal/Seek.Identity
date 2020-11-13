using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SeekQ.Identity.Application.Profile.Profile.ViewModel;
using SeekQ.Identity.Application.Profile.UserLanguageKnow.ViewModel;
using SeekQ.Identity.Models;
using System;
using System.Net.Http;
using System.Text;
using Xunit;

namespace SeekQ.Identity.Test
{
    public class ProfileControllerTest : BaseIntegrationTest<Startup>
    {
        public ProfileControllerTest(WebApplicationFactory<Startup> factory)
            : base(factory)
        {

        }

        [Theory]
        [InlineData("/api/v1/identity/profile")]
        public async void GetUser_getExpectedUser(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{url}/{SeedData.ID_USER_MOCK1}");

            // Assert
            response.EnsureSuccessStatusCode();
            var user = JsonConvert
                               .DeserializeObject<UserViewModel>
                                   (await response.Content.ReadAsStringAsync());

            Assert.True(user != null, "get user profile");
        }

        [Theory]
        [InlineData("/api/v1/identity/profile")]
        public async void UpdateUser_updateExpectedUser(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            ApplicationUser updateUser = new ApplicationUser
            {
                Id = SeedData.ID_USER_MOCK1.ToString(),
                MakeFirstNamePublic = true,
                MakeLastNamePublic = false,
                MakeBirthDatePublic = false,
                NickName = "Dianne",
                FirstName = "Dianne",
                LastName = "Morlotte",
                BirthDate = new DateTime(1988, 1, 31),
                School = "Springfield University",
                Job = "Software engineer",
                About = "Is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
                GenderId = 1
            };

            // Content
            var httpContent = new StringContent(JsonConvert.SerializeObject(updateUser), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync(url, httpContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var user = JsonConvert
                               .DeserializeObject<UserViewModel>
                                   (await response.Content.ReadAsStringAsync());

            Assert.True(user != null, "The user was updated");
        }
    }
}
