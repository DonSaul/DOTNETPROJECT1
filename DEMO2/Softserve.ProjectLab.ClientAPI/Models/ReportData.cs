namespace Softserve.ProjectLab.ClientAPI.Models
{
    public class ReportData
    {
        // Ingresamos las propiedades que necesitamos para la creacion del reporte
        public string WorkOrderName { get; set; }
        public string Status { get; set; }
        //!! Consulta: Segun el archivo entregado por Hector este campo tiene como ejemplo de salida '0'.
        //public int TechnicianName { get; set; }
        public string TechnicianName { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string WorkType { get; set; }
    }
}
