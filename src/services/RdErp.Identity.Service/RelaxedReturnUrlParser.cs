// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace RdErp.Identity.Service
{
    public class RelaxedReturnUrlParser : IReturnUrlParser
    {
        private readonly IAuthorizeRequestValidator _validator;
        private readonly IUserSession _userSession;
        private readonly ILogger _logger;

        public RelaxedReturnUrlParser(
            IAuthorizeRequestValidator validator,
            IUserSession userSession,
            ILogger<RelaxedReturnUrlParser> logger)
        {
            _validator = validator;
            _userSession = userSession;
            _logger = logger;
        }

        public async Task<AuthorizationRequest> ParseAsync(string returnUrl)
        {
            _logger.LogInformation($"Parsing URL {returnUrl}");
            if (IsValidReturnUrl(returnUrl))
            {
                var parameters = returnUrl.ReadQueryStringAsNameValueCollection();
                var user = await _userSession.GetUserAsync();
                var result = await _validator.ValidateAsync(parameters, user);
                if (!result.IsError)
                {
                    _logger.LogTrace("AuthorizationRequest being returned");

                    var request = result.ValidatedRequest;
                    var authorizationRequest = new AuthorizationRequest
                    {
                        ClientId = request.ClientId,
                        RedirectUri = request.RedirectUri,
                        DisplayMode = request.DisplayMode,
                        UiLocales = request.UiLocales,
                        IdP = request.GetIdP(),
                        Tenant = request.GetTenant(),
                        LoginHint = request.LoginHint,
                        PromptMode = request.PromptMode,
                        AcrValues = request.GetAcrValues(),
                        ScopesRequested = request.RequestedScopes,
                    };

                    authorizationRequest.Parameters.Add(request.Raw);

                    return authorizationRequest;
                }
            }

            _logger.LogTrace("No AuthorizationRequest being returned");
            return null;
        }

        public bool IsValidReturnUrl(string returnUrl)
        {
            return true;
        }
    }

    public static class Helpers
    {
        public static NameValueCollection ReadQueryStringAsNameValueCollection(this string url)
        {
            if (url != null)
            {
                var idx = url.IndexOf('?');
                if (idx >= 0)
                {
                    url = url.Substring(idx + 1);
                }
                var query = QueryHelpers.ParseNullableQuery(url);
                if (query != null)
                {
                    return query.AsNameValueCollection();
                }
            }

            return new NameValueCollection();
        }
    }
}