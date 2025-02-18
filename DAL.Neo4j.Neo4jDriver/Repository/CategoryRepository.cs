using DAL.Abstractions.Entities;
using DAL.Abstractions.Interfaces.Repositories;
using Neo4j.Driver;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DAL.Neo4j.Neo4jDriver.Repository
{
    internal class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nameRu")]
        public string NameRu { get; set; } = null!;

        [JsonProperty("nameEn")]
        public string NameEn { get; set; } = null!;

        [JsonProperty("parent")]
        public List<Category> Subcategories { get; set; } = new List<Category>();

        public static DAL.Abstractions.Entities.Category ConvertFromNeo4j(Category c)
        {
            return new DAL.Abstractions.Entities.Category
            {
                Id = c.Id,
                NameEn = c.NameEn,
                NameRu = c.NameRu,
                Subcategories = c.Subcategories.Select(ConvertFromNeo4j).ToList()
            };
        }
    }

    public class CategoryRepository : ICategoryRepository
    { 
        private readonly IDriver _driver;

        public CategoryRepository()
        {
            string username = "neo4j";
            string password = "1234567890";
            string host = "localhost";

            _driver = GraphDatabase.Driver($"bolt://{host}:7687", AuthTokens.Basic("neo4j", "1234567890")); ;
        }

        public async Task<ICollection<int>?> AllSubcategoriesIdsAsync(string name)
        {
            using var session = _driver.AsyncSession();

            try
            {
                var result = await session.RunAsync(
                    $$"""
                    MATCH (c:Category {nameEn:{{name}}}) -[r:PARENT *]->(c1:Category)
                    return c1.id
                    """);

                var ids = (await result.ToListAsync()).Select(record => record["id"].As<int>()).ToList();

                result = await session.RunAsync(
                    $$"""
                    MATCH (c:Category {nameEn:{{name}}})  return c.id;
                    """);
                var rootId = await result.SingleAsync();
                ids.Add(rootId["id"].As<int>());

                return ids;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<ICollection<int>?> AllSubcategoriesIdsAsync(int id)
        {
            using var session = _driver.AsyncSession();

            try
            {
                var result = await session.RunAsync(
                    $$"""
                MATCH (c:Category {id:{{id}}}) -[r:PARENT *]->(c1:Category)
                return c1.id
                """);

                var ids = (await result.ToListAsync()).Select(record => record["id"].As<int>()).ToList();
                ids.Add(id);

                return ids;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public Abstractions.Entities.Category Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Abstractions.Entities.Category>?> GetAllAsync()
        {

            using var session = _driver.AsyncSession();
            try
            {
                var result = await session.RunAsync(
                    "MATCH path = (c:Category) -[*]->(c_ch) WHERE NOT (c)<-[:PARENT]-() and NOT (c_ch)-->() " +
                    "WITH collect(path) AS ps " +
                    "CALL apoc.convert.toTree(ps) yield value " +
                    "RETURN value");

                var records = await result.ToListAsync();

                List<Category> categories = new();

                foreach (var record in records)
                {
                    var treeJson = JsonConvert.SerializeObject(record["value"].As<object>());
                    var tree = JsonConvert.DeserializeObject<Category>(treeJson);

                    categories.Add(tree);
                }


                return categories.Select(Category.ConvertFromNeo4j).ToList();

            }
            catch (InvalidOperationException)
            {
                return null;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<Abstractions.Entities.Category?> GetAsync(int id)
        {
            using var session = _driver.AsyncSession();
            try
            {
                var result = await session.RunAsync(
                    $"MATCH path = (c:Category) -[*]->(c_ch) WHERE c.id = {id}  and NOT (c_ch)-->() " +
                    "WITH collect(path) AS ps " +
                    "CALL apoc.convert.toTree(ps) yield value " +
                    "RETURN value");

                var record = await result.SingleAsync();

                var treeJson = JsonConvert.SerializeObject(record["value"].As<object>());
                var category = JsonConvert.DeserializeObject<Category>(treeJson);

                return Category.ConvertFromNeo4j(category!);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}
