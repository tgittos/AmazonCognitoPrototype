using System;
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
        private const string ClientId = "13vons313o3s04lfv68jjc8lqe";
        private readonly RegionEndpoint Region = RegionEndpoint.USEast2;

        public AWSCognitoService()
        {
            Client = new AmazonCognitoIdentityProviderClient(Region);
        }

        public async Task CreateUserPool()
        {
            var request = new CreateUserPoolRequest
            {

            }

            var response = await Client.CreateUserPoolAsync(request);
        }

        public async Task <IAASUserRegisterResponse> Register(string email, string password)
        {
            var request = new SignUpRequest
            {
                ClientId = ClientId,
                Username = email,
                Password = password
            };

            var response = await Client.SignUpAsync(request);

            return new IAASUserRegisterResponse();
        }

        public async Task<IAASUserSignInResponse> SignIn(string userPoolId, string email, string password)
        {
            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = userPoolId,
                ClientId = ClientId,
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
    }
}
