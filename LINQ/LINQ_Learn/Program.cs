using System.Xml.Linq;

namespace LINQ_Learn
{
    internal class Program
    {
        // LINQ (Language Integrated Query) : Ngon ngu truy van tich hop
        // Cau truc tuong tu SQL
        // Truy van cac nguon du lieu: IEnumerable, IEnumerable<T> (Array, List, Queue, Stack,...), XML, SQL, Entity, ...
        static void Main(string[] args)
        {
            var brands = new List<Brand>()
            {
                new Brand{ID = 1, Name = "Công ty AAA"},
                new Brand{ID = 2, Name = "Công ty BBB"},
                new Brand{ID = 4, Name = "Công ty CCC"},
            };

            var products = new List<Product>()
            {
                new Product(1, "Ban tra",    400, new string[] {"Xam", "Xanh"},         2),
                new Product(2, "Tranh treo", 400, new string[] {"Vang", "Xanh"},        1),
                new Product(3, "Đen trum",   500, new string[] {"Trang"},               3),
                new Product(4, "Ban hoc",    200, new string[] {"Trang", "Xanh"},       1),
                new Product(5, "Tui da",     300, new string[] {"Đo", "Đen", "Vang"},   2),
                new Product(6, "Giuong ngu", 500, new string[] {"Trang" },              2),
                new Product(7, "Tu ao",      600, new string[] {"Trang" },              3),
            };

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            /*Console.WriteLine("Danh sach tat ca cac san pham");
            // Lay danh sach cac san pham bang LINQ syntax 
            var queryselect = from p in products select p;
            foreach (var q in queryselect)
            {
                Console.WriteLine(q);
            }

            // Lay san pham co gia = 400
            Console.WriteLine("Danh sach san pham co gia 400");
            var querywhere = from p in products where p.Price == 400 select p;
            querywhere.ToList().ForEach(x => Console.WriteLine(x));*/

            // ============= LINQ: Query Syntax ================
            // from in
            // join (inner join) - join into (left join)
            // where
            // order by
            // select
            // group by

            /*
             * 1) Xac dinh nguon: from in IEnumerables
             * ... join, where, orderby let tenbien = ??....
             * 2) Lay du lieu: select, group by,...
             */

            /*// Cu phap truy van can ban
            var querySyntaxselect = from product in products select product;
            Console.WriteLine("Query Syntax from in select: ");
            foreach (var q in querySyntaxselect)
            {
                Console.WriteLine(q);
            }

            // Cu phap truy van voi menh de where
            Console.WriteLine("Query Syntax where: ");
            var querySyntaxwhere = from product in products
                                   where product.Price == 400
                                   select new
                                   {
                                       product.Name,
                                       product.Price
                                   };
            querySyntaxwhere.ToList().ForEach(item => Console.WriteLine(item));

            // Cu phap truy van voi nhieu menh de from 
            var querySyntaxManyFrom = from product in products
                                      from color in product.Colors
                                      where product.Price <= 500 && color.Equals("Xanh")
                                      select new
                                      {
                                          Ten = product.Name,
                                          Gia = product.Price,
                                          Mau = product.Colors
                                      };
            Console.WriteLine("Query Syntax Many From: ");
            querySyntaxManyFrom.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.Ten} - {item.Gia} {string.Join(",", item.Mau)}");
            });

            // Cu phap truy van voi orderby / orderby descending
            Console.WriteLine("Query Syntax orderby: ");
            var querySyntaxOrderBy = from product in products
                                     orderby product.Price //descending
                                     select product;
            querySyntaxOrderBy.ToList().ForEach(item => Console.WriteLine(item));

            // Cu phap truy van voi groupby
            var querySyntaxGroupBy = from product in products
                                     group product by product.Brand;
            Console.WriteLine("Query Syntex Group by");
            querySyntaxGroupBy.ToList().ForEach(group =>
            {
                Console.WriteLine(group.Key);
                group.ToList().ForEach(item =>
                {
                    Console.WriteLine(item);
                });
            });

            // VD: Nhom san pham theo gia, sap xep, tinh so luong cua tung nhom
            var querySyntaxGroupVidu = from pr in products
                                       group pr by pr.Price into gr
                                       orderby gr.Key
                                       select new
                                       {
                                           PriceGroup = gr.Key,
                                           listItem = gr.ToList(),
                                           TotalPrice = gr.Count()
                                       };
            Console.WriteLine("Vi du syntax group by: ");
            querySyntaxGroupVidu.ToList().ForEach(group =>
            {
                Console.WriteLine(group.PriceGroup);
                group.listItem.ForEach(item => { Console.WriteLine(item); });
                Console.WriteLine(group.TotalPrice);
            });

            // Dung let trong syntax linq
            var querySyntaxUseLet = from p in products
                                    group p by p.Price into gr
                                    orderby gr.Key descending
                                    let total = "Tong tien la: " + gr.Sum(i => i.Price)
                                    select new
                                    {
                                        Gia = gr.Key,
                                        CacSanPham = gr.ToList(),
                                        Total = total
                                    };
            Console.WriteLine("Vi du dung let trong syntax linq:");
            querySyntaxUseLet.ToList().ForEach(gr =>
            {
                Console.WriteLine("Gia: " + gr.Gia);
                gr.CacSanPham.ForEach(item => Console.WriteLine(item));
                Console.WriteLine(gr.Total);
            });

            // Join
            var querySyntaxJoin = from p in products
                                  join b in brands on p.Brand equals b.ID
                                  select new
                                  {
                                      Ten = p.Name,
                                      Gia = p.Price,
                                      ThuongHieu = b.Name
                                  };
            Console.WriteLine("Query Syntax Join: ");
            querySyntaxJoin.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.Ten,15} {item.Gia,10} {item.ThuongHieu,10}");
            });

            // Join Into ==> dung khi ta muon lay toan bo thanh phan ve ben trai khong phu thuoc vao ben phai nhu join
            // 
            var querySyntaxJoinInto = from p in products
                                      join b in brands on p.Brand equals b.ID into list
                                      from it in list.DefaultIfEmpty()
                                      select new
                                      {
                                          Ten = p.Name,
                                          Gia = p.Price,
                                          ThuongHieu = (it != null) ? it.Name : "No Brand"
                                      };
            Console.WriteLine("Query Syntax Join Into: ");
            querySyntaxJoinInto.ToList().ForEach(item =>
            {
                Console.WriteLine($"{item.Ten,15} {item.Gia,10} {item.ThuongHieu,10}");
            });*/

            // ================ LINQ: Query Method ===================
            // Select 
            // Where
            // SelectMany
            // Min, Max, Sum, Average
            // Join - inner join
            // GroupJoin - left join
            // Take - lay ra 1 so san pham dau tien
            // Skip - bo qua 1 so san pham
            // OrderBy / OrderByDescending : sap xep tang dan /  sap xep giam dan
            // Distinct - tra ve danh sach tap hop co cac phan tu duy nhat, khong lap nhau
            // Reverse - Dao lai tap hop
            // GroupBy - Tra ve 1 tap hop voi moi phan tu la 1 nhom theo tieu chi nao do
            // Single / SingleOrDefault - Single : neu tap hop co 1 phan tu thoa man dieu kien logic thi tra ve, neu khong co hoac nhieu hon 1 thi -> loi, SingleOrDefault nhu Single tuy nhien se khong loi khi khong tim duoc phan tu nao
            // Any - tra ve true neu tap hop ton tai phan tu thoa man dieu logic nao do
            // All - tra ve true neu tap hop co tat ca phan tu phai thoa man dieu kien logic
            // Count - tra ve so luong cua tap hop, co delegate (dieu kien logic) dem so luong phan tu thoa man logic nao do


            /*// Select
            var queryMethodSelect = products.Select((p) => { return new { Ten = p.Name, Gia = p.Price }; });
            Console.WriteLine("Query Method Select: ");
            foreach (var q in queryMethodSelect)
            {
                Console.WriteLine(q);
            }

            // Where
            var queryMethodWhere = products.Where((p) => { return p.Price >= 200 && p.Price <= 500; });
            Console.WriteLine("Quey Method Where: ");
            foreach (var q in queryMethodWhere)
            {
                Console.WriteLine(q);
            }

            // SelectMany
            var queryMethodSelectMany = products.SelectMany((p) => { return p.Colors; });
            Console.WriteLine("Query Method SelectMany: ");
            foreach (var color in queryMethodSelectMany)
            {
                Console.WriteLine(color);
            }

            // Min, Max, Sum, Average
            
            Console.WriteLine("Query Method Min: " + nums.Min());
            Console.WriteLine("Query Method Max: " + nums.Max());
            Console.WriteLine("Query Method Sum: " + nums.Sum());
            Console.WriteLine("Query Method Average: " + nums.Average());

            // Join
            var queryMethodJoin = products.Join(brands, p => p.Brand, b => b.ID, (p, b) => { return new { Ten = p.Name, ThuongHieu = b.Name }; });
            Console.WriteLine("Query Method Join: ");
            foreach (var kq in queryMethodJoin)
            {
                Console.WriteLine(kq);
            }

            // JoinGroup
            var queryMethodJoinGroup = brands.GroupJoin(products, b => b.ID, p => p.Brand, (brand, pros) => { return new { ThuongHieu = brand.Name, CacSanPham = pros }; });
            Console.WriteLine("Query Method Group Join: ");
            foreach (var kq in queryMethodJoinGroup)
            {
                Console.WriteLine(kq.ThuongHieu);
                foreach (var kq2 in kq.CacSanPham)
                {
                    Console.WriteLine(kq2);
                }
            }

            // Take
            Console.WriteLine("Query Method Take: ");
            products.Take(3).ToList().ForEach(p => Console.WriteLine(p));

            // Skip
            Console.WriteLine("Query Method Skip: ");
            products.Skip(3).ToList().ForEach(p => Console.WriteLine(p));

            // OrderBy
            Console.WriteLine("Query Method OrderBy:");
            products.OrderBy(p => p.Price).ToList().ForEach(p => Console.WriteLine(p));

            // OrderByDescending 
            Console.WriteLine("Query Method OrderBy Descending:");
            products.OrderByDescending(p => p.Price).ToList().ForEach(p => Console.WriteLine(p));

            // Distinct
            Console.WriteLine("Query Method Distinct: ");
            products.SelectMany(p => p.Colors).Distinct().ToList().ForEach(c => Console.WriteLine(c));

            // Reverse
            Console.WriteLine("Query Method Reverse: ");
            nums.Reverse().ToList().ForEach(p => Console.WriteLine(p));

            // GroupBy
            Console.WriteLine("Query Method Group By: ");
            products.GroupBy(p => p.Brand).ToList().ForEach((groupP) =>
            {
                Console.WriteLine(groupP.Key);
                foreach (var kq in groupP)
                {
                    Console.WriteLine(kq);
                }
            });

            // Single / SingleOrDefault
            Console.WriteLine("Query Method Single: " + products.Single(p => p.Price == 600));
            *//*Console.Write("Query Method Single: " + products.Single(p => p.Price == 1000)); ==> Co loi*/
            /*Console.Write("Query Method Single: " + products.Single(p => p.Price == 400)); ==> Co loi*//*

            Console.WriteLine("Query Method SingleOrDefault: " + products.SingleOrDefault(p => p.Price == 600));
            Console.WriteLine("Query Method SingleOrDefault: " + products.SingleOrDefault(p => p.Price == 1000)); // ==> khong Co loi, ket qua la null
            *//*Console.Write("Query Method SingleOrDefault: " + products.SingleOrDefault(p => p.Price == 400)); ==> Co loi*//*

            // Any
            Console.WriteLine("Query Method Any: " + products.Any(p => p.Price == 400)); // true
            Console.WriteLine("Query Method Any: " + products.Any(p => p.Price == 4000)); // fale

            // All 
            Console.WriteLine("Query Method All: " + products.All(p => p.Price == 400)); // false
            Console.WriteLine("Query Method All: " + products.All(p => p.Price >= 100 && p.Price <= 600)); // true

            // Count
            Console.WriteLine("Query Method Count: " + products.Count()); // 7
            Console.WriteLine("Query Method Count: " + products.Count(p => p.Brand == 1)); // 2

            // Vi du: Hay in ra ten san pham, ten thuong hieu, co gia (300 - 400), ket qua sap xep theo gia giam dan
            Console.WriteLine("Vi du: ");
            products
                .Where(p => p.Price >= 300 && p.Price <= 400)
                .OrderByDescending(p => p.Price)
                .Join(brands, p => p.Brand, b => b.ID,
                    (p, b) =>
                        {
                            return new
                            {
                                TenSanPham = p.Name,
                                TenThuongHieu = b.Name,
                                Gia = p.Price
                            };
                        })
                .ToList().ForEach(gr =>
                {
                    Console.WriteLine($"{gr.TenSanPham,15} {gr.TenThuongHieu,15} {gr.Gia,5}");
                });

            Console.ReadLine();*/
        }
    }
}