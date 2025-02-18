namespace DAL.Abstractions.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string NameRu { get; set; } = "";
        public string NameEn { get; set; } = "";
        public virtual ICollection<Category> Subcategories { get; set; } = new List<Category>();
    }
}
