// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1","My API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[] 
            { 
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

                }
            };
    }
}