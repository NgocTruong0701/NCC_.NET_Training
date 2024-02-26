using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Ncc.Entities;

namespace Timesheet.APIs.Projects.Dto
{
    [AutoMap(typeof(ProjectUser))]
    public class ProjectUserDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public ProjectUserType Type { get; set; }
        public bool IsTemp { get; set; }
    }
}