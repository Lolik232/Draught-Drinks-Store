namespace Core.BussinesLogic.Abstractions.Entities.Order
{
    public record OrderCreated(int Number,string Status, DateTime Date, string Sum, string Link);
    
}
