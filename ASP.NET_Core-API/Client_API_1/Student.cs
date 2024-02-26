namespace API_1
{
    public class Student
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public double MathScores { get; set; }
        public double EnglishScores { get; set; }
        public double ChemistryScores { get; set; }

        public Student()
        {
            Id = FirstName = LastName = string.Empty;
            Age = 0;
            MathScores = EnglishScores = ChemistryScores = 0;
        }

        public Student(string id, string firstName, string lastName, int age, double mathScores, double englishScores, double chemistryScores)
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
