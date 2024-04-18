using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface ISAPFixedAssetRepository
    {
        Task<Response> CreateFixedAsset(SAPFixedAsset sapFixedAsset, string hola);
        Task<Response> GetTheLast();
        Task<IEnumerable<Response>> GetAllActiveClass();
        Task<Response> UpdateFixedAssetDepreciation(SAPFixedAssetPatch sapFixedAsset, string hola);
    }
}
