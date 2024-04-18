using ApertureScience.Data;
using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SAPAssetRetirementController : ControllerBase
    {
        private readonly ISAPAssetRetirementRepository _SAPAssetRetirementRepository;

        public SAPAssetRetirementController(ISAPAssetRetirementRepository sapAssetRetirementRepository)
        {
            _SAPAssetRetirementRepository = sapAssetRetirementRepository;
        }

        // POST api/<SAPAssetRetirementController>
        [HttpPost]
        public async Task<IActionResult> CreateSAPAssetRetirement([FromBody] SAPAssetRetirement sapAssetRetirement)
        {
            Response respCreateSAPAssetRetirement = new Response();
            respCreateSAPAssetRetirement.conflicts = new List<Conflicts>();

            
            if (sapAssetRetirement == null || sapAssetRetirement.AssetDocumentLineCollection == null)
            {
                respCreateSAPAssetRetirement.conflicts.Add(new Conflicts { Problems = true, Description = "Las listas estan vaacías" });
                return BadRequest(respCreateSAPAssetRetirement);
            }

            foreach (var assetDocumentLineCollection in sapAssetRetirement.AssetDocumentLineCollection)
            {
                if (assetDocumentLineCollection == null ||
                    string.IsNullOrWhiteSpace(assetDocumentLineCollection.AssetNumber) ||
                    assetDocumentLineCollection.Quantity == 0 ||
                    assetDocumentLineCollection.TotalLC == 0)
                {
                    respCreateSAPAssetRetirement.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                    return BadRequest(respCreateSAPAssetRetirement);
                }
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string coockie = await SapLogin();
            var created = await _SAPAssetRetirementRepository.CreateAssetRetirement(sapAssetRetirement, coockie);

            if (created.conflicts != null && created.conflicts.Any(c => c.Problems == true))
            {
                var conflictoConProblemas = created.conflicts.FirstOrDefault(c => c.Problems);
                respCreateSAPAssetRetirement.conflicts.Add(new Conflicts { Problems = true, Description = conflictoConProblemas.Description });

                return BadRequest(respCreateSAPAssetRetirement);
            }

            respCreateSAPAssetRetirement.conflicts.Add(new Conflicts { Problems = false, Description = "Activov Fijo Dado de Baja Correctamente" });

            return Created("created", respCreateSAPAssetRetirement);
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
