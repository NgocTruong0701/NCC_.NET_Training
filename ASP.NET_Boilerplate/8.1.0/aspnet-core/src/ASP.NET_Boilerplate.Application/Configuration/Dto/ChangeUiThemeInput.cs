using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Boilerplate.Configuration.Dto
{
    public class ChangeUiThemeInput
    {
        [Required]
        [StringLength(32)]
        public string Theme { get; set; }
    }
}
