using System;

namespace ComprasVentas;

public interface IUniqueNameChecker
{
    Task<bool> ExistUsernameAsync(string value);
}
