namespace DEMO2.Models
{
    public class Technician
    {
        int technicianId { get; set; }
        string technician { get; set; }
        string address { get; set; }
        WorkOrder workOrder { get; set; }
    }
}
