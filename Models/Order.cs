using System.Text.Json.Serialization;

namespace Pizzeria.Models;

public class Order
{
    public int OrderId { get; set; }
    
    public int CartId { get; set; }
    [JsonIgnore]
    public virtual Cart Cart { get; set; }
    
    public int PizzaId { get; set; }
    public virtual Pizza Pizza { get; set; }
}