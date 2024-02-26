using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DemoEFCore
{
    [PrimaryKey(nameof(BlogId))]
    public class Blog
    {
        public int BlogId { get; set; }

        [Required]
        public string BlogName { get; set; }
        public string BlogUrl { get; set; }

        public virtual List<Post> Posts { get; set; }   
    }
}
