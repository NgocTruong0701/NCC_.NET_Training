using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OOP
{
    public class EnglishClass : Class
    {
        public List<Student> students = new List<Student>();

        public EnglishClass()
        {
            NameClass = DescriptionClass = String.Empty;
            TotalStudent = 0;
        }

        public EnglishClass(string nameClass, string descriptionClass)
        {
            NameClass = nameClass;
            DescriptionClass = descriptionClass;
            TotalStudent = students.Count();
        }
        public override void GetInforClass()
        {
            foreach(Student student in students)
            {
                string XepHang;
                if (student.English < 3) XepHang = "Kem";
                else if (student.English >= 3 && student.English < 5) XepHang = "Trung binh";
                else if (student.English >= 5 && student.English < 7.5) XepHang = "Kha";
                else if (student.English >= 7.5 && student.English < 8.5) XepHang = "Tot";
                else  XepHang = "Xuat sac";

                Console.WriteLine(student.GetInfor() + " Xep hang: " + XepHang);
            }
        }
    }
}
