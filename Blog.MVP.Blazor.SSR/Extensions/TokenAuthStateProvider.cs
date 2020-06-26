﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.MVP.Blazor.SSR.Extensions
{
    public class TokenAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;
        public TokenAuthStateProvider(IJSRuntime js)
        {
            _js = js;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userInfo = await _js.GetUserInfoAsync();
            await _js.LogAsync(userInfo);

            if (userInfo.AccessToken == null)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            else
                return new AuthenticationState(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[] { new Claim(ClaimTypes.Name, userInfo.Profile.Name) }, "tokenauth")));
        }
    }
}
