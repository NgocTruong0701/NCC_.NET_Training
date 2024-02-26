namespace Timesheet.APIs.Projects.Dto
{
    public class TimeSheetTaskDto
    {
        public long ProjectTaskId { get; set; }
        public string TaskName { get; set; }
        public bool Billable { get; set; }
        public bool IsDefault { get; set; }
    }
}