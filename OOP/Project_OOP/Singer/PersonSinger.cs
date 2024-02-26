using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP
{
    public class PersonSinger : Person, ISinger
    {
        public string TypeSing {get; set;}
        public string TypeDance { get; set;}
        public PersonSinger() : base() { }
        public string Dance()
        {
            return TypeDance;
        }

        public string Sing()
        {
            return TypeSing;
        }
    }
}
