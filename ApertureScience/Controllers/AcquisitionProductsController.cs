using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquisitionProductsController : ControllerBase
    {
        private readonly IAcquisitionProductRepository _acquisitionProductRepository;

        public AcquisitionProductsController(IAcquisitionProductRepository acquisitionProductRepository)
        {
            _acquisitionProductRepository = acquisitionProductRepository;
        }

        // GET: api/<AcquisitionProductsController>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _acquisitionProductRepository.GetAllProducts());
        }

        // GET api/<AcquisitionProductsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            return Ok(await _acquisitionProductRepository.GetById(id));
        }

        // POST api/<AcquisitionProductsController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionProduct acquisitionProduct)
        {
            Response respCreateAcquisitionProduct = new Response();
            respCreateAcquisitionProduct.conflicts = new List<Conflicts>();

            if (acquisitionProduct == null ||
                string.IsNullOrWhiteSpace(acquisitionProduct.Name) 
                )
            {
                respCreateAcquisitionProduct.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAcquisitionProduct);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _acquisitionProductRepository.InsertProduct(acquisitionProduct);

            if (created == false)
            {
                respCreateAcquisitionProduct.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateAcquisitionProduct);
            }

            respCreateAcquisitionProduct.conflicts.Add(new Conflicts { Problems = false, Description = "Producto Creado Correctamente" });

            return Created("created", respCreateAcquisitionProduct);
        }

        // PUT api/<AcquisitionProductsController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] AcquisitionProduct acquisitionProduct)
        {
            Response respUpdateAcquisitionProduct = new Response();
            respUpdateAcquisitionProduct.conflicts = new List<Conflicts>();

            if (acquisitionProduct == null)
            {
                respUpdateAcquisitionProduct.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateAcquisitionProduct);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _acquisitionProductRepository.UpdateProduct(acquisitionProduct);

            if (update == false)
            {
                respUpdateAcquisitionProduct.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateAcquisitionProduct);
            }

            respUpdateAcquisitionProduct.conflicts.Add(new Conflicts { Problems = false, Description = "Producto Actualizado Correctamente" });
            return Ok(respUpdateAcquisitionProduct);
        }

        // DELETE api/<AcquisitionProductsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Response respDeleteAcquisitionProduct = new Response();
            respDeleteAcquisitionProduct.conflicts = new List<Conflicts>();

            var delete = await _acquisitionProductRepository.DeleteProduct(new AcquisitionProduct { Id = id });

            if (delete == false)
            {
                respDeleteAcquisitionProduct.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteAcquisitionProduct);
            }
            respDeleteAcquisitionProduct.conflicts.Add(new Conflicts { Problems = false, Description = "Producto Eliminado Correctamente" });

            return Ok(respDeleteAcquisitionProduct);
        }
    }
}
