using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //创建http客户端
            //IdentityModel这个类库，给httpclient加了扩展方法，实现identity的相关操作
            var client = new HttpClient();
            var disco  = await client.GetDiscoveryDocumentAsync("https://localhost:50012");
            if (disco.IsError)
            {
                System.Console.WriteLine(disco.Error);
                return;
            }
            //请求identityServer来获取token 这里采用的是credential的方式类似于账号密码
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest{
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1",
                
            });
            if (tokenResponse.IsError)
            {
                System.Console.WriteLine(tokenResponse.Error);
                return;
            }
            System.Console.WriteLine(tokenResponse.Json);
            //发送access token去请求api 通常是使用http授权头，可以通过SetBearerToken扩展方法实现
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await apiClient.GetAsync("https://localhost:60011/identity");
            if (!response.IsSuccessStatusCode)
            {
                System.Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine(content);
            }
        }
    }
}
