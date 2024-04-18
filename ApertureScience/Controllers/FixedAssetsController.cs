using ApertureScience.Data;
using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Relational;
using System.Reflection.Metadata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetsController : ControllerBase
    {
        private readonly IFixedAssetRepository _fixedAssetRepository;
        private readonly IWebHostEnvironment _environment;

        public FixedAssetsController(IFixedAssetRepository fixedAssetRepository, IWebHostEnvironment environment)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _environment = environment;
        }
        // GET: api/<FixedAssetsController>
        [HttpGet]
        public async Task<IActionResult> GetAllFixedAsset()
        {
            return Ok(await _fixedAssetRepository.GetAllFixedAsset());
        }

        // GET api/<FixedAssetsController>/5
        [HttpGet("{name}")]
        public async Task<IActionResult> GetFixedAsset(string name)
        {
            return Ok(await _fixedAssetRepository.GetByName(name));
        }

        // GET api/<FixedAssetsController>/last
        [HttpGet("last")]
        public async Task<IActionResult> GetLAstFixedAssetName()
        {
            return Ok(await _fixedAssetRepository.GetTheLast());
        }

        // POST api/<FixedAssetsController>
        [HttpPost]
        public async Task<IActionResult> CreateFixedAsset([FromBody] FixedAsset fixedAsset)
        {
            Response respCreateFixedAsset = new Response();
            respCreateFixedAsset.responseFixedAssetId = new ResponseFixedAssetId();
            respCreateFixedAsset.conflicts = new List<Conflicts>();

            if (fixedAsset == null || 
                string.IsNullOrWhiteSpace(fixedAsset.Name) ||
                fixedAsset.YearsUsefulLife == 0 ||
                fixedAsset.AcquisitionDate == null ||
                //fixedAsset.ApplicationDate == null ||
                fixedAsset.Invoice == null ||
                string.IsNullOrWhiteSpace(fixedAsset.Description) ||
                //fixedAsset.Amount == null ||
                //fixedAsset.ShipmentCost == null ||
                //fixedAsset.Discount == null ||
                //fixedAsset.SubTotal == null ||
                //fixedAsset.IEPS == null ||
                //fixedAsset.RetentionISR == null ||
                //fixedAsset.IVA == null ||
                //fixedAsset.AditionalCost == null ||
                //fixedAsset.Total == null ||
                //fixedAsset.WarrantyDays == null ||
                //fixedAsset.MaintenanceCost == null ||
                fixedAsset.IdSupplier == 0 ||
                fixedAsset.IdCategory == 0 ||
                fixedAsset.IdDepartments == 0 || 
                fixedAsset.IdBusinessName == 0 || 
                fixedAsset.DepressionPercentage == 0 || 
                //string.IsNullOrWhiteSpace(fixedAsset.AssetClass) ||
                fixedAsset.FixedSAP == null)
            {
                respCreateFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateFixedAsset);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var imagePath = SaveImageAsync(fixedAssetImage, _environment, fixedAsset.Name);

            //fixedAsset.ImageRute = imagePath;

            var created = await _fixedAssetRepository.InsertFixedAsset(fixedAsset);

            if (created == 0)
            {
                respCreateFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateFixedAsset);
            }

            respCreateFixedAsset.responseFixedAssetId.Id = created;
            respCreateFixedAsset.conflicts.Add(new Conflicts { Problems = false, Description = "Activo Creado Correctamente" });

            return Created("created", respCreateFixedAsset);
        }

        // PUT api/<FixedAssetsController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateFixedAsset([FromBody] FixedAsset fixedAsset)
        {
            Response respUpdateFixedAsset = new Response();
            respUpdateFixedAsset.conflicts = new List<Conflicts>();

            if (fixedAsset == null)
            {
                respUpdateFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateFixedAsset);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _fixedAssetRepository.UpdateFixedAsset(fixedAsset);

            if (update == false)
            {
                respUpdateFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateFixedAsset);
            }

            respUpdateFixedAsset.conflicts.Add(new Conflicts { Problems = false, Description = "Activo Actualizado Correctamente" });
            //return NoContent();
            return Ok(respUpdateFixedAsset);
        }

        // PATCH api/<FixedAssetsController>/5
        [HttpPatch]
        public async Task<IActionResult> UpdateFixedAssetDocument([FromForm] FixedAssetDocument FixedAssetDocument)//, string name, int type, int id)
        {
            Response respUpdateFixedAsset = new Response();
            respUpdateFixedAsset.conflicts = new List<Conflicts>();

            if (FixedAssetDocument.FixedAssetImage == null &&
                FixedAssetDocument.FixedAssetContract == null &&
                FixedAssetDocument.FixedAssetWarranty == null)
            {
                respUpdateFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un archivo para Actualizar" });
                return BadRequest(respUpdateFixedAsset);
            }
            if (FixedAssetDocument.Name == null)
            {
                respUpdateFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un nombre para Actualizar" });
                return BadRequest(respUpdateFixedAsset);
            }
            if (FixedAssetDocument.Id == 0)
            {
                respUpdateFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita un ID Valido para Actualizar" });
                return BadRequest(respUpdateFixedAsset);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imagePath = SaveImageAsync(FixedAssetDocument, _environment);//, name, type);

            var update = await _fixedAssetRepository.InsertFixedAssetDocument(imagePath, FixedAssetDocument.Id);

            if (update == false)
            {
                respUpdateFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateFixedAsset);
            }

            respUpdateFixedAsset.conflicts.Add(new Conflicts { Problems = false, Description = "Activo Actualizado Correctamente" });
            //return NoContent();
            return Ok(respUpdateFixedAsset);
        }

        // DELETE api/<FixedAssetsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFixedAsset(int id)
        {
            Response respDeleteFixedAsset = new Response();
            respDeleteFixedAsset.conflicts = new List<Conflicts>();

            var delete = await _fixedAssetRepository.DeleteFixedAsset(new FixedAsset { Id = id });

            if (delete == false)
            {
                respDeleteFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteFixedAsset);
            }
            respDeleteFixedAsset.conflicts.Add(new Conflicts { Problems = false, Description = "Activo Eliminado Correctamente" });
            //return NoContent();
            return Ok(respDeleteFixedAsset);
        }

        private static Dictionary<int, string> SaveImageAsync(FixedAssetDocument imageFile, IWebHostEnvironment webRootPath)//, string name, int tipo)
        {
            var webRootPaths = webRootPath.WebRootPath;
            var path = SaveDocument.SaveImageAsync(imageFile, webRootPaths);//, name, tipo);

            return path;
        }
    }
}
