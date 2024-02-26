namespace Polyphism
{
    class Program
    {
        static void Main(string[] args)
        {

            Person p = new Employee();
            p.FirstName = "Sander";
            p.LastName = "Rossel";
            PrintFullName(p);
            // Press any key to quit.
            Console.ReadKey();
        }

        public static void PrintFullName(Person p)

        {
            Console.WriteLine(p.GetFullName());
        }
    }

    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual string GetFullName()

        {
            return FirstName + " " + LastName;
        }

        public string GetInfor(string firstName, string lastName)
        {
            return FirstName + " " + LastName;
        }

        public string GetInfor(string name)
        {
            return name;
        }
    }

    public class Employee : Person
    {
        public decimal Salary { get; set; }

        public sealed override string GetFullName()
        {
            return LastName + ", " + FirstName;
        }

        public long Long(string name)
        {
            return 1;
        }

        public string Long(string name, string name2)
        {
            return name + name2;
        }
    }   

}