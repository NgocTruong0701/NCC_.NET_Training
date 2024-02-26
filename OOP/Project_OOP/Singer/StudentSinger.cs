using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP
{
    public class StudentSinger : Student, ISinger
    {
        public string TypeSing { get; set; }
        public string TypeDance { get; set; }
        public string NameTeacher { get; set; }
        public string Dance()
        {
            return TypeDance + " Teacher: " + NameTeacher;
        }

        public string Sing()
        {
            return TypeSing + " Teacher: " + NameTeacher;
        }
    }
}
