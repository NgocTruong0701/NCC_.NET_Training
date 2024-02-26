using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QLSoThu
{
    public class Bird : Animal, IFlyable
    {
        private string color;
        private string sound;
        private string flyable;
        public string Color { get { return color; } set { color = value; } }
        public string Sound { get { return sound; } set { sound = value; } }
        public string FlyAble { get { return flyable; } set { flyable = value; } }
        public Bird()
        {
            Name = Color = Habitat = Species = Sound = FlyAble =  "";
        }

        public Bird(string name, string color, string habitat, string sound, string flyable)
        {
            Name = name;
            Color = color;
            Habitat = habitat;
            Species = "Bird";
            Sound = sound;
            FlyAble = flyable;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Species: {Species}");
            Console.WriteLine($"Skin Color: {Color}");
            Console.WriteLine($"Habitat: {Habitat}");
            Fly();
            MakeSound();
        }

        public void Fly()
        {
            Console.WriteLine("Flyable: " + FlyAble);
        }

        public override void MakeSound()
        {
            Console.WriteLine("Sound: " + Sound);
        }
    }
}
