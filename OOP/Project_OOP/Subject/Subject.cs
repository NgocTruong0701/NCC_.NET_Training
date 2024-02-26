using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP
{
    public abstract class Subject
    {
        protected string SubjectName { get; set; }
        protected string SubjectDescription { get; set; }
        protected int TotalStudent { get; set; }
    }
}
