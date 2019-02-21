using System;
using System.Threading.Tasks;
using AmazonCognitoSpike.Controllers.Authentication;

namespace AmazonCognitoSpike.Services.IAASServices
{
    public interface IIAASService
    {
        // TODO: verify user pool ID concept exists across other implementations
        Task<IAASCreateUserPoolResponse> CreateUserPool(IAASCreateUserPoolRequest request);
        Task<IAASUserRegisterResponse> Register(string userPoolClientId, string email, string password);
        // TODO: verify user pool ID concept exists across other implementations
        Task<IAASUserSignInResponse> SignIn(string userPoolId, string userPoolClientId, string email, string password);
        // TODO: verify user pool ID concept exists across other implementations
        Task<IAASUserSignOutResponse> SignOut(string userPoolId, string email);
    }
}
