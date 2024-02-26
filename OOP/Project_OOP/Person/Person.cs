using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP
{
    public class Person
    {
        public Person()
        {
            FirstName = LastName = string.Empty;
            Age = 0;
        }

        public Person(string firstName,string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        private string firstName;
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }
        public string LastName { get; set; }
        public int Age { get; set; }
        public virtual string GetInfor()
        {
            return FirstName + " " + LastName + " " + Age;
        }
    }
}
