namespace SlothEnterprise.ProductApplication.Interfaces
{
    public interface ISellerApplication
    {
        IProduct Product { get; set; }

        ISellerCompanyData CompanyData { get; set; }
    }
}