using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP
{
    public class MathClass : Class
    {
        public List<Student> students = new List<Student>();
        public Math MathSubject;

        public MathClass()
        {
            NameClass = DescriptionClass = String.Empty;
            TotalStudent = 0;
            MathSubject = new Math();
        }

        public MathClass(string nameClass, string descriptionClass)
        {
            NameClass = nameClass;
            DescriptionClass = descriptionClass;
            TotalStudent = students.Count();
            MathSubject = students.Count();
        }
        public override void GetInforClass()
        {
            foreach(Student student in students)
            {
                string XepHang;
                if (student.Math < 3) XepHang = "Kem";
                else if (student.Math >= 3 && student.Math < 5) XepHang = "Trung binh";
                else if (student.Math >= 5 && student.Math < 7.5) XepHang = "Kha";
                else if (student.Math >= 7.5 && student.Math < 8.5) XepHang = "Tot";
                else  XepHang = "Xuat sac";

                Console.WriteLine(student.GetInfor() + " Xep hang: " + XepHang);
            }
        }
    }
}
