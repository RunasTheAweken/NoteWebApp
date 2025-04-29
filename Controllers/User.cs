

using Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{

    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {

        private readonly IHasher _hasher;
        private readonly ILogger _logger;
        private readonly MyDbContext _context;
        public UserController(ILogger<UserController> logger, IHasher hasher, MyDbContext context)
        {
            _hasher = hasher;
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(dto);
            }
            if (dto == null)
            {
                _logger.LogWarning("UserController [Register] : Empty DTO received");
                return BadRequest("Request body is empty");
            }
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                _logger.LogError("UserController [Register] : Email already exists");
                return Conflict("Email already registered");
            }
            string hashedPassword;
            try
            {
                hashedPassword = _hasher.HashString(dto.Password);
            }
            catch (ArgumentNullException)
            {
                _logger.LogError("UserController [Register] : ERROR, data is null");
                return Conflict("Nickname or Email or Password should always be filled");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something unexpected happen: {ex.Message}");
                return StatusCode(500, "Shit happend");
            }

            User newUser = new User
            {
                Nickname = dto.Nickname,
                Email = dto.Email,
                PasswordHashed = hashedPassword
            };
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogInformation($"UserController [Register] : Success user {newUser.Nickname} has registered!");
            }

            _context.Add<User>(newUser);
            await _context.SaveChangesAsync();
            return Ok($"User with {newUser.Nickname} registered!");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            User? WantedUser = await _context.Users.FindAsync(id);
            if (WantedUser == null)
            {
                _logger.LogWarning("UserController [GET BY ID] : ERROR, no user with this id");
                return NotFound("User with this ID doesn't exist, yet");
            }
            var UsersNotes = await _context.Notes.Where(notes => notes.UserId == id).ToListAsync();
            return Ok(new { Username = WantedUser?.Nickname, Notes = UsersNotes });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, [FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(dto);

            if (dto == null)
            {
                _logger.LogWarning("UserController [DELETE] : Empty DTO received for ID {Id}", id);
                return BadRequest("Request body is empty");
            }

            var wantedUser = await _context.Users.FindAsync(id);

            if (wantedUser == null)
            {
                _logger.LogWarning("UserController [DELETE] : User tried to delete non existing account");
                return NotFound("User with this ID doesn't exist");
            }

            if (wantedUser.Email != dto.Email)
                return BadRequest("Email not matching");
            bool result;
            try
            {
                result = _hasher.IsValid(dto.Password, wantedUser.PasswordHashed);
            }
            catch (NullReferenceException)
            {
                return BadRequest("Input is missing");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserController [DELETE] : ERROR, {ex.Message}");
                return StatusCode(500, "shit happend");
            }
            if (!result)
                return Unauthorized("Password Incorrect");

            _context.Remove(wantedUser);
            var userNotes = await _context.Notes.Where(note => note.UserId == wantedUser.Id).ToListAsync();
            if (userNotes.Any())
            {
                _context.Notes.RemoveRange(userNotes);
                _logger.LogInformation("UserController [DELETE] : Deleted {Count} notes for user ID {Id}", userNotes.Count, id);
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserDTO dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(dto);

            if (dto == null)
            {
                _logger.LogWarning("UserController [UPDATE] : Empty DTO received for ID {Id}", id);
                return BadRequest("Request body is empty");
            }

            var wantedUser = await _context.Users.FindAsync(id);

            if (wantedUser == null)
            {
                _logger.LogWarning("UserController [UPDATE] : Invalid password for ID {Id}", id);
                return NotFound("User with this ID doesn't exist");
            }

            if (wantedUser.Email != dto.Email)
                return BadRequest("Email not matching");

            bool result;
            try
            {
                result = _hasher.IsValid(dto.Password, wantedUser.PasswordHashed);
            }
            catch (NullReferenceException)
            {
                return BadRequest("Input is missing");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserController [UPDATE] : Error validating password for ID {Id}", id);
                return StatusCode(500, "shit happend");
            }

            dto.Password = _hasher.HashString(dto.Password);

            User updatedUser = new User
            {
                Id = wantedUser.Id,
                Nickname = dto.Nickname,
                PasswordHashed = dto.Password
            };
            wantedUser.Nickname = dto.Nickname;
            wantedUser.PasswordHashed = _hasher.HashString(dto.Password);

            await _context.SaveChangesAsync();
            return Ok($"User {updatedUser.Nickname} is updated!");

        }

        [HttpGet("list")]
        public async Task<IActionResult> GetUserList()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Nickname,
                    u.Email
                })
                .ToListAsync();
            return Ok(users);
        }
    }

}