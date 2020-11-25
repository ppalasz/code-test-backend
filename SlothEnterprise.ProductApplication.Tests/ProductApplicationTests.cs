using System;
using FluentAssertions;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Interfaces;
using SlothEnterprise.ProductApplication.Models.Applications;
using SlothEnterprise.ProductApplication.Models.Products;
using SlothEnterprise.ProductApplication.Services;
using Xunit;
using Moq;
using SlothEnterprise.ProductApplication.Tests.Models;

namespace SlothEnterprise.ProductApplication.Tests
{
    public class ProductApplicationTests
    {
        private readonly IProductApplicationService _productApplicationService;
        private ISellerApplication _sellerApplication;
        
        private readonly Mock<IApplicationResult> _aplicationResultMock;
        private readonly Mock<ISellerApplication> _sellerApplicationMock = new Mock<ISellerApplication>();

        private readonly Mock<IConfidentialInvoiceService> _confidentialInvoiceServiceMock;
        private readonly Mock<IBusinessLoansService> _businessLoansServiceMock;
        private readonly Mock<ISelectInvoiceService> _selectInvoiceServiceMock;

        public ProductApplicationTests()
        {
            _businessLoansServiceMock = new Mock<IBusinessLoansService>();
            _selectInvoiceServiceMock = new Mock<ISelectInvoiceService>();
            _confidentialInvoiceServiceMock = new Mock<IConfidentialInvoiceService>();

            _aplicationResultMock = new Mock<IApplicationResult>();

            _productApplicationService = new ProductApplicationService(
                    _selectInvoiceServiceMock.Object,
                    _confidentialInvoiceServiceMock.Object,
                    _businessLoansServiceMock.Object
                );

            _sellerApplicationMock.SetupProperty(
                p => p.CompanyData,
                new SellerCompanyData()
            );
        }

        [Fact]
        public void SubmitApplicationFor_SelectiveInvoiceDiscount()
        {
            #region arrange

            _aplicationResultMock.SetupProperty(p => p.Success, true);
            _aplicationResultMock.SetupProperty(p => p.ApplicationId, 1);
            _sellerApplicationMock.SetupProperty(
                p => p.Product,
                new SelectiveInvoiceDiscount()
            );

            _sellerApplication = _sellerApplicationMock.Object;

            _selectInvoiceServiceMock
                .Setup<int>(
                    m => m.SubmitApplicationFor(
                            It.IsAny<string>(),
                            It.IsAny<decimal>(),
                            It.IsAny<decimal>()
                        )
                ).Returns(2);
            
            #endregion arrange

            //act
            var result = _productApplicationService.SubmitApplicationFor(_sellerApplication);

            //assert
            result.Should().Be(2, "SelectiveInvoiceDiscount");
        }

        [Theory]
        [InlineData(true, 1, 1)]
        [InlineData(false, 1, -1)]
        [InlineData(true, null, -1)]
        public void SubmitApplicationFor_ConfidentialInvoiceDiscount(bool success, int? applicationId, int result)
        {
            #region arrange

            _aplicationResultMock.SetupProperty(p => p.Success, success);
                _aplicationResultMock.SetupProperty(p => p.ApplicationId, applicationId);
                _sellerApplicationMock.SetupProperty(
                    p => p.Product,
                    new ConfidentialInvoiceDiscount()
                );

            _sellerApplication = _sellerApplicationMock.Object;

            _confidentialInvoiceServiceMock
                .Setup<IApplicationResult>(
                    m => m.SubmitApplicationFor(
                        It.IsAny<CompanyDataRequest>(),
                        It.IsAny<decimal>(),
                        It.IsAny<decimal>(),
                        It.IsAny<decimal>()
                    )
                ).Returns(_aplicationResultMock.Object);

            #endregion arrange

            //act & assert
            _productApplicationService
                .SubmitApplicationFor(_sellerApplication)
                .Should()
                .Be(result);
        }
        

        [Theory]
        [InlineData(true, 1, 1)]
        [InlineData(false, 1, -1)]
        [InlineData(true, null, -1)]
        public void SubmitApplicationFor_BusinessLoans(bool success, int? applicationId, int result)
        {
            #region arrange

            _aplicationResultMock.SetupProperty(p => p.Success, success);
            _aplicationResultMock.SetupProperty(p => p.ApplicationId, applicationId);
            _sellerApplicationMock.SetupProperty(
                p => p.Product,
                new BusinessLoans()
            );

            _sellerApplication = _sellerApplicationMock.Object;

            _businessLoansServiceMock
                .Setup<IApplicationResult>(
                    m => m.SubmitApplicationFor(
                        It.IsAny<CompanyDataRequest>(),
                        It.IsAny<LoansRequest>()
                    )
                ).Returns(_aplicationResultMock.Object);

            #endregion arrange

            //act & assert
            _productApplicationService.SubmitApplicationFor(_sellerApplication)
                .Should()
                .Be(result);
        }


        [Fact]
        public void SubmitApplicationFor_Dummy()
        {
            #region arrange

            _sellerApplicationMock.SetupProperty(
                p => p.Product,
                new DummyProduct()
            );

            _sellerApplication = _sellerApplicationMock.Object;

            _businessLoansServiceMock
                .Setup<IApplicationResult>(
                    m => m.SubmitApplicationFor(
                        It.IsAny<CompanyDataRequest>(),
                        It.IsAny<LoansRequest>()
                    )
                ).Returns(_aplicationResultMock.Object);

            #endregion arrange

            //act and assert, should throw an exception because it's dummy product
            Assert.Throws<InvalidOperationException>(() => _productApplicationService.SubmitApplicationFor(_sellerApplication));
        }

       
    }
}