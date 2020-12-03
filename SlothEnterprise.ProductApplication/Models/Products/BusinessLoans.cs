using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Interfaces;

namespace SlothEnterprise.ProductApplication.Models.Products
{
    public class BusinessLoans : IProduct
    {
        private readonly IBusinessLoansService _businessLoansService;
        private readonly ISellerApplication _application;

        public BusinessLoans(
            IBusinessLoansService businessLoansService, 
            ISellerApplication application)
        {
            _businessLoansService = businessLoansService;
            _application = application;
        }

        public int Id { get; set; }

        /// <summary>
        /// Per annum interest rate
        /// </summary>
        public decimal InterestRatePerAnnum { get; set; }

        /// <summary>
        /// Total available amount to withdraw
        /// </summary>
        public decimal LoanAmount { get; set; }


        public int SubmitApplication()
        {
            var result = _businessLoansService
                .SubmitApplicationFor(
                    new CompanyDataRequest
                    {
                        CompanyFounded = _application.CompanyData.Founded,
                        CompanyNumber = _application.CompanyData.Number,
                        CompanyName = _application.CompanyData.Name,
                        DirectorName = _application.CompanyData.DirectorName
                    },
                    new LoansRequest
                    {
                        InterestRatePerAnnum = InterestRatePerAnnum,
                        LoanAmount = LoanAmount
                    }
                );

            return result.Success ? result.ApplicationId ?? -1 : -1;
        }
    }
}