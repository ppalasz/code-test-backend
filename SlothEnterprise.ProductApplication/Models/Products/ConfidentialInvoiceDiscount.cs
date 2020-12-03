using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Interfaces;

namespace SlothEnterprise.ProductApplication.Models.Products
{
    public class ConfidentialInvoiceDiscount : IProduct
    {
        private readonly IConfidentialInvoiceService _confidentialInvoiceService;
        private readonly ISellerApplication _application;

        public ConfidentialInvoiceDiscount(
            IConfidentialInvoiceService confidentialInvoiceService,
            ISellerApplication application)
        {
            _confidentialInvoiceService = confidentialInvoiceService;
            _application = application;
        }

        public int Id { get; set; }

        public decimal TotalLedgerNetworth { get; set; }

        public decimal AdvancePercentage { get; set; }

        public decimal VatRate { get; set; } = VatRates.UkVatRate;

        public int SubmitApplication()
        {
            var result = _confidentialInvoiceService
                .SubmitApplicationFor(
                    new CompanyDataRequest
                    {
                        CompanyFounded = _application.CompanyData.Founded,
                        CompanyNumber = _application.CompanyData.Number,
                        CompanyName = _application.CompanyData.Name,
                        DirectorName = _application.CompanyData.DirectorName
                    },
                    TotalLedgerNetworth,
                    AdvancePercentage,
                    VatRate
                );

            return result.Success ? result.ApplicationId ?? -1 : -1;
        }
    }
}