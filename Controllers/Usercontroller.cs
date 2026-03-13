using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<UsersController> _logger;

        public UsersController(AppDbContext db, ILogger<UsersController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: api/users?q=texto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] string? q = null)
        {
            IQueryable<User> query = _db.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.ToLower();
                query = query.Where(u =>
                    u.FirstName.ToLower().Contains(term) ||
                    u.LastName.ToLower().Contains(term) ||
                    u.Email.ToLower().Contains(term) ||
                    (u.Department != null && u.Department.ToLower().Contains(term))
                );
            }

            var users = await query
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) return NotFound();
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            // [ApiController] valida ModelState automáticamente
            // Campos controlados por el servidor
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTimeOffset.UtcNow;
            user.UpdatedAt = null;

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User update)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) return NotFound();

            user.FirstName = update.FirstName;
            user.LastName = update.LastName;
            user.Email = update.Email;
            user.Department = update.Department;
            user.IsActive = update.IsActive;
            user.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}