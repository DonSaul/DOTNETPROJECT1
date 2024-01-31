namespace Softserve.ProjectLab.ClientAPI.Models
{
    public class WorkOrderDetails
    {
        public string WorkOrderName { get; set; }
        public string Technician { get; set; }
        public string WorkType { get; set; }
        
      //Verify that this is not needed
        public string Status { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public WorkOrderDetails()
        {
            WorkOrderName = string.Empty;
            Technician = string.Empty;
            WorkType = string.Empty;
            Status = string.Empty;
            EndTime = null;
            StartTime = null;
        }
    }
}
