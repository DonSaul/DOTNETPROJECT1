namespace Softserve.ProjectLab.ClientAPI.Models
{
    public class WorkOrderDetails
    {
        public string WorkOrderName { get; set; }
        public string Technician { get; set; }
        public string WorkType { get; set; }
        public string Status { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? StartTime { get; set; }
    }
}
