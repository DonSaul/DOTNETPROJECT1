namespace DEMO2.Models
{
    public class WorkOrder
    {
        string workOrderName {  get; set; }
        string technician { get; set; }
        string workType { get; set; }
        DateTimeOffset endTime { get; set; }
        DateTimeOffset startTime { get; set; }
    }
}
