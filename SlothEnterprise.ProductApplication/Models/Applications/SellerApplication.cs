using SlothEnterprise.ProductApplication.Interfaces;

namespace SlothEnterprise.ProductApplication.Models.Applications
{
    public class SellerApplication : ISellerApplication
    {
        public IProduct Product { get; set; }

        public ISellerCompanyData CompanyData { get; set; }
    }
}