using System;
using System.Collections.Generic;
using System.Text;

namespace Timesheet.APIs.OverTimeHours.Dto
{
    public class ChamCongInfoDto
    {
        public string NormalizeEmailAddress { get; set; }
        public List<DateTime> OpenTalkDates { get; set; }
        public List<DateTime> NormalWorkingDates { get; set; }
    }
}
