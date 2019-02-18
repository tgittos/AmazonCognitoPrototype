using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

namespace AmazonCognitoSpike.Services
{
    public class AWSCognitoService
    {
        private AmazonCognitoIdentityProviderClient Client;

        // TODO: move these to appsettings.json, read from there
        private const string ClientId = "13vons313o3s04lfv68jjc8lqe";
        private readonly RegionEndpoint Region = RegionEndpoint.USEast2;

        public AWSCognitoService()
        {
            Client = new AmazonCognitoIdentityProviderClient(Region);
        }

        public async Task<SignUpResponse> RegisterUser(string email, string password)
        {
            var request = new SignUpRequest
            {
                ClientId = ClientId,
                Username = email,
                Password = password
            };

            return await Client.SignUpAsync(request);
        }

        public async Task<AdminInitiateAuthResponse> SignIn(string userPoolId, string email, string password)
        {
            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = userPoolId,
                ClientId = ClientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
            };

            request.AuthParameters.Add("USERNAME", email);
            request.AuthParameters.Add("PASSWORD", password);

            return await Client.AdminInitiateAuthAsync(request);
        }
    }
}
