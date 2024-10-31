using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ServiceLayer;
using NamoriTravel.Models;
using ModelsDTO;

namespace NamoriTravel.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IServiceManager _serviceManager;
        public AccountController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        #region //-------------------MVC Web APP---------//
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Passwords do not match.");
                    return View(model);
                }

                var registerDto = new RegisterDto
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password
                };

                try
                {
                    var token = await _serviceManager.authService.RegisterAsync(registerDto);
                    // Decode the token to extract the UserID
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    var nameIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
                    if (nameIdClaim != null)
                    {
                        HttpContext.Session.SetString("UserID", nameIdClaim.Value);
                    }
                    HttpContext.Session.SetString("jwtToken", token);
                    await _serviceManager.loggingService.LogAuditAsync(null, "AccountController", "Register", $"User {registerDto.Username} registered.");
                    return RedirectToAction("Index", "Dashboard");
                }
                catch (Exception ex)
                {
                    await _serviceManager.loggingService.LogErrorAsync(ex, "Error during registration", null);
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginDto = new LoginDto
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password
                };

                try
                {
                    var token = await _serviceManager.authService.LoginAsync(loginDto);
                    // Decode the token to extract the UserID
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    var nameIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
                    if (nameIdClaim != null)
                    {
                        HttpContext.Session.SetString("UserID", nameIdClaim.Value);
                        HttpContext.Session.SetString("All_PageList", JsonSerializer.Serialize(await _serviceManager.pageService.GetAllPagesAsync(Convert.ToInt32(nameIdClaim.Value))));
                    }
                    HttpContext.Session.SetString("jwtToken", token);
                    await _serviceManager.loggingService.LogAuditAsync(Convert.ToInt32(nameIdClaim.Value), "AccountController", "Login", $"User {loginDto.Username} logged in.");
                    return RedirectToAction("Index", "Dashboard");
                }
                catch (Exception ex)
                {
                    await _serviceManager.loggingService.LogErrorAsync(ex, "Error during login", null);
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            // Clear the session or cookie
            HttpContext.Session.Remove("jwtToken");
            HttpContext.Session.Remove("UserID");
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region //------------------WEB API--------------//
        [HttpPost("register")]
        public async Task<IActionResult> ApiRegister([FromBody] RegisterDto registerDto)
        {
            try
            {
                var token = await _serviceManager.authService.RegisterAsync(registerDto);
                // Decode the token to extract the UserID
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var nameIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
                if (nameIdClaim != null)
                {
                    HttpContext.Session.SetString("UserID", nameIdClaim.Value);
                }
                HttpContext.Session.SetString("jwtToken", token);
                await _serviceManager.loggingService.LogAuditAsync(null, "AccountController", "ApiRegister", $"API user {registerDto.Username} registered.");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error during API registration", null);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> ApiLogin([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _serviceManager.authService.LoginAsync(loginDto);
                // Decode the token to extract the UserID
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var nameIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
                if (nameIdClaim != null)
                {
                    HttpContext.Session.SetString("UserID", nameIdClaim.Value);
                }
                HttpContext.Session.SetString("jwtToken", token);
                await _serviceManager.loggingService.LogAuditAsync(null, "AccountController", "ApiLogin", $"API user {loginDto.Username} logged in.");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                await _serviceManager.loggingService.LogErrorAsync(ex, "Error during API login", null);
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion
    }
}
