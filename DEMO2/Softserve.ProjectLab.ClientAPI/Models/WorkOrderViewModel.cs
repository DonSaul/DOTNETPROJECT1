namespace Softserve.ProjectLab.ClientAPI.Models
{
	public class WorkOrderViewModel
	{
		public IEnumerable<WorkOrderDetails>? WorkOrders { get; set; }
		public IEnumerable<Status>? Statuses { get; set; }
		public IEnumerable<WorkType>? WorkTypes { get; set; }

	}
}
