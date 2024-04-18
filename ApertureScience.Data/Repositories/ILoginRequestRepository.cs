using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface ILoginRequestRepository
    {
        Task<LoginRequest> GetByEmail(LoginRequest loginRequest);
    }
}
