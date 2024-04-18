using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionBrandsController : ControllerBase
    {
        private readonly IAcquisitionBrandRepository _acquisitionBrandRepository;

        public AcquisitionBrandsController(IAcquisitionBrandRepository acquisitionBrandRepository)
        {
            _acquisitionBrandRepository = acquisitionBrandRepository;
        }
        
        // GET: api/<AcquisitionBrandsController>
        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            return Ok(await _acquisitionBrandRepository.GetAllBrands());
        }

        // GET api/<AcquisitionBrandsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            return Ok(await _acquisitionBrandRepository.GetById(id));
        }

        // POST api/<AcquisitionBrandsController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionBrand acquisitionBrand)
        {
            Response respCreateAcquisitionBrand = new Response();
            respCreateAcquisitionBrand.conflicts = new List<Conflicts>();

            if (acquisitionBrand == null ||
                string.IsNullOrWhiteSpace(acquisitionBrand.Name)
                )
            {
                respCreateAcquisitionBrand.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAcquisitionBrand);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionBrandRepository.InsertBrand(acquisitionBrand);

            if (created == false)
            {
                respCreateAcquisitionBrand.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateAcquisitionBrand);
            }

            respCreateAcquisitionBrand.conflicts.Add(new Conflicts { Problems = false, Description = "Marca Creado Correctamente" });

            return Created("created", respCreateAcquisitionBrand);
        }

        // PUT api/<AcquisitionBrandsController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateBrand([FromBody] AcquisitionBrand acquisitionBrand)
        {
            Response respUpdateAcquisitionBrand = new Response();
            respUpdateAcquisitionBrand.conflicts = new List<Conflicts>();

            if (acquisitionBrand == null)
            {
                respUpdateAcquisitionBrand.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateAcquisitionBrand);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _acquisitionBrandRepository.UpdateBrand(acquisitionBrand);

            if (update == false)
            {
                respUpdateAcquisitionBrand.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateAcquisitionBrand);
            }

            respUpdateAcquisitionBrand.conflicts.Add(new Conflicts { Problems = false, Description = "Marca Actualizado Correctamente" });
            return Ok(respUpdateAcquisitionBrand);
        }

        // DELETE api/<AcquisitionBrandsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            Response respDeleteAcquisitionBrand = new Response();
            respDeleteAcquisitionBrand.conflicts = new List<Conflicts>();

            var delete = await _acquisitionBrandRepository.DeleteBrand(new AcquisitionBrand { Id = id });

            if (delete == false)
            {
                respDeleteAcquisitionBrand.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteAcquisitionBrand);
            }
            respDeleteAcquisitionBrand.conflicts.Add(new Conflicts { Problems = false, Description = "Marca Eliminado Correctamente" });

            return Ok(respDeleteAcquisitionBrand);
        }
    }
}
