// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;
namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                // new IdentityResources.Address(),
                // new IdentityResources.Phone(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1","My API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[] 
            { 
                //机器人到机器人客户端 没有人为的交互(form例子1)
                new Client
                {
                    //clientID 和 clientSecrets 这种方式可以理解为登陆账号和密码
                    ClientId = "client",
                    //没有交互的用户，通过 clientid/secrit 作为认证方式
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //认证方式的密码
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    //client 能访问的 scopes
                    AllowedScopes = {"api1"}

                },
                //交互式的 asp.net core mvc 客户端
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    //登入后重定向链接
                    RedirectUris = {"https://localhost:5001/signin-oidc"},
                    //登出后重定向的地址
                    PostLogoutRedirectUris = {"https://localhost:5001/signout-callback-oidc"},

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        // IdentityServerConstants.StandardScopes.Address,
                        // IdentityServerConstants.StandardScopes.Phone,
                    }
                },
                //同时使用opid 用户认证 和 OAuth2.0 api访问
                new Client 
                {
                    ClientId = "opidAndApi",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    //登入后重定向链接
                    RedirectUris = {"https://localhost:5001/signin-oidc"},
                    //登出后重定向的地址
                    PostLogoutRedirectUris = {"https://localhost:5001/signout-callback-oidc"},
                    //允许 refresh tokens 刷新令牌
                    AllowOfflineAccess = true,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                        
                    }

                }
            };
    }
}