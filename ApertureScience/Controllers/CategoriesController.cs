using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        // GET: api/<CategorysController>
        [HttpGet]
        public async Task<IActionResult> GetAllCategorys()
        {
            return Ok(await _categoryRepository.GetAllCategorys());
        }

        // GET api/<CategorysController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            return Ok(await _categoryRepository.GetById(id));
        }

        // POST api/<CategorysController>
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            Response respCreateCategory = new Response();
            respCreateCategory.conflicts = new List<Conflicts>();

            if (category == null || string.IsNullOrWhiteSpace(category.Name))
            {
                respCreateCategory.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateCategory);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _categoryRepository.InsertCategory(category);

            if (created == false)
            {
                respCreateCategory.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateCategory);
            }

            respCreateCategory.conflicts.Add(new Conflicts { Problems = false, Description = "Categoría Creado Correctamente" });

            return Created("created", respCreateCategory);
        }

        // PUT api/<CategorysController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            Response respUpdateCategory = new Response();
            respUpdateCategory.conflicts = new List<Conflicts>();

            if (category == null)
            {
                respUpdateCategory.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateCategory);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _categoryRepository.UpdateCategory(category);

            if (update == false)
            {
                respUpdateCategory.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateCategory);
            }

            respUpdateCategory.conflicts.Add(new Conflicts { Problems = false, Description = "Categoría Actualizada Correctamente" });
            //return NoContent();
            return Ok(respUpdateCategory);
        }

        // DELETE api/<CategorysController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Response respDeleteCategory = new Response();
            respDeleteCategory.conflicts = new List<Conflicts>();

            var delete = await _categoryRepository.DeleteCategory(new Category { Id = id });

            if (delete == false)
            {
                respDeleteCategory.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteCategory);
            }
            respDeleteCategory.conflicts.Add(new Conflicts { Problems = false, Description = "Categoría Eliminado Correctamente" });

            return Ok(respDeleteCategory);
        }
    }
}
