namespace Interface
{
    public class Program
    {
        static void Main(string[] args)
        {
            Person person =new Person();
            Employee employee = new Employee();
            DogBreeder dogBreeder = new DogBreeder();
            EmployeeDogBreeder employeeDogBreeder = new EmployeeDogBreeder();

            string FName = "Le";
            string LName = "Ngoc Truong";
            int Age = 21;
            double Salary = 12.12;
            string Pos = "DEV";
            string NameOfDog = "Husky";

            // Person
            person.FirstName = FName;
            person.LastName = LName;
            person.Age = Age;
            Console.WriteLine(person.GetInfor());

            // Employee
            employee.FirstName = FName;
            employee.LastName = LName;
            employee.Salary = Salary;
            employee.Position = Pos;
            Console.WriteLine(employee.GetInfor());

            // DogBreeder
            dogBreeder.FirstName = FName;
            dogBreeder.LastName = LName;
            dogBreeder.Age = Age;
            dogBreeder.NameOfDog = NameOfDog;
            Console.WriteLine(dogBreeder.GetInfor());

            // EmployeeDogBreeder
            employeeDogBreeder.FirstName = FName;
            employeeDogBreeder.LastName = LName;
            employeeDogBreeder.Age = Age;
            employeeDogBreeder.Position = Pos;
            employeeDogBreeder.Salary = Salary;
            employeeDogBreeder.NameOfDog = NameOfDog;
            Console.WriteLine(employeeDogBreeder.GetInfor());

            Console.ReadKey();

        }
    }
}