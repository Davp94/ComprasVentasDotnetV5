using System;
using Microsoft.AspNetCore.Identity.Data;

namespace ComprasVentas;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);

    Task<AuthResponse> RefreshToken(RefreshTokenRequest request);
}
