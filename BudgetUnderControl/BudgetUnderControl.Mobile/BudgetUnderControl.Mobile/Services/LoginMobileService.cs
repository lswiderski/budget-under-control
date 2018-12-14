using Autofac;
using BudgetUnderControl.Infrastructure.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public class LoginMobileService : ILoginMobileService
    {

        private readonly HttpClient httpClient;
        private readonly GeneralSettings settings;

        public LoginMobileService(GeneralSettings settings)
        {
            this.httpClient = App.Container.ResolveNamed<HttpClient>("api");
            this.settings = settings;
        }

        public async Task<bool> LoginAsync(string username, string password, bool clearLocalData)
        {
            // login
            var userExternalId = await RemoteLoginAsync(username, password);
            //if logged
            if(userExternalId != Guid.Empty)
            {
                if (clearLocalData)
                {
                    //clear DB
                }

                //sync

                return true;
            }
            else
            {
                return false;
            }

        }

       private async Task<Guid> RemoteLoginAsync(string username, string password)
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
                    return Guid.Empty;
                }
               var statusCode = response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                using (var json = new JsonTextReader(reader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var apiResponse = serializer.Deserialize<Guid>(json);

                    return apiResponse;
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
