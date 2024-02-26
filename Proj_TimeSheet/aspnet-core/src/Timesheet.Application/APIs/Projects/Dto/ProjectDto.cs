using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using static Ncc.Entities.Enum.StatusEnum;

namespace Timesheet.APIs.Projects.Dto
{
    public class ProjectDto : EntityDto<long>
    {
        public string CustomerName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ProjectStatus Status { get; set; }
        public List<string> Pms { get; set; }
        public int ActiveMember { get; set; }
        public ProjectType ProjectType { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }

    }
}
