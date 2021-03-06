﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

namespace AmazonCognitoSpike.Services.IAASServices.Implementations
{
    public class AWSCognitoService : IIAASService
    {
        private readonly AmazonCognitoIdentityProviderClient Client;

        // TODO: move these to appsettings.json, read from there
        private readonly RegionEndpoint Region = RegionEndpoint.USEast2;
        private readonly string BaseAuthority = "https://cognito-idp.us-east-2.amazonaws.com";

        private readonly List<string> SupportedAuthFlows = new List<string>
        {
            AuthFlowType.ADMIN_NO_SRP_AUTH
        };

        public AWSCognitoService()
        {
            Client = new AmazonCognitoIdentityProviderClient(Region);
            
        }

        public async Task<IAASCreateUserPoolResponse> CreateUserPool(IAASCreateUserPoolRequest createRequest)
        {
            var request = new CreateUserPoolRequest
            {
                PoolName = createRequest.Name,
                Policies = new UserPoolPolicyType
                {
                    PasswordPolicy = new PasswordPolicyType
                    {
                        MinimumLength = createRequest.PasswordPolicy.MinimumLength,
                        RequireLowercase = createRequest.PasswordPolicy.RequireLowercase,
                        RequireNumbers = createRequest.PasswordPolicy.RequireNumbers,
                        RequireSymbols = createRequest.PasswordPolicy.RequireSymbols,
                        RequireUppercase = createRequest.PasswordPolicy.RequireUppercase
                    }
                }
            };

            var response = await Client.CreateUserPoolAsync(request);

            var clientId = await CreateUserPoolClient($"{createRequest.Name} Prototype Client", response.UserPool.Id);

            return new IAASCreateUserPoolResponse
            {
                Id = response.UserPool.Id,
                Audience = clientId,
                Authority = $"{BaseAuthority}/{response.UserPool.Id}"
            };
        }

        public async Task <IAASUserRegisterResponse> Register(string userPoolClientId, string email, string password)
        {
            var request = new SignUpRequest
            {
                ClientId = userPoolClientId,
                Username = email,
                Password = password
            };

            var response = await Client.SignUpAsync(request);

            return new IAASUserRegisterResponse();
        }

        public async Task<IAASUserSignInResponse> SignIn(string userPoolId, string userPoolClientId, string email, string password)
        {
            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = userPoolId,
                ClientId = userPoolClientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
            };

            request.AuthParameters.Add("USERNAME", email);
            request.AuthParameters.Add("PASSWORD", password);

            var response = await Client.AdminInitiateAuthAsync(request);

            return new IAASUserSignInResponse
            {
                Token = response.AuthenticationResult.IdToken
            };
        }

        public async Task<IAASUserSignOutResponse> SignOut(string userPoolId, string email)
        {
            var request = new AdminUserGlobalSignOutRequest
            {
                UserPoolId = userPoolId,
                Username = email
            };

            var response = await Client.AdminUserGlobalSignOutAsync(request);

            return new IAASUserSignOutResponse();
        }

        private async Task<string> CreateUserPoolClient(string clientName, string userPoolId)
        {
            var request = new CreateUserPoolClientRequest
            {
                ClientName = clientName,
                UserPoolId = userPoolId,
                ExplicitAuthFlows = SupportedAuthFlows
            };

            var response = await Client.CreateUserPoolClientAsync(request);

            return response.UserPoolClient.ClientId;
        }
    }
}
