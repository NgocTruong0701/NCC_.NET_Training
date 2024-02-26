using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSoThu
{
    public class Zoo
    {
        private List<Animal> animals = new List<Animal>();
        public void AddAnimal(Animal animal)
        {
            animals.Add(animal);
        }

        public void DisplayAllAnimals()
        {
            Console.WriteLine("Zoo Residents:");
            Console.WriteLine("===============================");
            Console.WriteLine("------------------------");
            Console.WriteLine("List Fish:");
            Console.WriteLine("------------------------");
            foreach (var animal in animals)
            {
                if (animal is Fish)
                {
                    animal.DisplayInfo();
                    Console.WriteLine("");
                }
            }
            Console.WriteLine("------------------------");
            Console.WriteLine("List Bird:");
            Console.WriteLine("------------------------");
            foreach (var animal in animals)
            {
                if (animal is Bird)
                {
                    animal.DisplayInfo();
                    Console.WriteLine("");
                }
            }
        }
    }
}
