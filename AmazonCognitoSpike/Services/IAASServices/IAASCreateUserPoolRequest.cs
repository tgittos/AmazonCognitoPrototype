using System;
namespace AmazonCognitoSpike.Services.IAASServices
{
    public class IAASPasswordPolicy
    {

        public int MinimumLength { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireNumbers { get; set; }
        public bool RequireSymbols { get; set; }
        public bool RequireUppercase { get; set; }
    }

    public class IAASCreateUserPoolRequest
    {
        public string Name { get; set; }
        public IAASPasswordPolicy PasswordPolicy { get; set; }
    }
}
