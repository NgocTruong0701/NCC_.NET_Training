namespace TH2
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            // Tạo danh sách chứa các đối tượng Item
            var list1 = new List<Item>
            {
                new Item { Id = 1, Name = "a" },
                new Item { Id = 2, Name = "b" },
                new Item { Id = 3, Name = "c" }
            };

            var list2 = new List<Item>
            {
                new Item { Id = 2, Name = "b" },
                new Item { Id = 4, Name = "d" },
                new Item { Id = 5, Name = "e" }
            };

            // =========================================================================================
























            // Bài 2.1
            // list3 = [{id: 2, name: b}] ;
            Console.WriteLine("Bai 2.1:");
            var list3c1 = from item in list1
                          where item.Id == 2
                          select new
                          {
                              item.Id,
                              item.Name
                          };
            list3c1.ToList().ForEach(item =>
            {
                Console.WriteLine(item);
            });













            var list3c2 = from item1 in list1
                          join item2 in list2 on item1.Id equals item2.Id
                          select new
                          {
                              item1.Id,
                              item2.Name
                          };
            Console.WriteLine("Bai 2.1 cach 2:");
            list3c2.ToList().ForEach(item =>
            {
                Console.WriteLine(item);
            });

















            Console.WriteLine("Bai 2.1 cach 3:");
            list1.Where(item => item.Id == 2).Select(item => new {
                item.Id,
                item.Name
            }).ToList().ForEach(item =>
            {
                Console.WriteLine(item);
            });


















            Console.WriteLine("Bai 2.1 cach 4:");
            list1.Join(list2, it1 => it1.Id, it2 => it2.Id, (it1, it3) =>
            {
                return new { it1.Id, it1.Name, };
            }).ToList().ForEach(it =>
            {
                Console.WriteLine(it);
            });








            Console.WriteLine("Bai 2.1 cach 5:");
            list1.Where(item1 => list2.Any(item2 => item2.Id == item1.Id)).ToList().ForEach(item =>
            {
                Console.WriteLine($"{{id: {item.Id}, name: {item.Name}}}");
            });

            // ================================================================================================
































            // Bài 2.2:
            Console.WriteLine("Bai 2.2:");
            // list4 = [{id: 1, name: a}, {id: 3, name: c}];
            var list4c1 = from it1 in list1
                          where !list2.Any(it2 => it2.Id == it1.Id)
                          select new
                          {
                              it1.Id,
                              it1.Name,
                          };
            list4c1.ToList().ForEach(it =>
            {
                Console.WriteLine(it);
            });

            Console.WriteLine("Bai 2.2 cach 2:");
            list1.Where(it1 => (!list2.Any(it2 => it2.Id == it1.Id))).Select(it1 => new {
                it1.Id,
                it1.Name,
            }).ToList().ForEach(it =>
            {
                Console.WriteLine(it);
            });
        }
    }
}