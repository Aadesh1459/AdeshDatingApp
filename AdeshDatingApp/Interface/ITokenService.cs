using AdeshDatingApp.Entities;

namespace AdeshDatingApp.Interface
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
         
    }
}