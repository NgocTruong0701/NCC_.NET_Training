using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP
{
    public class Math : Subject
    {
        public Math()
        {
            SubjectName = SubjectDescription = "";
            TotalStudent = 0;
        }
        public Math(string name, string description, int totalStudents)
        {
            SubjectName = name;
            SubjectDescription = description;
            TotalStudent = totalStudents;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Subject Name: {SubjectName}");
            Console.WriteLine($"Subject Description: {SubjectDescription}");
            Console.WriteLine($"Total Students: {TotalStudent}");
        }
    }
}
