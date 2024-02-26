using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Microsoft.Office.Interop.Word;
using Ncc.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static Ncc.Entities.Enum.StatusEnum;

namespace Timesheet.APIs.Projects.Dto
{
    [AutoMap(typeof(Project))]
    public class ProjectCreateEditDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string Note { get; set; }
        public ProjectType ProjectType { get; set; }
        public long CustomerId { get; set; }
        public List<ProjectTaskDto> Tasks { get; set; }
        public List<ProjectUserDto> Users { get; set; }
        public List<ProjectTargetUsersDto> ProjectTargetUsers { get; set; }
        public string KomuChannelId { get; set; }
        public bool IsNotifyToKomu { get; set; }
        public bool IsNoticeKMSubmitTS { get; set; }
        public bool IsNoticeKMRequestOffDate { get; set; }
        public bool IsNoticeKMApproveRequestOffDate { get; set; }
        public bool IsNoticeKMRequestChangeWorkingTime { get; set; }
        public bool IsNoticeKMApproveChangeWorkingTime { get; set; }
        public bool isAllUserBelongTo { get; set; }
    }
}
