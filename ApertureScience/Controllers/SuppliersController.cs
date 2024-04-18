using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Relational;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersController(ISupplierRepository supplieRepository)
        {
            _supplierRepository = supplieRepository;
        }

        // GET: api/<Suppliers>
        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            return Ok(await _supplierRepository.GetAllSuppliers());
        }

        // GET api/<Suppliers>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            return Ok(await _supplierRepository.GetById(id));
        }

        // POST api/<Suppliers>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Supplier supplier)
        {
            Response respCreateSupplier = new Response();
            respCreateSupplier.conflicts = new List<Conflicts>();

            if (supplier == null || 
                string.IsNullOrWhiteSpace(supplier.Tradename) ||
                string.IsNullOrWhiteSpace(supplier.Businessname) ||
                string.IsNullOrWhiteSpace(supplier.Code) ||
                string.IsNullOrWhiteSpace(supplier.RFC) ||
                string.IsNullOrWhiteSpace(supplier.Contact) ||
                string.IsNullOrWhiteSpace(supplier.Email))
            {
                respCreateSupplier.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateSupplier);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _supplierRepository.InsertSupplier(supplier);

            if (created == false)
            {
                respCreateSupplier.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateSupplier);
            }

            respCreateSupplier.conflicts.Add(new Conflicts { Problems = false, Description = "Proovedor Creado Correctamente" });

            return Created("created", respCreateSupplier);
        }

        // PUT api/<Suppliers>/5
        [HttpPut]
        public async Task<IActionResult> UpdateSupplier([FromBody] Supplier supplier)
        {
            Response respUpdateSupplier = new Response();
            respUpdateSupplier.conflicts = new List<Conflicts>();

            if (supplier == null)
            {
                respUpdateSupplier.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateSupplier);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _supplierRepository.UpdateSupplier(supplier);

            if (update == false)
            {
                respUpdateSupplier.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateSupplier);
            }

            respUpdateSupplier.conflicts.Add(new Conflicts { Problems = false, Description = "Proveedor Actualizado Correctamente" });
            return Ok(respUpdateSupplier);
        }

        // DELETE api/<Suppliers>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            Response respDeleteSupplier = new Response();
            respDeleteSupplier.conflicts = new List<Conflicts>();

            var delete = await _supplierRepository.DeleteSupplier(new Supplier { Id = id });

            if (delete == false)
            {
                respDeleteSupplier.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteSupplier);
            }
            respDeleteSupplier.conflicts.Add(new Conflicts { Problems = false, Description = "Provedor Eliminado Correctamente" });
            //return NoContent();
            return Ok(respDeleteSupplier);
        }
    
    }
}
