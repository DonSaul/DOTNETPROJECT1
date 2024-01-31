namespace Softserve.ProjectLab.ClientAPI.Models
{
    public class ReportData
    {
        // Ingresamos las propiedades que necesitamos para la creacion del reporte
        public string WorkOrderName { get; set; }
        public string Status { get; set; }
        public string TechnicianName { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string WorkType { get; set; }
    }
}
