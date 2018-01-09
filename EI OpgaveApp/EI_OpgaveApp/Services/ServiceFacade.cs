using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Services
{
    public class ServiceFacade
    {
        private static ServiceFacade serviceFacade;
        private JobRecLineService jobRecLineService;
        private MaintenanceActivityService maintenanceActivityService;
        private MaintenanceService maintenanceService;
        private PDFService pdfService;
        private ResourcesService resourcesServce;
        private SalesPersonService salesPersonService;
        private TimeRegistrationService timeRegistrationService;
        private ThreadManager threadManager;
        private CustomerService customerService;
        private JobService jobService;
        private JobTaskService jobTaskService;
        private TimeRegStatsService timeRegStatsService;
        private AppErrorService appErrorService;
        private FixedAssetService fixedAssetService;
        private WorkTypeService workTypeService;
        private ResourcePersonService resourcePersonService;
        public static ServiceFacade GetInstance
        {
            get
            {
                if (serviceFacade == null)
                {
                    serviceFacade = new ServiceFacade();
                }
                return serviceFacade;
            }
        }

        public JobRecLineService JobRecLineService
        {
            get
            {
                if (jobRecLineService == null)
                {
                    jobRecLineService = new JobRecLineService();
                }
                return jobRecLineService;
            }
        }

        public MaintenanceActivityService MaintenanceActivityService
        {
            get
            {
                if (maintenanceActivityService == null)
                {
                    maintenanceActivityService = new MaintenanceActivityService();
                }
                return maintenanceActivityService;
            }
        }

        public MaintenanceService MaintenanceService
        {
            get
            {
                if (maintenanceService == null)
                {
                    maintenanceService = new MaintenanceService();
                }
                return maintenanceService;
            }
        }

        public PDFService PDFService
        {
            get
            {
                if (pdfService == null)
                {
                    pdfService = new PDFService();
                }
                return pdfService;
            }
        }

        public ResourcesService ResourcesService
        {
            get
            {
                if (resourcesServce == null)
                {
                    resourcesServce = new ResourcesService();
                }
                return resourcesServce;
            }
        }

        public SalesPersonService SalesPersonService
        {
            get
            {
                if (salesPersonService == null)
                {
                    salesPersonService = new SalesPersonService();
                }
                return salesPersonService;
            }
        }

        public TimeRegistrationService TimeRegistrationService
        {
            get
            {
                if (timeRegistrationService == null)
                {
                    timeRegistrationService = new TimeRegistrationService();
                }
                return timeRegistrationService;
            }
        }

        public ThreadManager ThreadManager
        {
            get
            {
                if (threadManager == null)
                {
                    threadManager = new ThreadManager();
                }
                return threadManager;
            }
        }

        public CustomerService CustomerService
        {
            get
            {
                if (customerService == null)
                {
                    customerService = new CustomerService();
                }
                return customerService;
            }
        }
        public JobService JobService
        {
            get
            {
                if (jobService == null)
                {
                    jobService = new JobService();
                }
                return jobService;
            }
        }
        public JobTaskService JobTaskService
        {
            get
            {
                if (jobTaskService == null)
                {
                    jobTaskService = new JobTaskService();
                }
                return jobTaskService;
            }
        }
        public TimeRegStatsService TimeRegStatsService
        {
            get
            {
                if (timeRegStatsService == null)
                {
                    timeRegStatsService = new TimeRegStatsService();
                }
                return timeRegStatsService;
            }
        }
        public AppErrorService AppErrorService
        {
            get
            {
                if (appErrorService == null)
                {
                    appErrorService = new AppErrorService();
                }
                return appErrorService;
            }
        }
        public FixedAssetService FixedAssetService
        {
            get
            {
                if(fixedAssetService == null)
                {
                    fixedAssetService = new FixedAssetService();
                }
                return fixedAssetService;
            }
        }
        public WorkTypeService WorkTypeService
        {
            get
            {
                if (workTypeService == null)
                {
                    workTypeService = new WorkTypeService();
                }
                return workTypeService;
            }
        }
        public ResourcePersonService ResourcePersonService
        {
            get
            {
                if (resourcePersonService == null)
                {
                    resourcePersonService = new ResourcePersonService();
                }
                return resourcePersonService;
            }
        }
    }
}
