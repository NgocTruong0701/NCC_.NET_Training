using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Ncc.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timesheet.APIs.Projects.Dto
{
    [AutoMap(typeof(ProjectTask))]
    public class ProjectTaskDto : EntityDto<long>
    {
        public long TaskId { get; set; }
        public bool Billable { get; set; }
    }
}
