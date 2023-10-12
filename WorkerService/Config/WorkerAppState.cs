using CG.Infrastructure.CGConfiguration;
using CG.Infrastructure.CGModels;
using CG.Infrastructure.CGTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Config
{
    public class WorkerAppState : IAppState
    {
        public string ConMain { get; set; }
        public string ConLog { get; set; }
        public CompanyModel Company { get; set; }
        public TAppSource AppSource { get; set; }
        public string AppVersion { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool init { get; set; }
        public string logo { get; set; }
        public int ShowDelivery { get; set; }
        public int ShowInventory { get; set; }
        public int ShowProduction { get; set; }
        public int ShowPurchase { get; set; }
        public int ShowSales { get; set; }
        public int ShowPlan { get; set; }
        public int ShowGL { get; set; }
        public int ShowHR { get; set; }
        public int ShowCost { get; set; }
        public int ShowQuality { get; set; }
        public int ShowTO { get; set; }
        public int ShowNotify { get; set; }
        public int ShowPricing { get; set; }
        public string DBName { get; set; }
        public string Server { get; set; }
        public string DefTheme { get; set; }
        public string DefLangusge { get; set; }
        public string DefCurrency { get; set; }
        public int Decimels { get; set; }
        public List<Users> UserSessions { get; set; }
        public List<ItemModel> itemModels { get; set; }
    }
}
