using backendV03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using backendV03.Database;
using BCrypt.Net;

namespace backendV03.Controllers
{
    [Route("api/cliente")]
    [ApiController]
    public class ClienteController: ControllerBase
    {
        private readonly Conexion _context;

        public ClienteController(Conexion context)
        {
            _context = context;
        }

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

    }
}
