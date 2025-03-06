using backendV03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using backendV03.Database;
using BCrypt.Net;
using backendV03.Tokens;
using System.Threading.Tasks;


namespace backendV03.Controllers
{
    [Route("api/cliente")]
    [ApiController]
    public class ClienteController: ControllerBase
    {
        private readonly Conexion _context;
        private readonly Jwt _jwt;

        public ClienteController(Conexion context, Jwt jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        //registro

        [HttpPost("registro")]
        public async Task<IActionResult> RegistroCliente([FromBody] Cliente data)
        {
            try
            {
                if (data == null)
                {
                    return BadRequest(new { msg = "Datos inválidos", data = (object?)null });
                }

                // Verificar si el correo ya existe
                var clienteExistente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == data.Email);
                if (clienteExistente != null)
                {
                    return Conflict(new { msg = "El correo ya existe en la base de datos", data = (object?)null });
                }

                if (string.IsNullOrEmpty(data.Password))
                {
                    return BadRequest(new { msg = "No hay una contraseña", data = (object?)null });
                }

                // Hashear la contraseña
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(data.Password);

                // Crear el nuevo cliente
                Cliente nuevoCliente = new Cliente
                {
                    Nombres = data.Nombres,
                    Apellidos = data.Apellidos,
                    Pais = data.Pais,
                    Email = data.Email,
                    Password = hashedPassword,
                    Perfil = data.Perfil,
                    rol = data.rol,
                    Telefono = data.Telefono,
                    Genero = data.Genero,
                    F_Nacimiento = data.F_Nacimiento,
                    Dni = data.Dni,
                    CreatedAt = DateTime.UtcNow
                };

                // Guardar en la BD
                _context.Clientes.Add(nuevoCliente);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { data = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Error en el servidor", error = ex.Message });
            }
        }

        //Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto data)
        {
            try
            {
                // Verificar si el usuario existe en la base de datos
                var user = await _context.Clientes.FirstOrDefaultAsync(u => u.Email == data.Email);
                if (user == null)
                {
                    return NotFound(new { msg = "El correo no está registrado", data = (object?)null });
                }

                // Verificar la contraseña
                if (!BCrypt.Net.BCrypt.Verify(data.Password, user.Password))
                {
                    return Unauthorized(new { msg = "Contraseña incorrecta", data = (object?)null });
                }

                // Generar el token
                var token = _jwt.CreateToken(user);

                // Enviar respuesta
                return Ok(new
                {
                    data = new
                    {
                        cliente = new
                        {
                            user.Nombres,
                            user.Apellidos
                        },
                        token
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Error en el servidor", error = ex.Message });
            }
        }
        // DTO para evitar exponer todo el modelo Cliente en la petición
        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
