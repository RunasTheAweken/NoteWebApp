namespace Controllers
{
    using Context;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.ComponentModel.DataAnnotations;

    [ApiController]
    [Route("notes")]
    public class NotesController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILogger<NotesController> _logger;

        public NotesController(MyDbContext context, ILogger<NotesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateNote(int userId, [FromBody] NoteDTO note, CancellationToken cancellationToken = default)
        {

            if (note == null)
            {
                _logger.LogWarning("NoteController [CreateNote] : Empty DTO received for UserId {UserId}", userId);
                return BadRequest("Request body is empty");
            }


            var user = await _context.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("NoteController [CreateNote] : User with ID {UserId} not found", userId);
                return NotFound("User with this ID doesn't exist");
            }


            if (string.IsNullOrEmpty(note.Title))
            {
                _logger.LogWarning("NoteController [CreateNote] : Title is null, replacing with template for UserId {UserId}", userId);
                note.Title = "Empty Title";
            }
            if (string.IsNullOrEmpty(note.Content))
            {
                _logger.LogWarning("NoteController [CreateNote] : Content is null, replacing with template for UserId {UserId}", userId);
                note.Content = "Empty Content";
            }


            var createdNote = new Note
            {
                Title = note.Title,
                Content = note.Content,
                UserId = userId
            };

            await _context.Notes.AddAsync(createdNote, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("NoteController [CreateNote] : Note '{Title}' created for UserId {UserId}", note.Title, userId);
            return CreatedAtAction(nameof(GetAllUsersNotes), new { userId }, new { createdNote.Id, createdNote.Title, createdNote.Content });
        }

        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateNote(int noteId, [FromBody] NoteDTO note, CancellationToken cancellationToken = default)
        {

            if (note == null)
            {
                _logger.LogWarning("NoteController [UpdateNote] : Empty DTO received for NoteId {NoteId}", noteId);
                return BadRequest("Request body is empty");
            }

 
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("NoteController [UpdateNote] : Invalid model state for NoteId {NoteId}", noteId);
                return BadRequest(ModelState);
            }


            var wantedNote = await _context.Notes.FindAsync(new object[] { noteId }, cancellationToken);
            if (wantedNote == null)
            {
                _logger.LogWarning("NoteController [UpdateNote] : No note with ID {NoteId}", noteId);
                return NotFound("Note with this ID doesn't exist");
            }


            wantedNote.Title = note.Title;
            wantedNote.Content = note.Content;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("NoteController [UpdateNote] : Note '{Title}' updated for NoteId {NoteId}", wantedNote.Title, noteId);
            return Ok(new { wantedNote.Id, wantedNote.Title, wantedNote.Content });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllUsersNotes(int userId, CancellationToken cancellationToken = default)
        {
            var wantedUser = await _context.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (wantedUser == null)
            {
                _logger.LogWarning("NoteController [GetAllUsersNotes] : No user with ID {UserId}", userId);
                return NotFound("User with this ID doesn't exist");
            }


            var result = await _context.Notes
                .Where(note => note.UserId == userId)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("NoteController [GetAllUsersNotes] : Retrieved {Count} notes for UserId {UserId}", result.Count, userId);
            return Ok(new
            {
                Username = wantedUser.Nickname,
                Notes = result
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes(CancellationToken cancellationToken = default)
        {
            var notes = await _context.Notes.ToListAsync(cancellationToken);
            _logger.LogInformation("NoteController [GetAllNotes] : Retrieved {Count} notes", notes.Count);
            return Ok(notes);
        }

        [HttpDelete("{noteId}")]
        public async Task<IActionResult> DeleteNote(int noteId, CancellationToken cancellationToken = default)
        {
            var note = await _context.Notes.FindAsync(new object[] { noteId }, cancellationToken);
            if (note == null)
            {
                _logger.LogWarning("NoteController [DeleteNote] : No note with ID {NoteId}", noteId);
                return NotFound("Note with this ID doesn't exist");
            }


            _context.Notes.Remove(note);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("NoteController [DeleteNote] : Note with ID {NoteId} deleted", noteId);
            return NoContent();
        }
    }

    public class NoteDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
    }
}