using Data.Mongo.Collections;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Account;
using Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    /// <summary>
    /// Account Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILoginLogService _loginLogService;
        public AccountController(IAccountService accountService, ILoginLogService loginLogService)
        {
            _accountService = accountService;
            _loginLogService = loginLogService;
        }

        /// <summary>
        /// Async Authenticate
        /// </summary>
        /// <param name="request">AuthenticationRequest</param>
        [HttpPost("Authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            //auth
            var result = await _accountService.AuthenticateAsync(request);
            if (result.Errors == null || !result.Errors.Any())
            {
                //mongo usage example
                LoginLog log = new LoginLog()
                {
                    LoginTime = DateTime.Now,
                    UserEmail = request.Email
                };
                await _loginLogService.Add(log);
            }
            return Ok(result);
        }

        /// <summary>
        /// Async Register
        /// </summary>
        /// <param name="request">RegisterRequest</param>
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var uri = $"{Request.Scheme}://{Request.Host.Value}";
            return Ok(await _accountService.RegisterAsync(request, uri));
        }

        /// <summary>
        /// Async Confirm email
        /// </summary>
        /// <param name="userId">String</param>
        /// <param name="password">String</param>
        [HttpGet("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string password)
        {
            return Ok(await _accountService.ConfirmEmailAsync(userId, password));
        }

        /// <summary>
        /// Async Forgot password
        /// </summary>
        /// <param name="request">ForgotPasswordRequest</param>
        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var uri = $"{Request.Scheme}://{Request.Host.Value}";
            await _accountService.ForgotPasswordAsync(request, uri);
            return Ok();
        }

        /// <summary>
        /// Async Reset password
        /// </summary>
        /// <param name="request">ResetPasswordRequest</param>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _accountService.ResetPasswordAsync(request));
        }

        /// <summary>
        /// Async Refresh token
        /// </summary>
        /// <param name="request">RefreshTokenRequest</param>
        [HttpPost("Refreshtoken")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request)
        {
            return Ok(await _accountService.RefreshTokenAsync(request));
        }

        /// <summary>
        /// Async Logout
        /// </summary>
        /// <param name="userEmail">string</param>
        [HttpGet("Logout")]
        public async Task<IActionResult> LogoutAsync(string userEmail)
        {
            return Ok(await _accountService.LogoutAsync(userEmail));
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
