using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    internal class DogBreeder : Person, IDogBreeders
    {
        public string NameOfDog { get ; set ; }

        public override string GetInfor()
        {

            return base.GetInfor() + " Name of Dog: " + NameOfDog;
        }


    }
}
