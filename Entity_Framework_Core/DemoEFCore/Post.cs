using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEFCore
{
    // Lớp Post lưu thông tin bài viết
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; }
        public DateTime CreateAt { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public int BlogID { get; set; }
        public virtual Blog? Blog { get; set; }
    }
}
