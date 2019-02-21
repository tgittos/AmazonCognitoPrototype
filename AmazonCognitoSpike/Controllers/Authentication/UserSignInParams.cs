using System;
namespace AmazonCognitoSpike.Controllers.Authentication
{
    public class UserSignInParams
    {
        public string Email { get; set; }
        public string Password { get; set; }
        // TODO: save this to the Org record in the API project when integrating
        public string UserPoolId { get; set; }
        // TODO: save this to the Org record in the API project when integrating
        public string UserPoolClientId { get; set; }
    }
}
