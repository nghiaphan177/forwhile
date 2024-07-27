using ForWhile.Domain.Entities;

namespace ForWhile.Services.Interface
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
