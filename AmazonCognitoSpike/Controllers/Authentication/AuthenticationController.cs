using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using AmazonCognitoSpike.Data;
using AmazonCognitoSpike.Data.Models;
using AmazonCognitoSpike.Services.IAASServices;
using AmazonCognitoSpike.Services.IAASServices.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonCognitoSpike.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DataContext db;
        private readonly IIAASService authService;

        public AuthenticationController(DataContext _db, IIAASService _authService)
        {
            db = _db;
            authService = _authService;
        }

        // POST api/authentication/createUserPool
        [HttpPost]
        [Route("createOrganization", Name = "CreateOrganization")]
        public async Task<ActionResult<string>> CreateOrganization(IAASCreateUserPoolRequest pool)
        {
            try
            {
                var response = await authService.CreateUserPool(pool);
                var newOrg = new Organization
                {
                    OrganizationId = Guid.NewGuid(),
                    CognitoUserPoolId = response.Id,
                    CognitoAuthority = response.Authority,
                    CognitoAudience = response.Audience
                };
                db.Organizations.Add(newOrg);
                db.SaveChanges();
                return Ok(new CreateOrganizationResponse
                {
                    OrganizationId = newOrg.OrganizationId
                });
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }

        }

        // POST api/authentication/register
        [HttpPost]
        [Route("register", Name = "Register")]
        public async Task<ActionResult<string>> Register([FromHeader] Guid orgId, UserRegisterParams user)
        {
            try
            {
                var org = db.Organizations.FirstOrDefault(o => o.OrganizationId == orgId);
                await authService.Register(org.CognitoAudience, user.Email, user.Password);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        // POST api/authentication/signin
        [HttpPost]
        [Route("signin", Name = "SignIn")]
        public async Task<ActionResult<string>> SignIn([FromHeader] Guid orgId, UserSignInParams user)
        {
            try
            {
                var org = db.Organizations.FirstOrDefault(o => o.OrganizationId == orgId);
                var response = await authService.SignIn(org.CognitoUserPoolId, org.CognitoAudience, user.Email, user.Password);
                return Ok(response.Token);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}
