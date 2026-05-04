namespace YourDarkSoulsAssistant.Core.Models;

public class BaseModel<T> where T : class
{
    public T Id { get; set; }
    
}