using Abp.Application.Services.Dto;
using Ncc.Entities;
using System.Collections.Generic;

namespace Timesheet.APIs.Projects.Dto
{
    public class ProjectsIncludingTasksDto : EntityDto<long>
    {
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string ProjectCode { get; set; }
        public ProjectUserType ProjectUserType { get; set; }
        public List<string> ListPM { get; set; }
        public List<TimeSheetTaskDto> Tasks { get; set;}
        public List<TimeSheetTargetUserDto> TargetUsers { get; set; }
    }
}