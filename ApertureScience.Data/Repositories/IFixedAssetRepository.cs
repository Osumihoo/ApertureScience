using ApertureScience.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IFixedAssetRepository
    {
        Task<IEnumerable<FixedAsset>> GetAllFixedAsset();
        Task<FixedAsset> GetByName(string name);
        Task<int> InsertFixedAsset(FixedAsset fixedAsset);
        Task<bool> UpdateFixedAsset(FixedAsset fixedAsset);
        Task<bool> InsertFixedAssetDocument(Dictionary<int, string> documentRute, int id);
        Task<bool> DeleteFixedAsset(FixedAsset fixedAsset);
        Task<ResponseFixedAssetName> GetTheLast();
    }
}
