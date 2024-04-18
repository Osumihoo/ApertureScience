using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressesController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        // GET: api/<AddressesController>
        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            return Ok(await _addressRepository.GetAllAddresses());
        }

        // GET api/<AddressesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress(int id)
        {
            return Ok(await _addressRepository.GetById(id));
        }

        // POST api/<AddressesController>
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] Address address)
        {
            Response respCreateAddress = new Response();
            respCreateAddress.conflicts = new List<Conflicts>();

            if (address == null || 
                string.IsNullOrWhiteSpace(address.Street) ||
                string.IsNullOrWhiteSpace(address.Neighborhood) ||
                string.IsNullOrWhiteSpace(address.City) ||
                string.IsNullOrWhiteSpace(address.PC) ||
                string.IsNullOrWhiteSpace(address.State) ||
                string.IsNullOrWhiteSpace(address.Country) ||
                string.IsNullOrWhiteSpace(address.Num) )
            {
                respCreateAddress.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateAddress);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _addressRepository.InsertAddress(address);

            if (created == false)
            {
                respCreateAddress.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateAddress);
            }

            respCreateAddress.conflicts.Add(new Conflicts { Problems = false, Description = "Dirección Creado Correctamente" });

            return Created("created", respCreateAddress);
        }

        // PUT api/<AddressesController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateAddress([FromBody] Address address)
        {
            Response respUpdateAddress = new Response();
            respUpdateAddress.conflicts = new List<Conflicts>();

            if (address == null)
            {
                respUpdateAddress.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateAddress);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _addressRepository.UpdateAddress(address);

            if (update == false)
            {
                respUpdateAddress.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateAddress);
            }

            respUpdateAddress.conflicts.Add(new Conflicts { Problems = false, Description = "Dirección Actualizada Correctamente" });
            //return NoContent();
            return Ok(respUpdateAddress);
        }

        // DELETE api/<AddressesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            Response respDeleteAddress = new Response();
            respDeleteAddress.conflicts = new List<Conflicts>();

            var delete = await _addressRepository.DeleteAddress(new Address { Id = id });

            if (delete == false)
            {
                respDeleteAddress.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteAddress);
            }
            respDeleteAddress.conflicts.Add(new Conflicts { Problems = false, Description = "Area Eliminado Correctamente" });
            //return NoContent();
            return Ok(respDeleteAddress);
        }
    }
}
