using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP
{
    public class Student : Person
    {
        public double Math { get; set; }
        public double English { get; set; }

        public Student() : base()
        {
            Math = English = 0;
        }

        public Student(string firstName, string lastName, int age, double math, double english) : base(firstName, lastName, age)
        {
            Math = math;
            English = english;
        }

        public Student(string firstName, string lastName, int age) : base(firstName, lastName, age)
        {
            Math = English = 0;
        }

        internal double Total()
        {
            return Math + English;
        }
        public override string GetInfor()
        {
            return base.GetInfor() + " " + Total().ToString();
        }

        public string GetInfor(double math, double english)
        {
            return base.FirstName + " " + base.LastName + " " + math + " " + english;
        }

        public string GetInfor(double score)
        {
            return base.FirstName + " " + base.LastName + " " + score;
        }
    }
}
