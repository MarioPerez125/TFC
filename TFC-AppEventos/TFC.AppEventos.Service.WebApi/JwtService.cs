namespace TFC.AppEventos.Service.WebApi
{
    using global::TFC.AppEventos.Application.DTO;
    using global::TFC.AppEventos.Transversal.Utils;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    namespace TFC.AppEventos.Application.Services
    {
        public class JwtService
        {
            private readonly string _secretKey;
            private readonly string _issuer;
            private readonly string _audience;
            private readonly string _expireMinutes;

            public JwtService(IConfiguration configuration)
            {
                _secretKey = configuration["Jwt:Key"];
                _issuer = configuration["Jwt:Issuer"];
                _audience = configuration["Jwt:Audience"];
                _expireMinutes = configuration["Jwt:ExpireMinutes"];
            }

            public string GenerateToken(UserDto user)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

                var token = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_expireMinutes)),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            public BaseResponse ValidateToken(string token)
            {
                var response = new BaseResponse();

                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_secretKey);

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = _issuer,
                        ValidateAudience = true,
                        ValidAudience = _audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    response.IsSuccess = true;
                    response.Message = "Token válido";
                    response.ResponseCode = ResponseCodes.OK;
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = $"Token inválido: {ex.Message}";
                    response.ResponseCode = ResponseCodes.ERROR_BAD_PASSWORD;
                }

                return response;
            }
        }
    }
}
