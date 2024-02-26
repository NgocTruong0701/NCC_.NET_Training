using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP
{
    public abstract class Class
    {
        protected string NameClass { get; set; }
        protected string DescriptionClass { get; set; }
        protected int TotalStudent { get; set; }
        public abstract void GetInforClass();
    }
}
