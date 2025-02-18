namespace Core.BussinesLogic.Abstractions.Entities.User
{
    public record UserCreate(string Email, string Password, ContactDataCreate ContactData);
}
