using System.Security.Cryptography;

namespace TH
{
    internal class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Class()
        {

        }

        public Class(int _id, string _name)
        {
            this.Id = _id;
            this.Name = _name;
        }

        public override string ToString()
        {
            return $"{this.Id,3} {this.Name,10}";
        }
    }

    internal class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClassId { get; set; }

        public Student()
        {

        }

        public Student(int _id, string _name, int _classId)
        {
            this.Id = _id;
            this.Name = _name;
            this.ClassId = _classId;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Class> classes = new List<Class>()
            {
                new Class(){Id = 1, Name = "c1"},
                new Class(){Id = 2, Name = "c2"},
                new Class(){Id = 3, Name = "c3"},
            };

            List<Student> students = new List<Student>()
            {
                new Student(){Id = 11, Name = "st1", ClassId = 1},
                new Student(){Id = 12, Name = "st2", ClassId = 1},
                new Student(){Id = 13, Name = "st3", ClassId = 2},
            };



























            // ======= viết query linq lấy ra ( tất cả các cách có thể) ==================================================================
            /*
             * studentId, classId:
                11, 1
                12, 1
                13, 1
                11, 2
                12, 2
                13, 2
                11, 3
                12, 3
                13, 3
             */

            /*// Use query syntax
            var list1 = from cls in classes
                        from st in students
                        select new
                        {
                            studentId = st.Id,
                            classId = cls.Id
                        };

            Console.WriteLine("Cau 1 cach 1:");
            list1.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.studentId}, {item.classId}");
            });

            // Use query method select many
            var list1c2 = classes.SelectMany(st => students, (cls, st) => new
            {
                studentId = st.Id,
                classId = cls.Id
            });

            Console.WriteLine("Cau 1 cach 2:");
            list1c2.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.studentId}, {item.classId}");
            });

            // Use join with true conditions 
            var list1c3 = from cls in classes
                          join st in students on 1 equals 1
                          select new
                          {
                              classId = cls.Id,
                              studentId = st.Id
                          };
            Console.WriteLine("Cau 1 cach 3:");
            list1c3.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.studentId}, {item.classId}");
            });*/

            // =====================================================================================================================






















            /*
             * class name, count student:
                c1, 2
                c2, 1
                c3, 0
             *//*
            var list2 = classes.GroupJoin(students, cls => cls.Id, st => st.ClassId, (cls, stes) =>
            {
                return new
                {
                    ClassName = cls.Name,
                    CountStudent = stes.Count()
                };
            });

            Console.WriteLine("Cau 2 cach 1:");
            list2.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.ClassName}, {item.CountStudent}");
            });

            var list2c2 = from cls in classes
                          join st in students on cls.Id equals st.ClassId into studentGroup
                          select new
                          {
                              ClassName = cls.Name,
                              CountStudent = studentGroup.Count()
                          };
            Console.WriteLine("Cau 2 cach 2:");
            list2c2.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.ClassName}, {item.CountStudent}");
            });*/

            // =============================================================================================================================































            /*
             * class Id, count student:
                1, 2
                2, 1
             */

            /*var list3c1 = from st in students
                          group st by st.ClassId
                          into studentGroupByClassId
                          select new
                          {
                              ClassId = studentGroupByClassId.Key,
                              CountStudent = studentGroupByClassId.Count()
                          };
            Console.WriteLine("Cau 3 cach 1");
            list3c1.ToList().ForEach(item => { Console.WriteLine($"{item.ClassId}, {item.CountStudent}"); });*/











            /*var list3c2 = from cls in classes
                          join st in students on cls.Id equals st.ClassId
                          group st by st.ClassId into studentGroup
                          select new
                          {
                              ClassId = studentGroup.Key,
                              CountStudent = studentGroup.Count()
                          };
            Console.WriteLine("Cau 3 cach 2:");
            list3c2.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.ClassId}, {item.CountStudent}");
            });*/







            /*Console.WriteLine("Cau 3 cach 3:");
            var list3c3 = from st in students
                          group st by st.ClassId into stGroup
                          where stGroup.Count() > 0
                          select new
                          {
                              ClassId = stGroup.Key,
                              CountStudent = stGroup.Count()
                          };
            list3c3.ToList().ForEach(it => { Console.WriteLine($"{it.ClassId}, {it.CountStudent}"); });*/








            /*var list3c4 = classes.Join(students, cls => cls.Id, st => st.ClassId, (cls, st) =>
            {
                return new
                {
                    ClassId = cls.Id,
                    Student = st
                };
            }).GroupBy(st => st.ClassId)
            .Select(group => new
            {
                ClassId = group.Key,
                CountStudent = group.Count()
            });

            Console.WriteLine("Cau 3 cach 4:");
            foreach (var item in list3c4)
            {
                Console.WriteLine($"{item.ClassId}, {item.CountStudent}");
            }*/






            /*var list3c5 = from cls in classes
                          join st in students on cls.Id equals st.ClassId into groupStudent
                          where groupStudent.Count() > 0
                          select new
                          {
                              ClassId = cls.Id,
                              Count = groupStudent.Count()
                          };
            Console.WriteLine("Cau 3 cach 5:");
            foreach (var item in list3c5)
            {
                Console.WriteLine($"{item.ClassId}, {item.Count}");
            }*/

            //==================================================================================================================


























            /*
             class name, count student:
                c1, 2
                c2, 1
             */

            /* var list4 = classes.Join(students, cls => cls.Id, st => st.ClassId, (cls, st) =>
             {
                 return new
                 {
                     ClassName = cls.Name,
                     Student = st
                 };
             }).GroupBy(result => result.Student.ClassId).Select(result =>
             {
                 return new
                 {
                     ClassName = result.First().ClassName,
                     CountStudent = result.Count()
                 };
             });

             Console.WriteLine("Cau 4 cach 1:");
             list4.ToList().ForEach(item =>
             {
                 Console.WriteLine($"{item.ClassName}, {item.CountStudent}");
             });*/










            /*var list4c2 = from cls in classes
                          join st in students on cls.Id equals st.ClassId
                          group cls by cls.Id into gr
                          select new
                          {
                              ClassName = gr.First().Name,
                              CountStudent = gr.Count()
                          };

            Console.WriteLine("Cau 4 cach 2:");
            list4c2.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.ClassName}, {item.CountStudent}");
            });*/















            /*var list4c3 = from cls in classes
                          join st in students on cls.Id equals st.ClassId into groupStudent
                          where groupStudent.Count() > 0
                          select new
                          {
                              ClassName = cls.Name,
                              CountStudent = groupStudent.Count()
                          };

            Console.WriteLine("Cau 4 cach 3:");
            list4c3.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.ClassName}, {item.CountStudent}");
            });*/


            //=========================================================================================================================================
























            /*
             * class name, name of first student:
                c1, st1
                c2, st3
                c3, null
             */

            /*
               

            var list5 = classes.GroupJoin(students, cls => cls.Id, st => st.ClassId, (cls, stes) =>
            {
                return new
                {
                    ClassName = cls.Name,
                    NameOfFirstSt = (stes.Count() > 0) ? stes.First().Name : "null"
                };
            });

            Console.WriteLine("Cau 5 cach 1:");
            list5.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.ClassName}, {item.NameOfFirstSt}");
            });*/













            /*var list5c2 = from cls in classes
                         join st in students on cls.Id equals st.ClassId into stGroup
                         let firstStudent = stGroup.FirstOrDefault()
                         select new
                         {
                             ClassName = cls.Name,
                             FirstStudentName = firstStudent?.Name
                         };

            foreach (var item in list5c2)
            {
                Console.WriteLine($"{item.ClassName}, {item.FirstStudentName ?? "null"}");
            }*/

        }
    }
}