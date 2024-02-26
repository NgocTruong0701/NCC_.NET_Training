using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Ncc.Entities;

namespace Timesheet.APIs.Projects.Dto
{
    [AutoMap(typeof(ProjectTargetUser))]
    public class ProjectTargetUsersDto : EntityDto<long>
    { 
        public long UserId { get; set; }
        public string RoleName { get; set; }
    }
}