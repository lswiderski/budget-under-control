using Autofac;
using BudgetUnderControl.MobileDomain.Repositiories;
using BudgetUnderControl.Mobile.Keys;
using BudgetUnderControl.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using BudgetUnderControl.CommonInfrastructure.Settings;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace BudgetUnderControl.Mobile.Services
{
    public class LoginMobileService : ILoginMobileService
    {

        private readonly HttpClient httpClient;
        private readonly GeneralSettings settings;

        private readonly INavigationViewModel navigationViewModel;
        private readonly ISyncMobileService syncMobileService;
        private readonly IUserRepository userRepository;

        public LoginMobileService(GeneralSettings settings, INavigationViewModel navigationViewModel, ISyncMobileService syncMobileService,
            IUserRepository userRepository)
        {
            this.httpClient = App.Container.ResolveNamed<HttpClient>("api");
            this.settings = settings;
            this.navigationViewModel = navigationViewModel;
            this.syncMobileService = syncMobileService;
            this.userRepository = userRepository;
        }

        public async Task<bool> LoginAsync(string username, string password, bool clearLocalData)
        {
            // login
            var token = await RemoteLoginAsync(username, password);
            //if logged
            if(!string.IsNullOrEmpty(token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken validatedToken;
                var tokenValidationParameters = new TokenValidationParameters();
                var readToken =  tokenHandler.ReadJwtToken(token);
                var claim = readToken.Claims.First(c => c.Type == "unique_name");

                var userId = Guid.Parse(claim.Value);
                Preferences.Set(PreferencesKeys.JWTTOKEN, token);
                Preferences.Set(PreferencesKeys.IsUserLogged, true);
                Preferences.Set(PreferencesKeys.UserExternalId, userId.ToString());
                navigationViewModel.RefreshUserButtons();

                if (clearLocalData)
                {
                    //clear DB
                    await syncMobileService.CleanDataBaseAsync();
                }

                //set externalId
                var user = await this.userRepository.GetFirstUserAsync();
                user.EditExternalId(userId.ToString());
                await userRepository.UpdateUserAsync(user);

                //sync
                //await syncMobileService.SyncAsync();

                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task LogoutAsync(Type redirectToPage)
        {
            await this.LogoutAsync();
            App.MasterPage.NavigateTo(redirectToPage);
        }
        public async Task LogoutAsync()
        {
            Preferences.Set(PreferencesKeys.IsUserLogged, false);
            Preferences.Remove(PreferencesKeys.UserExternalId);
            Preferences.Remove(PreferencesKeys.JWTTOKEN);
            navigationViewModel.RefreshUserButtons();
        }

        private async Task<string> RemoteLoginAsync(string username, string password)
        {
            //call api
            try
            {
                var url = "login/mobile";
                var dataAsString = JsonConvert.SerializeObject(new {Username = username, Password = password });
                var content = new StringContent(dataAsString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("Api-Key", settings.ApiKey);
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                var response = await httpClient.PostAsync(url, content);
                if(!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }
               var statusCode = response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                {
                    var token = await reader.ReadToEndAsync();
                    return token;
                }
            }
            catch (Exception e)
            {
                //just for development purpose
                throw e;
            }
        }
    }
}
