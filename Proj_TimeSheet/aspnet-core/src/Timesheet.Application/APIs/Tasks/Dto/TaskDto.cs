using Abp.AutoMapper;
using Abp.Domain.Entities;
using Ncc.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Timesheet.Anotations;
using static Ncc.Entities.Enum.StatusEnum;

namespace Timesheet.APIs.Tasks.Dto
{
    [AutoMap(typeof(Task))]
    public class TaskDto : Entity<long>
    {
        
        public string Name { get; set; }
       
        public TaskType Type { get; set; }
       
        public bool IsDeleted { get; set; }
    }
}
