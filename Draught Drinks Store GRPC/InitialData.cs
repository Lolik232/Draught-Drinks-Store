using DAL.Abstractions.Interfaces.Repositories;
using DAL.EFCore.PostgreSQL;
using DAL.Neo4j.Neo4jDriver.Repository;
using Neo4j.Driver;
using static System.Net.WebRequestMethods;

namespace Draught_Drinks_Store_GRPC
{
    public class InitialData : BackgroundService
    {
        private readonly ShopContext _context;
        private readonly ICategoryRepository _categoryRepo;

        public InitialData()
        {
            _categoryRepo = new CategoryRepository();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (_context.Products.Count() == 0)
                {
                    _context.Products.AddRange(
                        new List<DAL.Abstractions.Entities.Product> {
                            new(){
                            ImageUrl = "https://alivaria.by/media/41457/baltika_3_05l_2016.png?height=1140&mode=max",
                            CategoryId = 6,
                            Name = "Балтика 3",
                            Price = 67.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 100}
                            },
                             new(){
                            ImageUrl = "https://forumsamogon.ru/wp-content/uploads/3/8/4/384623521015aef85ed483c61c3d9032.jpg",
                            CategoryId = 7,
                            Name = "Балтика 9",
                            Price = 90.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 50}
                            },
                                new(){
                            ImageUrl = "https://lexa.bonsai-sushi.ru/wp-content/uploads/2021/12/Coca-Cola_0-5.jpg",
                            CategoryId = 4,
                            Name = "Кола",
                            Price = 100.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 150}
                            },
                             new(){
                            ImageUrl = "https://vol.selhozproduct.ru/upload/usl/f_625f096a16946.jpg",
                            CategoryId = 3,
                            Name = "Сосиски",
                            Price = 150.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 150}
                            },  new(){
                            ImageUrl = "https://www.positive-market.ru/f/store/offers/m9fz5tf8xvcv8ffs.jpg",
                            CategoryId = 5,
                            Name = "Байкал",
                            Price = 50.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 40}
                            }, new(){
                            ImageUrl = "http://home.nubo.ru/pavel_egorov/russian/rdm0501.jpg",
                            CategoryId = 6,
                            Name = "Фирменное пиво \"Кузьмич\"",
                            Price = 130.00m,
                            ProductsInStocks = new DAL.Abstractions.Entities.ProductsInStock{
                                Count = 50}
                            },
                        }
                        );
                    _context.SaveChanges();
                }
                var categories = await _categoryRepo.GetAllAsync();
                if (categories.Count == 0)
                {
                    IDriver driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "1234567890"));
                    using var session = driver.AsyncSession();
                    try
                    {
                        var result = await session.RunAsync(
                        """
                            CREATE
                            (c1:Category {nameRu: 'Пиво', nameEn: 'beer', id: 1}),
                            (c2:Category {nameRu: 'Напитки', nameEn: 'drinks', id: 2}),
                            (c3:Category {nameRu: 'Еда', nameEn: 'meal', id: 3}),
                            (c4:Category {nameRu: 'Кола', nameEn: 'cola', id: 4}),
                            (c5:Category {nameRu: 'Байкал', nameEn: 'baikal', id: 5}),
                            (c6:Category {nameRu: 'Светлое', nameEn: 'light', id: 6}),
                            (c7:Category {nameRu: 'Тёмное', nameEn: 'dark', id: 7}),
                            (c1)-[:PARENT]->(c6),(c1)-[:PARENT]->(c7),
                            (c2)-[:PARENT]->(c4),(c2)-[:PARENT]->(c5);
                            """);
                    }
                    catch (InvalidOperationException invalidOperation)
                    {
                        await Console.Out.WriteLineAsync(invalidOperation.Message);
                    }
                    finally
                    {
                        await session.CloseAsync();
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
    }
}
