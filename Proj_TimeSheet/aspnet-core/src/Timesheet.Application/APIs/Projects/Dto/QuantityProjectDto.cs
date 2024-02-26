using System;
using System.Collections.Generic;
using System.Text;
using static Ncc.Entities.Enum.StatusEnum;

namespace Timesheet.APIs.Projects.Dto
{
    public class QuantityProjectDto
    {
        public ProjectStatus Status { get; set; }
        public int Quantity { get; set; }
    }
}
