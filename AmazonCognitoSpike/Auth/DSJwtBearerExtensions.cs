// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using AmazonCognitoSpike.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AmazonCognitoSpike.Auth
{
    public static class JwtBearerExtensions
    {
        public static AuthenticationBuilder DSAddJwtBearer(this AuthenticationBuilder builder)
            => builder.DSAddJwtBearer(JwtBearerDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder DSAddJwtBearer(this AuthenticationBuilder builder, Action<JwtBearerOptions> configureOptions)
            => builder.DSAddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder DSAddJwtBearer(this AuthenticationBuilder builder, string authenticationScheme, Action<JwtBearerOptions> configureOptions)
            => builder.DSAddJwtBearer(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder DSAddJwtBearer(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<JwtBearerOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());
            return builder.AddScheme<JwtBearerOptions, JwtBearerHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
