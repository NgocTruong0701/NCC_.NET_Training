namespace Project_OOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int choice;
            MathClass MathClass = new MathClass();
            EnglishClass EnglishClass = new EnglishClass();
            do
            {
                Console.WriteLine("========= Menu lua chon ============");
                Console.WriteLine("1. Them mot hoc sinh vao lop toan.");
                Console.WriteLine("2. Them mot hoc sinh vao lop tieng anh.");
                Console.WriteLine("3. Them mot ca si hoc sinh vao lop toan.");
                Console.WriteLine("4. Them mot ca si hoc sinh vao lop tieng anh.");
                Console.WriteLine("5. Xem thong tin lop hoc toan.");
                Console.WriteLine("6. Xem thong tin lop hoc tieng anh.");
                Console.WriteLine("7. Thoat.");
                Console.WriteLine("========= Nhap su lua chon ==========");
                choice = int.Parse(Console.ReadLine());

                if (choice < 1 || choice > 7)
                    Console.WriteLine("Lua chon khong hop le, moi nhap lai.");
                switch(choice)
                {
                    case 1: 

                }
            }
            while (choice == 7);

            Console.ReadKey();
        }

        public Student CreateStudent()
        {
            int choice;
            string firstName;
            string lastName;
            int age;
            double math = 0;
            double english = 0;

            Console.WriteLine("Hoc sinh ay la hoc sinh moi hay cu (0-1)");
            choice = int.Parse(Console.ReadLine());
            if (choice < 0 || choice > 1)
                Console.WriteLine("Nhap khong hop le");
            if (choice == 0)
            {
                Console.WriteLine("Nhap ho hoc sinh: ");
                firstName = Console.ReadLine();

                Console.WriteLine("Nhap ten hoc sinh: ");
                lastName = Console.ReadLine();

                Console.WriteLine("Nhap tuoi hoc sinh: ");
                age = int.Parse(Console.ReadLine());

                Student st = new Student(firstName, lastName, age, math, english);
                return st;
            }
            else
            {
                Console.WriteLine("Nhap ho hoc sinh: ");
                firstName = Console.ReadLine();

                Console.WriteLine("Nhap ten hoc sinh: ");
                lastName = Console.ReadLine();

                Console.WriteLine("Nhap tuoi hoc sinh: ");
                age = int.Parse(Console.ReadLine());

                Console.WriteLine("Nhap diem toan: ");
                math = double.Parse(Console.ReadLine());

                Console.WriteLine("Nhap diem anh: ");
                english = double.Parse(Console.ReadLine());

                Student st = new Student(firstName, lastName, age, math, english);
                return st;
            }
        }

        public void AddStudentIntoMathClass()
        {
            
        }
    }
}