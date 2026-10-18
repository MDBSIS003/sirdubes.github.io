using FuturePath.Business;
using FuturePath.Data;

namespace FuturePath.Desktop
{
    // One shared set of controllers, passed to forms through their constructors.
    public class AppController
    {
        private SqlServerStore store;
        private AccessSession access;
        private AdvisorySessionController advising;
        private AuthenticationController authentication;
        private AdminMaintenanceController maintenance;
        private ReportController reports;
        public SqlServerStore Store
        {
            get { return store; }
        }

        public AccessSession Access
        {
            get { return access; }
        }

        public AdvisorySessionController Advising
        {
            get { return advising; }
        }

        public AuthenticationController Authentication
        {
            get { return authentication; }
        }

        public AdminMaintenanceController Maintenance
        {
            get { return maintenance; }
        }

        public ReportController Reports
        {
            get { return reports; }
        }

        public AppController(SqlServerStore store)
        {
            this.store = store;
            access = new AccessSession();
            SystemClock clock = new SystemClock();
            advising = new AdvisorySessionController(store, access, clock);
            authentication = new AuthenticationController(store, access, clock);
            maintenance = new AdminMaintenanceController(store, access, clock);
            reports = new ReportController(store, access, clock);
        }
    }
}
