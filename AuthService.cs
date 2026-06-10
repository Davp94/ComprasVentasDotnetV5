using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ComprasVentas;

public class AuthService(AppDbContext appDbContext, IConfiguration configuration) : IAuthService
{

    private readonly AppDbContext _context = appDbContext;

    private readonly IConfiguration _configuration = configuration;

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var usuario = await _context.Usuarios.Include(u=>u.Roles).FirstOrDefaultAsync(u => u.Email == request.Email);

        if(usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password))
        {
            return null;
        }

        var accessToken = GenerateAccessToken(usuario);
        var refreshToken = GenerateRefreshToken();
        usuario.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            Expires =  DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenDurationInDays")),
            IsActive = true,
            UsuarioId = usuario.Id
        });
        await _context.SaveChangesAsync();

        var roles = usuario.Roles.Select(r=>r.Nombre).ToList();
        var permisos = usuario.Roles.SelectMany(r=>r.Permisos).Select(p=>p.Nombre).ToList();
        var expiration = _configuration.GetValue<int>("Jwt:DurationInMinutes");
        return new AuthResponse(
            accessToken,
            refreshToken,
            usuario.Id,
            expiration,
            usuario.Email,
            roles,
            permisos
        );
    }

    public async Task<AuthResponse> RefreshToken(RefreshTokenRequest request)
    {
        var usuario = await _context.Usuarios
            .Include(u=>u.RefreshTokens)
            .Include(u=>u.Roles)
            .FirstOrDefaultAsync(u=>u.RefreshTokens.Any(t => t.Token == request.RefreshToken));

        if(usuario == null) return null;

        var refreshToken = usuario.RefreshTokens.Single(x=>x.Token == request.RefreshToken);
        if(!refreshToken.IsActive) return null;

        var newAccessToken = GenerateAccessToken(usuario);
        var newRefreshToken = GenerateRefreshToken();

        refreshToken.IsActive = false;
        usuario.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            Expires =  DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenDurationInDays")),
            IsActive = true,
            UsuarioId = usuario.Id
        }); 
        await _context.SaveChangesAsync();  
          var roles = usuario.Roles.Select(r=>r.Nombre).ToList();
        var permisos = usuario.Roles.SelectMany(r=>r.Permisos).Select(p=>p.Nombre).ToList();
        var expiration = _configuration.GetValue<int>("Jwt:DurationInMinutes");
        return new AuthResponse(
            newAccessToken,
            newRefreshToken,
            usuario.Id,
            expiration,
            usuario.Email,
            roles,
            permisos
        ); 
    }

    private string GenerateAccessToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Email),
        };
        foreach (var rol in usuario.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, rol.Nombre));
            foreach (var permiso in rol.Permisos)
            {
                claims.Add(new Claim("Permission", permiso.Nombre));
            }
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:DurationInMinutes"));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: cred
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

     private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        return Convert.ToBase64String(randomNumber); 
    }
}
