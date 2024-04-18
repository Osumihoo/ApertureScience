using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionCategoriesController : ControllerBase
    {
        private readonly IAcquisitionCategoryRepository _acquisitionCategoryRepository;

        public AcquisitionCategoriesController(IAcquisitionCategoryRepository acquisitionCategoryRepository)
        {
            _acquisitionCategoryRepository = acquisitionCategoryRepository;
        }
        // GET: api/<AcquisitionCategoriesController>
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _acquisitionCategoryRepository.GetAllCategories());
        }

        // GET api/<AcquisitionCategoriesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            return Ok(await _acquisitionCategoryRepository.GetById(id));
        }

        // POST api/<AcquisitionCategoriesController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionCategory acquisitionCategory)
        {
            Response respCreateAcquisitionCategory = new Response();
            respCreateAcquisitionCategory.conflicts = new List<Conflicts>();

            if (acquisitionCategory == null ||
                string.IsNullOrWhiteSpace(acquisitionCategory.Name)
                )
            {
                respCreateAcquisitionCategory.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAcquisitionCategory);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionCategoryRepository.InsertCategory(acquisitionCategory);

            if (created == false)
            {
                respCreateAcquisitionCategory.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateAcquisitionCategory);
            }

            respCreateAcquisitionCategory.conflicts.Add(new Conflicts { Problems = false, Description = "Categoría Creado Correctamente" });

            return Created("created", respCreateAcquisitionCategory);
        }

        // PUT api/<AcquisitionCategoriesController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] AcquisitionCategory acquisitionCategory)
        {
            Response respUpdateAcquisitionCategory = new Response();
            respUpdateAcquisitionCategory.conflicts = new List<Conflicts>();

            if (acquisitionCategory == null)
            {
                respUpdateAcquisitionCategory.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateAcquisitionCategory);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _acquisitionCategoryRepository.UpdateCategory(acquisitionCategory);

            if (update == false)
            {
                respUpdateAcquisitionCategory.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateAcquisitionCategory);
            }

            respUpdateAcquisitionCategory.conflicts.Add(new Conflicts { Problems = false, Description = "Categoría Actualizado Correctamente" });
            return Ok(respUpdateAcquisitionCategory);
        }

        // DELETE api/<AcquisitionCategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Response respDeleteAcquisitionCategory = new Response();
            respDeleteAcquisitionCategory.conflicts = new List<Conflicts>();

            var delete = await _acquisitionCategoryRepository.DeleteCategory(new AcquisitionCategory { Id = id });

            if (delete == false)
            {
                respDeleteAcquisitionCategory.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteAcquisitionCategory);
            }
            respDeleteAcquisitionCategory.conflicts.Add(new Conflicts { Problems = false, Description = "Categoría Eliminado Correctamente" });

            return Ok(respDeleteAcquisitionCategory);
        }
    }
}
