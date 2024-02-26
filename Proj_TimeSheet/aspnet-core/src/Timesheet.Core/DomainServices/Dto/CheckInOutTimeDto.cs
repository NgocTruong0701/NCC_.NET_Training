using System;
using System.Collections.Generic;
using System.Text;

namespace Timesheet.DomainServices.Dto
{
    public class CheckInOutTimeDto
    {
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string Note { get; set; }
    }
}
