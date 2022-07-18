using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Pizzeria.Models;

public class Cart
{
    public Cart()
    {
        this.Orders = new HashSet<Order>();
    }
    
    [Key]
    public int CartId { get; set; }
    public string UserId { get; set; }
    public bool Ordered { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}