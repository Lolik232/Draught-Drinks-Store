using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Product.V1;
using CategoryModel = Category.V1.CategoryGetResponce.Types.Category;
using ProductModel = Product.V1.ProductGetResponce.Types.Product;

namespace Draught_Drinks_Store_GRPC.Services.v1
{
    public class ProductService : Product.V1.Product.ProductBase
    {
        CategoryService categoryService = new CategoryService();

        private List<ProductModel> products = new() {
            new(){
            Id =1,
            CategoryId = 4,
            Name = "Балтика 3",
            ImageUrl = "https://alivaria.by/media/41457/baltika_3_05l_2016.png?height=1140&mode=max",
            Price = "1000 RUB"
            },
              new(){
            Id =2,
            CategoryId = 5,
            Name = "Балтика 4",
            ImageUrl = "https://alivaria.by/media/41457/baltika_3_05l_2016.png?height=1140&mode=max",
            Price = "1050 RUB"
            },

            new(){
            Id =3,
            CategoryId = 6,
            Name = "Балтика Кола",
            ImageUrl = "https://lexa.bonsai-sushi.ru/wp-content/uploads/2021/12/Coca-Cola_0-5.jpg",
            Price = "100 RUB"
            },
             new(){
            Id =4,
            CategoryId = 7,
            Name = "Байкал",
            ImageUrl = "https://all-aforizmy.ru/wp-content/uploads/2022/02/scale_1200-8.jpg",
            Price = "10000000 RUB"
            },

        };

        public override Task<ProductGetResponce> Get(ProductGetRequest request, ServerCallContext context)
        {
            return base.Get(request, context);
        }


        void GetSubcategories(CategoryModel curCategory, List<int> subcategories)
        {
            subcategories.Add(curCategory.Id);
            foreach (var subcategory in curCategory.Subcategories)
            {
                GetSubcategories(subcategory, subcategories);
            }
        }

        List<int> GetSubcategories(int categoryId)
        {
            var category = categoryService.FindCategory(categoryId);
            List<int> subcategories = new() { categoryId };

            foreach (var subcategory in category.Subcategories)
            {
                GetSubcategories(subcategory, subcategories);
            }

            return subcategories;
        }

        public override Task<ProductGetAllResponce> GetAll(ProductGetAllRequest request, ServerCallContext context)
        {
            var filter = request.Filter;
            int categoryID = 1;

            var responce = new ProductGetAllResponce();

            switch (filter.CategoryCase)
            {
                case ProductGetAllRequest.Types.Filter.CategoryOneofCase.CategoryId:
                    categoryID = filter.CategoryId;
                    break;
                case ProductGetAllRequest.Types.Filter.CategoryOneofCase.NameEn:
                    var category = categoryService.FindCategory(filter.NameEn);
                    categoryID = category.Id;
                    break;
                case ProductGetAllRequest.Types.Filter.CategoryOneofCase.None:
                    responce.Count = products.Count;
                    responce.Products.AddRange(products);
                    return Task.FromResult(responce);
            }

            var categories = GetSubcategories(categoryID);

            var productsByCategory = products.Where(pr => categories.Contains(pr.CategoryId));

            responce.Products.AddRange(productsByCategory);
            responce.Count = responce.Products.Count;


            return Task.FromResult(responce);
        }
    }
}
