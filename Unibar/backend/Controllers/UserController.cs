using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == "user_type")?.Value;
            return int.TryParse(roleClaim, out int type) && type == 1;
        }

        private void DeletePhysicalFile(string fileName)
{
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        var filePath = Path.Combine(uploadsFolder, fileName);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
}

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new UserResponse {
                    id = u.id,
                    username = u.username,
                    email = u.email,
                    user_type = u.user_type
                })
                .ToListAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id) {
            var user = await _context.Users.FindAsync(id);
            if(user == null) {
                return NotFound("User not found");
            }

            return Ok(new { user.id, user.username});
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user) {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Password hashing
            user.password_hash = BCrypt.Net.BCrypt.HashPassword(user.password_hash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllUsers), new {id = user.id}, new UserResponse {
                id = user.id,
                username = user.username,
                email = user.email,
                user_type = user.user_type
            });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {

            if (!IsAdmin())
            {
                return Forbid();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found."});
            }

           // Slet maskiner
            var machines = _context.Machines.Where(m => m.made_by == id);
            _context.Machines.RemoveRange(machines);

            // Slet materialer
            var materials = _context.Materials.Where(m => m.made_by == id);
            _context.Materials.RemoveRange(materials);

            // Slet processer
            var processes = _context.Processes.Where(p => p.made_by == id);
            _context.Processes.RemoveRange(processes);

            // Slet calculations
            var calculations = _context.Calculations.Where(c => c.made_by == id);
            _context.Calculations.RemoveRange(calculations);

            // Slet reference prints
            var references = _context.ReferencePrints.Where(r => r.suggested_by == id);
            _context.ReferencePrints.RemoveRange(references);

            //Slet billeder
            var files = _context.UploadedFiles.Where(f => f.uploaded_by == id).ToList();

            foreach (var file in files)
            {
                DeletePhysicalFile(file.file_name);
            }

            _context.UploadedFiles.RemoveRange(files);


            // Til sidst: slet selve brugeren
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User and their content deleted successfully." });
        }

        [Authorize]
        [HttpPut("toggle-admin/{id}")]
        public async Task<IActionResult> ToggleAdminRole(int id) {
            if (!IsAdmin()) {
                return Forbid();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }

            if (user.user_type == 1)
            {
                user.user_type = 0; // fjern admin-rolle
            }
            else
            {
                user.user_type = 1; // gør til admin
            }
            await _context.SaveChangesAsync();

            return Ok(new { user.id, user.username, user.user_type });
        }
    }
}