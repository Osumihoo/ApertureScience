using ApertureScience.Data;
using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Relational;

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SAPFixedAssetsController : ControllerBase
    {
        private readonly ISAPFixedAssetRepository _SAPFixedAssetRepository;

        public SAPFixedAssetsController(ISAPFixedAssetRepository sapFixedAssetRepository)
        {
            _SAPFixedAssetRepository = sapFixedAssetRepository;
        }

        // POST api/<SAPFixedAssetsController>
        [HttpPost]
        public async Task<IActionResult> CreateSAPFixedAsset([FromBody] SAPFixedAsset sapFixedAsset)
        {
            Response respCreateSAPFixedAsset = new Response();
            respCreateSAPFixedAsset.conflicts = new List<Conflicts>();

            if (sapFixedAsset == null ||
                string.IsNullOrWhiteSpace(sapFixedAsset.ItemCode) ||
                string.IsNullOrWhiteSpace(sapFixedAsset.ItemName) || 
                sapFixedAsset.ItemsGroupCode != 141 || 
                string.IsNullOrWhiteSpace(sapFixedAsset.VatLiable) || 
                string.IsNullOrWhiteSpace(sapFixedAsset.PurchaseItem) || 
                string.IsNullOrWhiteSpace(sapFixedAsset.SalesItem) || 
                string.IsNullOrWhiteSpace(sapFixedAsset.InventoryItem) || 
                string.IsNullOrWhiteSpace(sapFixedAsset.AssetItem) || 
                string.IsNullOrWhiteSpace(sapFixedAsset.GLMethod) || 
                string.IsNullOrWhiteSpace(sapFixedAsset.ItemType) || 
                string.IsNullOrWhiteSpace(sapFixedAsset.AssetClass) ||
                sapFixedAsset.ItemDepreciationParameters == null)
            {
                respCreateSAPFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateSAPFixedAsset);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string coockie = await SapLogin();
            var created = await _SAPFixedAssetRepository.CreateFixedAsset(sapFixedAsset, coockie);

            if (created.conflicts != null && created.conflicts.Any(c => c.Problems == true))
            {
                var conflictoConProblemas = created.conflicts.FirstOrDefault(c => c.Problems);
                respCreateSAPFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = conflictoConProblemas.Description });

                return BadRequest(respCreateSAPFixedAsset);
            }

            respCreateSAPFixedAsset.conflicts.Add(new Conflicts { Problems = false, Description = "Activov Fijo Creado Correctamente" });

            return Created("created", respCreateSAPFixedAsset);
        }

        //GET api/<SAPFixedAssetsController>/
        [HttpGet]

        public async Task<IActionResult> GetSAPTheLastFixedAsset()
        {
            return Ok(await _SAPFixedAssetRepository.GetTheLast());
        }

        //GET api/<SAPFixedAssetsController>/
        [HttpGet("activeclass")]

        public async Task<IActionResult> GetSAPActiveClass()
        {
            return Ok(await _SAPFixedAssetRepository.GetAllActiveClass());
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateFixedAssetDepreciation([FromBody] SAPFixedAssetPatch sapFixedAssetPatch)
        {
            Response respCreateSAPFixedAssetDepreciation = new Response();
            respCreateSAPFixedAssetDepreciation.conflicts = new List<Conflicts>();

            if (sapFixedAssetPatch == null ||
                string.IsNullOrWhiteSpace(sapFixedAssetPatch.ItemCode) ||
                sapFixedAssetPatch.ItemDepreciationParameters == null)
            {
                respCreateSAPFixedAssetDepreciation.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateSAPFixedAssetDepreciation);

            }

            foreach (var itemDepreciationParameter in sapFixedAssetPatch.ItemDepreciationParameters)
            {
                // Aquí puedes acceder a cada componente de la lista
                // Por ejemplo, puedes verificar si algún componente es nulo o tiene un valor específico
                if (itemDepreciationParameter == null || 
                    itemDepreciationParameter.FiscalYear == null ||
                    string.IsNullOrWhiteSpace(itemDepreciationParameter.FiscalYear) ||
                    itemDepreciationParameter.DepreciationArea == null ||
                    string.IsNullOrWhiteSpace(itemDepreciationParameter.DepreciationArea) ||
                    itemDepreciationParameter.DepreciationStartDate == null ||
                    string.IsNullOrWhiteSpace(itemDepreciationParameter.DepreciationStartDate))
                {
                    // Si encuentras que un componente no cumple con los requisitos, puedes agregar un conflicto a la lista y luego devolver BadRequest
                    respCreateSAPFixedAssetDepreciation.conflicts.Add(new Conflicts { Problems = true, Description = "Uno o más parámetros de depreciación de artículo son nulos" });
                    return BadRequest(respCreateSAPFixedAssetDepreciation);
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string coockie = await SapLogin();
            var created = await _SAPFixedAssetRepository.UpdateFixedAssetDepreciation(sapFixedAssetPatch, coockie);

            if (created.conflicts != null && created.conflicts.Any(c => c.Problems == true))
            {
                var conflictoConProblemas = created.conflicts.FirstOrDefault(c => c.Problems);
                respCreateSAPFixedAssetDepreciation.conflicts.Add(new Conflicts { Problems = true, Description = conflictoConProblemas.Description });

                return BadRequest(respCreateSAPFixedAssetDepreciation);
            }

            respCreateSAPFixedAssetDepreciation.conflicts.Add(new Conflicts { Problems = false, Description = "Fecha de Apertura Actualizada Correctamente" });

            return Created("created", respCreateSAPFixedAssetDepreciation);
        }

        private async Task<string> SapLogin()
        {
            // Crear una instancia de la clase SAPLogin
            var sapLoginInstance = new SAPLogin();

            // Llamar al método SAPLog en la instancia creada
            var cookie = await sapLoginInstance.SAPLog();

            return cookie;
        }

    }
}
