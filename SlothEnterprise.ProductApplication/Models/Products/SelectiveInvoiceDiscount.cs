using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Interfaces;

namespace SlothEnterprise.ProductApplication.Models.Products
{
    public class SelectiveInvoiceDiscount : IProduct
    {
        private readonly ISelectInvoiceService _selectInvoiceService;
        private readonly ISellerApplication _application;

        public SelectiveInvoiceDiscount(
            ISelectInvoiceService selectInvoiceService,
            ISellerApplication application)
        {
            _selectInvoiceService = selectInvoiceService;
            _application = application;
        }

        public int Id { get; set; }

        /// <summary>
        /// Proposed networth of the Invoice
        /// </summary>
        public decimal InvoiceAmount { get; set; }

        /// <summary>
        /// Percentage of the networth agreed and advanced to seller
        /// </summary>
        public decimal AdvancePercentage { get; set; } = 0.80M;

        public int SubmitApplication()
        {
            return _selectInvoiceService
                .SubmitApplicationFor(
                    _application.CompanyData.Number.ToString(),
                    InvoiceAmount,
                    AdvancePercentage
                );
        }
    }
}