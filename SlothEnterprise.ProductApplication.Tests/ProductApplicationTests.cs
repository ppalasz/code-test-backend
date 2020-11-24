using FluentAssertions;
using Moq;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Interfaces;
using SlothEnterprise.ProductApplication.Models.Applications;
using SlothEnterprise.ProductApplication.Models.Products;
using SlothEnterprise.ProductApplication.Services;
using Xunit;

namespace SlothEnterprise.ProductApplication.Tests
{
    public class ProductApplicationTests
    {
        private readonly ProductApplicationService _sut;
        private readonly Mock<IConfidentialInvoiceService> _confidentialInvoiceServiceMock;
        private readonly ISellerApplication _sellerApplication;
        private readonly Mock<IApplicationResult> _result;

        public ProductApplicationTests()
        {
            _confidentialInvoiceServiceMock = new Mock<IConfidentialInvoiceService>();
            _result = new Mock<IApplicationResult>();

            var businessLoansServiceMock = new Mock<IBusinessLoansService>();
            var selectInvoiceServiceMock = new Mock<ISelectInvoiceService>();

            _sut = new ProductApplicationService(
                    selectInvoiceServiceMock.Object,
                    _confidentialInvoiceServiceMock.Object,
                    businessLoansServiceMock.Object
                );

            _result.SetupProperty(p => p.ApplicationId, 1);
            _result.SetupProperty(p => p.Success, true);

            var sellerApplicationMock = new Mock<ISellerApplication>();

            sellerApplicationMock
                .SetupProperty(
                        p => p.Product,
                        new ConfidentialInvoiceDiscount()
                    );

            sellerApplicationMock
                .SetupProperty(
                        p => p.CompanyData,
                        new SellerCompanyData()
                    );

            _sellerApplication = sellerApplicationMock.Object;
        }

        [Fact]
        public void ProductApplicationService_SubmitApplicationFor_WhenCalledWithSelectiveInvoiceDiscount_ShouldReturnOne()
        {
            //arrange
            _confidentialInvoiceServiceMock
                .Setup(m => m.SubmitApplicationFor(
                    It.IsAny<CompanyDataRequest>(),
                    It.IsAny<decimal>(),
                    It.IsAny<decimal>(),
                    It.IsAny<decimal>()))
                .Returns(_result.Object);

            //act
            var result = _sut.SubmitApplicationFor(_sellerApplication);

            //assert
            result.Should().Be(1);
        }
    }
}