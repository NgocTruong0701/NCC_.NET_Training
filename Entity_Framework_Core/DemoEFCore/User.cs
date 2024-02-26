using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEFCore
{
    // Lớp User lưu thông tin người dùng
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        
        [MaxLength(50)]
        public string LastName { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime BirthDate { get; set; }

        public UserRole Role { get; set; }
        public virtual List<Post> Posts { get; set; }
    }

    // định nghĩa enum UserRole để đại diện vai trò người dùng
    public enum UserRole
    {
        Admin,
        User,
        Guest
    }
}
