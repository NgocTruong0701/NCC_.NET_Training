using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ_Learn
{
    internal class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string[] Colors { get; set; }
        public int Brand { get; set; }

        public Product()
        {
            
        }

        public Product(int _id, string _name, double _price, string[] _color, int _brand)
        {
            this.ID = _id;
            this.Name = _name;
            this.Price = _price;
            this.Colors = _color;
            this.Brand = _brand;
        }

        // Ghi de phuong thuc ToString()
        public override string ToString()
        {
            return $"{ID, 3} {Name, 12} {Price, 5} {Brand, 3} {String.Join(",", Colors)}";
        }
    }
}
