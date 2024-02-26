namespace Interface
{
    public class Employee : 
    {
        public string Position { get; set; }
        public double Salary { get; set ; }

        public string GetSalaryAndPos()
        {
            return "Salary: " + Salary.ToString() + " Position: " + Position;
        }

        public string GetPhone()
        {
            return this.Phone;
        }
    }
}
