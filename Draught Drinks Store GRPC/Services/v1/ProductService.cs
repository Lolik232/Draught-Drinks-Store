using Core.BussinesLogic.Abstractions.Interfaces;
using Core.BussinesLogic.Abstractions.Primitives;
using Grpc.Core;
using Product.V1;

namespace Draught_Drinks_Store_GRPC.Services.v1
{
    public class ProductService : Product.V1.Product.ProductBase
    {
        private readonly IProductService _productService;

        public ProductService(IProductService productService)
        {
            _productService = productService;
        }

        public override async Task<ProductGetResponce> Get(ProductGetRequest request, ServerCallContext context)
        {
            var result = await _productService.GetProductByIdAsync(request.Id);

            if (result.IsFailture)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ""));
            }

            var productResponce = new ProductGetResponce
            {
                Product = new ProductGetResponce.Types.Product
                {
                    Id = (int)result.Value.Id,
                    ImageUrl = result.Value.ImageUrl,
                    CategoryId = (int)result.Value.CategoryId,
                    Name = result.Value.Name,
                    Price = result.Value.Price.ToString()
                }
            };

            return await Task.FromResult(productResponce);
        }

        public override async Task<ProductGetAllResponce> GetAll(ProductGetAllRequest request, ServerCallContext context)
        {
            Result<ICollection<DAL.Abstractions.Entities.Product>> result;


            switch (request.Filter?.CategoryCase)
            {
                case ProductGetAllRequest.Types.Filter.CategoryOneofCase.CategoryId:
                    result = await _productService.GetAllProductsByCategoryAsync(0, int.MaxValue, request.Filter.CategoryId);
                    break;

                case ProductGetAllRequest.Types.Filter.CategoryOneofCase.NameEn:
                    result = await _productService.GetAllProductsByCategoryAsync(0, int.MaxValue, request.Filter.NameEn);
                    break;
                default:
                    result = await _productService.GetAllProductsAsync(0, int.MaxValue);
                    break;
            }

            if (request.Filter is null)
            {
                result = await _productService.GetAllProductsAsync(0, int.MaxValue);
            }

            if (result.IsFailture)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ""));
            }

            var responce = new ProductGetAllResponce
            {
                Count = result.Value.Count
            };

            responce.Products.AddRange(result.Value.Select(pr => new ProductGetResponce.Types.Product
            {
                Id = (int)pr.Id,
                CategoryId = (int)pr.CategoryId,
                ImageUrl = pr.ImageUrl,
                Price = pr.Price.ToString().Replace(',', '.') + " RUB",
                Name = pr.Name
            }).ToList());

            return await Task.FromResult(responce);
        }
    }
}
