using System.Security.Cryptography.X509Certificates;

namespace QLSoThu
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Zoo zoo = new Zoo();
            Fish goldfish = new Fish("Goldfish", "Freshwater", "Aquarium", "Bloop", "Slow");
            Fish shark = new Fish("Shark", "Saltwater", "Ocean", "Chomp", "Fast");
            Fish clownfish = new Fish("Clownfish", "Saltwater", "Reef", "Squeak", "Fast");
            Fish bettaFish = new Fish("Betta Fish", "Freshwater", "Bowl", "Bubble", "Slow");

            Bird sparrow = new Bird("Sparrow", "Brown", "Forest", "Chirp", "Yes");
            Bird eagle = new Bird("Eagle", "Brown", "Mountain", "Screech", "Yes");
            Bird penguin = new Bird("Penguin", "Black and White", "Antarctica", "Honk", "No");

            zoo.AddAnimal(goldfish);
            zoo.AddAnimal(shark);
            zoo.AddAnimal(clownfish);
            zoo.AddAnimal(bettaFish);
            zoo.AddAnimal(sparrow);
            zoo.AddAnimal(eagle);
            zoo.AddAnimal(penguin);

            zoo.DisplayAllAnimals();

            Console.ReadKey();
        }
    }
}