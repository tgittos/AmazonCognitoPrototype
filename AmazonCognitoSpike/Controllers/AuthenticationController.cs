using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using AmazonCognitoSpike.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazonCognitoSpike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public class UserRegisterParams
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class UserSignInParams
        {
            public string Email { get; set; }
            public string Password { get; set; }
            // TODO: save this to the Org record in the API project when integrating
            public string UserPoolId { get; set; }
        }

        private readonly AWSCognitoService Cognito;

        public AuthenticationController(AWSCognitoService _cognito)
        {
            Cognito = _cognito;
        }

        // POST api/authentication/register
        [HttpPost]
        [Route("register", Name = "Register")]
        public async Task<ActionResult<string>> Register(UserRegisterParams user)
        {
            try
            {
                var response = await Cognito.RegisterUser(user.Email, user.Password);

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
        public async Task<ActionResult<string>> SignIn(UserSignInParams user)
        {
            try
            {
                var response = await Cognito.SignIn(user.UserPoolId, user.Email, user.Password);

                return Ok(response.AuthenticationResult.IdToken);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}
