using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/<Users>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userRepository.GetAllUsers()); ;
        }

        // GET api/<Users>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(await _userRepository.GetById(id));
        }

        // GET api/<Users>/algo@algo.com.mx
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetEmail(string email)
        {
            return Ok(await _userRepository.GetByEmail(email));
        }

        // POST api/<Users>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            Response respCreateUser = new Response();
            respCreateUser.users = new List<User>();
            respCreateUser.conflicts = new List<Conflicts>();


            if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.LastNameP) || string.IsNullOrWhiteSpace(user.LastNameM) || string.IsNullOrWhiteSpace(user.Email) || user.IdRole == null || user.IdDepartment == null)
            {
                respCreateUser.users.Add(new User() { });
                respCreateUser.conflicts.Add(new Conflicts { Problems = true, Description = "Todos los campos son obligatorios" });
                return BadRequest(respCreateUser);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailAuth = await _userRepository.GetByEmail(user.Email);

            if (emailAuth != null)
            {
                respCreateUser.conflicts.Add(new Conflicts { Problems = true, Description = "El correo ya existe" });

                return BadRequest(respCreateUser);
            }

            var created = await _userRepository.InsertUser(user);

            if (created == null)
            {
                respCreateUser.conflicts.Add(new Conflicts { Problems = true, Description = "Consulta nula" });

                return BadRequest(respCreateUser);
            }

            respCreateUser.users.Add(new User { Name = created.Name, LastNameP = created.LastNameP, LastNameM = created.LastNameM, Email = created.Email, Username = created.Username, Password = created.Password, IdRole = user.IdRole, IdDepartment = user.IdDepartment });
            respCreateUser.conflicts.Add(new Conflicts { Problems = false, Description = "Usuario Creado Correctamente" });


            return Created("created", respCreateUser);
        }

        // PUT api/<Users>/5
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            Response respUpdateUser = new Response();
            respUpdateUser.conflicts = new List<Conflicts>();

            if (user == null)
            {
                respUpdateUser.conflicts.Add(new Conflicts { Problems = true, Description = "Se necesita mínimo un campo para Actualizar" });
                return BadRequest(respUpdateUser);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailAuth = await _userRepository.GetByEmail(user.Email);

            if (emailAuth != null)
            {
                respUpdateUser.conflicts.Add(new Conflicts { Problems = true, Description = "El correo ya existe" });

                return BadRequest(respUpdateUser);
            }

            var update = await _userRepository.UpdateUser(user);

            if (update == false)
            {
                respUpdateUser.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Actualizado" });
                return BadRequest(respUpdateUser);
            }

            respUpdateUser.conflicts.Add(new Conflicts { Problems = false, Description = "Usuario Actualizado Correctamente" });
            //return NoContent();
            return Ok(respUpdateUser);
        }

        // DELETE api/<Users>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            Response respDeleteUser = new Response();
            //respCreateUser.users = new List<User>();
            respDeleteUser.conflicts = new List<Conflicts>();

            var delete = await _userRepository.DeleteUser(new User { Id = id });

            if(delete == false)
            {
                respDeleteUser.conflicts.Add(new Conflicts { Problems = true, Description = "No fue Eliminado" });
                return BadRequest(respDeleteUser);
            }
            respDeleteUser.conflicts.Add(new Conflicts { Problems = false, Description = "Usuario Eliminado Correctamente" });
            //return NoContent();
            return Ok(respDeleteUser);
        }
    }
}
