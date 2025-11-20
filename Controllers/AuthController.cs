using Lab5_ChristianMamani.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab5_ChristianMamani.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Lab5DbContext _context;
        private readonly IConfiguration _config;

        public AuthController(Lab5DbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado correctamente" });
        }

        // ✅ Login que genera JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
            if (user == null)
                return Unauthorized("Usuario no encontrado");

            // Validar contraseña
            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                return Unauthorized("Contraseña incorrecta");

            // Generar token
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        // 🔑 Método privado para generar JWT
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ✅ Endpoint accesible solo para Admins
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult GetAdminData()
        {
            return Ok("Datos sensibles solo para administradores");
        }

        // ✅ Endpoint accesible solo para Users
        [Authorize(Roles = "User")]
        [HttpGet("user")]
        public IActionResult GetUserData()
        {
            return Ok("Datos visibles solo para usuarios");
        }

        // ✅ Endpoint accesible para cualquier usuario autenticado
        [Authorize]
        [HttpGet("general")]
        public IActionResult GetGeneralData()
        {
            return Ok("Datos accesibles para cualquier usuario autenticado");
        }
    }

    // DTO para login
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
