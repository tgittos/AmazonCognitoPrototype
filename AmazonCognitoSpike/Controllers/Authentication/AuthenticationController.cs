using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
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
        private readonly IIAASService authService;

        public AuthenticationController(IIAASService _authService)
        {
            authService = _authService;
        }

        // POST api/authentication/createUserPool
        [HttpPost]
        [Route("createUserPool", Name = "CreateUserPool")]
        public async Task<ActionResult<string>> CreateUserPool(IAASCreateUserPoolRequest pool)
        {
            try
            {
                var response = await authService.CreateUserPool(pool);
                return Ok(response.Id);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }

        }

        // POST api/authentication/register
        [HttpPost]
        [Route("register", Name = "Register")]
        public async Task<ActionResult<string>> Register(UserRegisterParams user)
        {
            try
            {
                await authService.Register(user.UserPoolClientId, user.Email, user.Password);
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
                var response = await authService.SignIn(user.UserPoolId, user.UserPoolClientId, user.Email, user.Password);
                return Ok(response.Token);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}
