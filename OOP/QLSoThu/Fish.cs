using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QLSoThu
{
    public class Fish : Animal, ISwimmable
    {
        public string TypeFish { get; set; }
        public string Sound { get; set; }
        public string SwimmAble { get; set; }
        public Fish()
        {
            Name = TypeFish = Habitat = Species = Sound = SwimmAble = "";
        }

        public Fish(string name, string typePish, string habitat, string sound, string swimmable)
        {
            Name = name;
            TypeFish = typePish;
            Habitat = habitat;
            Species = "Fish";
            Sound = sound;
            SwimmAble = swimmable;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Species: {Species}");
            Console.WriteLine($"Skin Color: {TypeFish}");
            Console.WriteLine($"Habitat: {Habitat}");
            MakeSound();
            Swim();
        }

        public override void MakeSound()
        {
            Console.WriteLine("Sound: " + Sound);
        }

        public void Swim()
        {
            Console.WriteLine("Swimmable: " + SwimmAble);
        }
    }
}
