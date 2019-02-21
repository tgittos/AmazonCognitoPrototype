using System;
namespace AmazonCognitoSpike.Auth
{
    public interface IAudienceAuthorityResolver
    {
        void Resolve(Guid organizationId, JwtBearerOptions options);
    }
}
