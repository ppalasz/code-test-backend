using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Interfaces;
using System;
using SlothEnterprise.ProductApplication.Models.Products;

namespace SlothEnterprise.ProductApplication.Services
{
    public class ProductApplicationService: IProductApplicationService
    {
        public int SubmitApplicationFor(ISellerApplication application)
        {
            return application.Product.SubmitApplication();
        }
    }
}