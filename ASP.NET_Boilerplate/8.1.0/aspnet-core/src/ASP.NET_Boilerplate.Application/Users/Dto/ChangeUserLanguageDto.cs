using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Boilerplate.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}