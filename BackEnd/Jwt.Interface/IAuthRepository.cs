using Jwt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Interface
{
    public interface IAuthRepository
    {
        Users FindByUserEmail(string email);
        void DisableGeneratedToken(string email);
        Users EnableGeneretedToken(string email);
        bool HasToken(string email);
    }
}
