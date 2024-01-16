namespace Softserve.ProjectLab.ClientAPI.Models
{
    public class WorkOrder
    {
        public string WorkOrderName { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public int TechnicianId { get; set; }
        public string Duration { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public int WorkTypeId { get; set; }
    }
}
