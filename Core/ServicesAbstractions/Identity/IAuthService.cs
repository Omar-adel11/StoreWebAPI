using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions.Identity
{
    public interface IAuthService
    {
        Task<UserResultDto> LoginAsync(LoginDto logindto);

        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
    }
}
