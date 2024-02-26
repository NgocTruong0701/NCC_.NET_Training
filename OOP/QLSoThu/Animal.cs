using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSoThu
{
    public abstract class Animal
    {
        public string Name { get; set; }
        public string Species { get; set; }
        protected string Habitat { get; set; }
        public abstract void DisplayInfo();
        public abstract void MakeSound();
    }
}
