using System;
using System.Threading.Tasks;
using AmazonCognitoSpike.Controllers.Authentication;

namespace AmazonCognitoSpike.Services.IAASServices
{
    public interface IIAASService
    {
        Task<IAASUserRegisterResponse> Register(string email, string password);
        Task<IAASUserSignInResponse> SignIn(string userPoolId, string email, string password);
    }
}
