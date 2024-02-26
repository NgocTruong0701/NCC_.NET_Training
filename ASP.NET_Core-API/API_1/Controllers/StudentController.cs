using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace API_1.Controllers
{
    /*[Authorize] */// xác thực và ủy quyền 
    [Route("student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private static List<Student> students = new List<Student>{
            new Student
            {
                Id = 0,
                FirstName = "John",
                LastName = "Doe",
                Age = 20,
                MathScores = 85.5,
                EnglishScores = 78.0,
                ChemistryScores = 90.5
            },
            new Student
            {
                Id = 1,
                FirstName = "Alice",
                LastName = "Smith",
                Age = 22,
                MathScores = 90.0,
                EnglishScores = 85.0,
                ChemistryScores = 88.5
            },
            new Student
            {
                Id = 2,
                FirstName = "Bob",
                LastName = "Johnson",
                Age = 21,
                MathScores = 78.0,
                EnglishScores = 82.0,
                ChemistryScores = 76.5
            }
        };

        // GET: student/all
        [HttpGet("all")]
        public IActionResult GetStudents()
        {
            return Ok(students);
        }

        // GET: student/001
        [HttpGet("{id}")]
        public IActionResult GetStudent(long id)
        {
            foreach (Student st in students)
            {
                if (st.Id == id)
                    return Ok(st);
            }
            return NotFound("Không tìm thấy student có id: " + id);
        }

        // GET: student/scores?totalscores=250
        [HttpGet("scores")]
        public IActionResult GetStudentWithScores(double totalscore)
        {
            if (totalscore <= 0)
            {
                return BadRequest("Điểm không hợp lệ.");
            }
            else
            {
                Student student = students.Select(x => x).Where(x => (x.MathScores + x.EnglishScores + x.ChemistryScores) == totalscore).FirstOrDefault();
                
                if (student != null)
                {
                    return Ok(student);
                }
                else
                {
                    return NotFound("Không tìm thấy student có tổng điểm: " + totalscore);
                }
            }
        }

        // GET: student?pagenumber=1&pagesize=2
        [HttpGet]
        public IActionResult GetStudentWithPagination(int page, int pageSize)
        {
            int totalCount = students.Count();
            int totalPage = (int)Math.Ceiling((double)totalCount / pageSize);

            // Kiểm tra giá trị pagenumber để tránh truy vấn ngoài phạm vi
            if (page < 1)
            {
                page = 1;
            }
            if (page > totalPage)
            {
                page = totalPage;
            }

            List<Student> studentOnPage = students.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(studentOnPage);
        }

        // POST: student/add
        [HttpPost("add")]
        public IActionResult Add([FromBody] StudentDTO studentDTO)
        {
            int count = Student._nextStudentID++;

            Student st = new Student(count, studentDTO.FirstName, studentDTO.LastName, studentDTO.Age, studentDTO.MathScores, studentDTO.EnglishScores, studentDTO.ChemistryScores);
            Student s = students.Where(x => x.Id == count).FirstOrDefault();
            if (students.Contains(s))
            {
                return BadRequest("Student already exists.");
            }

            students.Add(st);
            return Ok(st);
        }

        // PUT: student/update
        [HttpPut("update/{id}")]
        public IActionResult Update(long id, [FromBody] StudentDTO studentDTO)
        {
            int index = students.FindIndex(st => st.Id == id);
            if (index != -1)
            {
                Student newStudent = new Student(id, studentDTO.FirstName, studentDTO.LastName, studentDTO.Age, studentDTO.MathScores, studentDTO.EnglishScores, studentDTO.ChemistryScores); 
                students[index] = newStudent;
                return Ok(newStudent);
            }

            return NotFound("Không tìm thấy student với id: " + id);
        }

        // DELETE: student/delete/001
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(long id)
        {
            var st = students.Where(s => s.Id == id).FirstOrDefault();
            if(st != null)
            {
                students.Remove(st);
                return Ok(st);
            }
            return NotFound("Không tìm thấy student có id: " + id);
        }

    }
}
