using System;
namespace AmazonCognitoSpike.Data.Models
{
    public class Organization
    {
        public Guid OrganizationId { get; set; }
        public string CognitoUserPoolId { get; set; }
        public string CognitoAudience { get; set; }
        public string CognitoAuthority { get; set; }
    }
}
