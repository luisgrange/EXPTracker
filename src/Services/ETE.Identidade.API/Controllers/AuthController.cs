using ETE.Identidade.API.Extensions;
using ETE.Identidade.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ETE.Identidade.API.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }   

        [HttpPost("create-account")]
        public async Task<ActionResult> Register(UserRegisterViewModel userRegister)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser { 
                UserName = userRegister.Email, 
                Email = userRegister.Email, 
                EmailConfirmed = true 
            };

            var result = await _userManager.CreateAsync(user, userRegister.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(await GenerateToken(userRegister.Email));

            }

            return BadRequest();
            
        }

        [HttpPost("authentication")]
        public async Task<ActionResult> Login(UserLoginViewModel user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return Ok(await GenerateToken(user.Email));
            }

            return BadRequest();

        }

        private async Task<UserLoginResponseViewModel> GenerateToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach(var role in userRoles)
            {
                claims.Add(new Claim("role", role));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Sender,
                Audience = _appSettings.ValidatedIn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiryHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            var response = new UserLoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiryHours).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaimViewModel { Type = c.Type, Value = c.Value })
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
