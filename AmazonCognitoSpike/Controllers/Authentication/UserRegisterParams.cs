using System;
namespace AmazonCognitoSpike.Controllers.Authentication
{
    public class UserRegisterParams
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserPoolClientId { get; set; }
    }
}
