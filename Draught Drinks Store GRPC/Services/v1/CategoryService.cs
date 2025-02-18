using Category.V1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using CategoryModel = Category.V1.CategoryGetResponce.Types.Category;

namespace Draught_Drinks_Store_GRPC.Services.v1
{
    public class CategoryService : Category.V1.Category.CategoryBase
    {
        bool initialized = false;

        private void Initialize()
        {
            List<CategoryModel> categories = new List<CategoryModel>() {
             new(){
                    Id = 1,
                    NameEn = "beer",
                    NameRu = "Пиво",
            },
             new(){
                Id = 2,
                NameEn = "drink",
                NameRu = "Напитки",
             },
              new(){
                Id = 3,
                NameEn = "meal",
                NameRu = "Еда",
             }
            };

            List<CategoryModel> beerSubcategories = new List<CategoryModel>() {
             new(){
                    Id = 4,
                    NameEn = "light",
                    NameRu = "Светлое",
            },
             new(){
                Id = 5,
                NameEn = "dark",
                NameRu = "Тёмное",
             }
            };

            List<CategoryModel> drinkSubcategories = new List<CategoryModel>() {
             new(){
                    Id = 6,
                    NameEn = "cola",
                    NameRu = "Кола",
            },
             new(){
                Id = 7,
                NameEn = "baikal",
                NameRu = "Байкал",
             }
            };


            categories[0].Subcategories.AddRange(beerSubcategories);
            categories[1].Subcategories.AddRange(drinkSubcategories);

            _categories = categories;
        }


        private List<CategoryModel> _categories = new();

        void FindCategory(int id, ref bool found, CategoryModel currentCategory, ref CategoryModel founded)
        {
            foreach (var category in currentCategory.Subcategories)
            {
                if (category.Id == id)
                {
                    found = true;
                    founded = category;
                    return;
                }
                found = false;
                FindCategory(id, ref found, category, ref founded);
                if (found) return;
            }
        }

        public CategoryModel FindCategory(int id)
        {
            if (!initialized) { Initialize(); }

            foreach (var category in _categories)
            {
                if (category.Id == id)
                {
                    return category;
                }
                var found = false;
                CategoryModel founded = new();
                FindCategory(id, ref found, category, ref founded);
                if (found) return founded;
            }

            throw new ArgumentException();
        }


        void FindCategory(string nameEn, ref bool found, CategoryModel currentCategory, ref CategoryModel founded)
        {
            foreach (var category in currentCategory.Subcategories)
            {
                if (category.NameEn == nameEn)
                {
                    found = true;
                    founded = category;
                    return;
                }
                found = false;
                FindCategory(nameEn, ref found, category, ref founded);
                if (found) return;
            }
        }

        public CategoryModel FindCategory(string nameEn)
        {
            if (!initialized) { Initialize(); }

            foreach (var category in _categories)
            {
                if (category.NameEn == nameEn)
                {
                    return category;
                }
                var found = false;
                CategoryModel founded = new();
                FindCategory(nameEn, ref found, category, ref founded);
                if (found) return founded;
            }

            throw new ArgumentException();
        }



        public override Task<CategoryGetResponce> Get(CategoryGetRequest request, ServerCallContext context)
        {
            if (!initialized) { Initialize(); }


            CategoryModel category = new();
            switch (request.FilterCase)
            {
                case CategoryGetRequest.FilterOneofCase.Id:
                    category = FindCategory(request.Id);
                    break;
                case CategoryGetRequest.FilterOneofCase.NameEn:
                    category = FindCategory(request.NameEn);
                    break;

            }

            var responce = new CategoryGetResponce();
            responce.Category = category;

            return Task.FromResult(responce);
        }

        public override Task<CategoryGetAllResponce> GetAll(Empty request, ServerCallContext context)
        {
            if (!initialized) { Initialize(); }

            var responce = new CategoryGetAllResponce();
            responce.Categories.AddRange(_categories);

            return Task.FromResult(responce);
        }
    }
}
