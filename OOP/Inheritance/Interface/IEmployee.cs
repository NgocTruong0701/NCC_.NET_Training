using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    internal interface IEmployee
    {
        string Position { get; set; }
        double Salary { get; set; }

        string GetSalaryAndPos();
    }
}
