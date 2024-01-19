namespace Softserve.ProjectLab.ClientAPI.Models
{
    public class TechnicianDetails
    {
        public int TechnicianId { get; set; }
        public string Technician { get; set; }
        public string Address { get; set; }

        public WorkOrderDetails[] WorkOrders { get; set; }

    }
}
