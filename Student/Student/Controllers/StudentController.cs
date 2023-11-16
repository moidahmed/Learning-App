using Microsoft.AspNetCore.Mvc; 
using Student.Data; 

namespace Student.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        List<Student> students = new List<Student>
        {
            new Student{ StudentId = 1, FirstName = "Cristiano", LastName = "Ronaldo", Class = 7},
            new Student{ StudentId = 2, FirstName = "Lionel", LastName = "Messi", Class = 10}
        };
        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }
        /*provide Key : Authorization , Value : Basic dXNlcjpwYXNz, Add to : Header on the Authorization in the postman also check Type to API KEy */
        [HttpGet("GetHardCotedStudents")]
        [ServiceFilter(typeof(BasicAuthenticationFilter))]
        public IEnumerable<Student> GetHardCotedStudents()
        {
            return students;
        }

        [HttpGet("GetAllStudents")]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            try
            {
                var students = _context.Students.ToList();

                if (students == null || students.Count == 0)
                {
                    return NotFound("No students found.");
                }

                return Ok(students);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllStudents: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
        [HttpPost("Create")]
        public ActionResult<Student> Create(Student student)
        {
            try
            {
                _context.Add(student);
                _context.SaveChanges();

                return Ok(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update(Student student)
        {
            try
            {
                if (student.StudentId == 0)
                {
                    return BadRequest();
                }

                var existingStudent = await _context.Students.FindAsync(student.StudentId);

                if (existingStudent == null)
                {
                    return NotFound();
                }
                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
                existingStudent.Class = student.Class;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

    }
}