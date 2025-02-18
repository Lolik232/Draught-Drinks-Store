using DAL.Abstractions.Entities;
using Neo4j.Driver;
using Newtonsoft.Json;
using System;

internal class Program
{
    public class Category
    {
        [JsonProperty("_id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("parent")]
        List<Category> Subcategories { get; set; } = new List<Category>();
    }

    private async static Task Main(string[] args)
    {
        IDriver driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "1234567890"));
        using var session = driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(
                       """
                MATCH (c:Category {id:10}) -[r:PARENT *]->(c1:Category)
                return c1.id
                """);
            var res = await result.SingleAsync();
        }
        catch (InvalidOperationException invalidOperation)
        {
            await Console.Out.WriteLineAsync(invalidOperation.Message);
        }

        await Console.Out.WriteLineAsync();
    }
}