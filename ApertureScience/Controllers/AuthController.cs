using ApertureScience.Data;
using ApertureScience.Data.Repositories;
using ApertureScience.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApertureScience.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginRequestRepository _loginRequestRepository;
        private readonly IUserRepository _userRepository;

        public AuthController(ILoginRequestRepository loginRequestRepository, IUserRepository userRepository)
        {
            _loginRequestRepository = loginRequestRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequest login)
        {
            //Response respc = new Response();
            Response respLogin = new Response();
            respLogin.responseLogin = new List<ResponseLogin>();
            respLogin.conflicts = new List<Conflicts>();
            //respLogin.responseLogin = new List<ResponseLogin>();

            //Dictionary<ResponseLogin, Conflicts> responseLogin = new Dictionary<ResponseLogin, Conflicts>();

            if (login == null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            {
                //var newResponseLogin = new ResponseLogin { Name = "", LastNameP = "", LastNameM = ""};
                //var newConflicts = new Conflicts { Problems = true, Description = "Email y contraseña son requeridos" };

                //responseLogin.Add(newResponseLogin, newConflicts);
                respLogin.responseLogin.Add(new ResponseLogin() {  });
                respLogin.conflicts.Add(new Conflicts { Problems=true, Description = "Email y contraseña son requeridos" });
                return BadRequest(respLogin);
            }

            // Buscar el usuario por correo electrónico
            var user = await _loginRequestRepository.GetByEmail(login);

            if (user == null)
            {
                respLogin.responseLogin.Add(new ResponseLogin() {  });
                respLogin.conflicts.Add(new Conflicts { Problems = true, Description = "Usuario no encontrado" });
                return NotFound(respLogin);
            }
            
            var perro = await _userRepository.GetByEmail(login.Email);
            //Console.WriteLine(perro);
            //Verificar la contraseña

            //var inputPassword = PasswordHelper.HashPassword(login.Password);

            //Console.WriteLine(user);
            bool isPasswordValid = VerifyPassword(login.Password, user.Password);

            if (!isPasswordValid)
            {
                respLogin.responseLogin.Add(new ResponseLogin() { });
                respLogin.conflicts.Add(new Conflicts { Problems = true, Description = "Credenciales inválidas" });
                return Unauthorized(respLogin);
            }

            //Aquí puedes generar un token de autenticación si las credenciales son válidas
            //Puedes usar bibliotecas como JWT para generar tokens JWT
            respLogin.responseLogin.Add(new ResponseLogin { 
                Id = perro.Id, 
                Name = perro.Name, 
                LastNameP = perro.LastNameP, 
                LastNameM = perro.LastNameM, 
                IdRole = perro.IdRole, 
                IdDepartment = perro.IdDepartment });
            respLogin.conflicts.Add(new Conflicts { Problems = false, Description = "Login Correctamente" });
            //respLogin.responseLogin[perro.Id] = new Conflicts { Problems = false };
            //respLogin.responseLogin[new ResponseLogin { Name = perro.Name, LastNameP = perro.LastNameP, LastNameM = perro.LastNameM, IdRole = perro.IdRole, IdDepartment = perro.IdDepartment }] = new Conflicts { Problems = false };
            //respLogin.conflicts.Add(new Conflicts { Problems = false });
            //respc.conflicts.Add(new Conflicts { Problems = false });
            return Ok(respLogin); // Puedes devolver un token JWT u otra información aquí

        }
        private static bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            var EncryptPassword = PasswordHelper.HashPassword(inputPassword);



            return EncryptPassword == hashedPassword;
        }
    }
}
