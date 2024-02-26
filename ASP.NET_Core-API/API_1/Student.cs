using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_1
{
    public class Student
    {
        [Key]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public double MathScores { get; set; }
        public double EnglishScores { get; set; }
        public double ChemistryScores { get; set; }

        public static int _nextStudentID = 3;

        public Student()
        {
            Id = 0;
            FirstName = LastName = string.Empty;
            Age = 0;
            MathScores = EnglishScores = ChemistryScores = 0;
        }

        public Student(long id)
        {
            Id = id;
            FirstName = LastName = string.Empty;
            Age = 0;
            MathScores = EnglishScores = ChemistryScores = 0;
        }

        public Student(long id, string firstName, string lastName, int age, double mathScores, double englishScores, double chemistryScores)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            MathScores = mathScores;
            EnglishScores = englishScores;
            ChemistryScores = chemistryScores;
        }


    }
}
