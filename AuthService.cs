using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

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
    }

    public Task<AuthResponse> RefreshToken(RefreshTokenRequest request)
    {
        throw new NotImplementedException();
    }

    private string GenerateAccessToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Email),
        };
        //TODO ADD ROLES Y PERMISOS
        //GENERATE TOKEN
        //BUILD TOKENRESOPONSE
        return "";
    }
}
