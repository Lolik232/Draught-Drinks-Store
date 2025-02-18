using Category.V1;
using Core.BussinesLogic.Abstractions.Interfaces;

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Draught_Drinks_Store_GRPC.Services.v1
{
    public class CategoryService : Category.V1.Category.CategoryBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryService(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public override async Task<CategoryGetResponce> Get(CategoryGetRequest request, ServerCallContext context)
        {
            throw new RpcException(new Status(StatusCode.Internal, "no"));
        }

        public override async Task<CategoryGetAllResponce> GetAll(Empty request, ServerCallContext context)
        {
            var categoryResult = await _categoryService.GetAllAsync();

            if (categoryResult.IsFailture)
            {
                throw new RpcException(new Status(StatusCode.Internal, ""));
            }

            var categories = categoryResult.Value.Select(ToGRPCCategory);

            var responce = new CategoryGetAllResponce{};
            responce.Categories.AddRange(categories);

            return await Task.FromResult(responce);
        }

        private static CategoryGetResponce.Types.Category ToGRPCCategory(DAL.Abstractions.Entities.Category cat)
        {
            var category = new CategoryGetResponce.Types.Category { Id = cat.Id, NameEn = cat.NameEn, NameRu = cat.NameRu };
            category.Subcategories.AddRange(cat.Subcategories.Select(ToGRPCCategory));
            return category;
        }

    }
}
