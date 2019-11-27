namespace PharmacyApp.ViewModels
{
        public class AdminDashboardViewModel
        {
            //public int ExpiredUsers { get; set; }
            //public int VerificationComplaints { get; set; }
            public int AuditLogs { get; set; }
        }

        public class SuperAdminDashboardViewModel
        {
        public int ExpiredUsers { get; set; }
        public int Institutions { get; set; }
            public int Administrators { get; set; }
        }

}