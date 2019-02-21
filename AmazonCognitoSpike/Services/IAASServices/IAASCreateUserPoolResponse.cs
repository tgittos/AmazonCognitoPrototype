using System;
namespace AmazonCognitoSpike.Services.IAASServices
{
    public class IAASCreateUserPoolResponse
    {
        public string Id { get; set; }
        public string Audience { get; set; }
        public string Authority { get; set; }
    }
}
