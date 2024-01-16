namespace Softserve.ProjectLab.ClientAPI.Config
{
    public static class ApiUrls
    {
        public static string RefreshDatabase { get; } = "/api/Configuration/RefreshDatabase";
        public static string GetStatus { get; } = "/api/Status";
        public static string GetAllTechnicians { get; } = "/api/Technician";
        public static string GetTechnicianById { get; } = "/api/Technician/";
        public static string GetAllWorkOrders { get; } = "/api/WorkOrder";
        public static string GetWorkOrderByName { get; } = "/api/WorkOrder/";
        public static string GetWorkType { get; } = "/api/WorkType";
        public static string ExportWorkOrderReportsToCsv { get; } = "/api/Reports/export-csv";
    }
}
