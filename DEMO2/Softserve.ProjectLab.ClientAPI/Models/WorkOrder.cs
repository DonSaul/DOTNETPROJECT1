namespace Softserve.ProjectLab.ClientAPI.Models
{
    public class WorkOrder
    {
        public string WorkOrderName { get; set; }
        public int StatusId { get; set; }
        public int TechnicianId { get; set; }
        public string Duration { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int WorkTypeId { get; set; }
    }
}
