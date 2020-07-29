using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineGallery.BLL.DTOs.Users;
using OnlineGallery.BLL.DTOs.Users.Authentication;
using OnlineGallery.BLL.Exceptions;
using OnlineGallery.BLL.Options;
using OnlineGallery.BLL.Services.Interfaces;
using OnlineGallery.DAL.Models;

namespace OnlineGallery.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AccountService(
            IOptions<JwtOptions> jwtSettings,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtOptions = jwtSettings.Value;
        }

        public async Task<UserDto> Register(RegistrationRequest request)
        {
            var user = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new IdentityException(result.Errors);

            await _userManager.AddToRoleAsync(user, "User");
            var userFromDb = await _userManager.FindByNameAsync(request.UserName);

            return _mapper.Map<UserDto>(userFromDb);
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null ||
                !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new ObjectNotFoundException("Username or password are incorrect.");

            var roles = await _userManager.GetRolesAsync(user);

            return new AuthenticationResponse {Token = CreateToken(user, roles)};
        }

        private string CreateToken(User user, IEnumerable<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            });

            foreach (var role in roles) claims.AddClaim(new Claim(ClaimTypes.Role, role));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,

                SigningCredentials = new SigningCredentials(key,
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.LifetimeMinutes)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}