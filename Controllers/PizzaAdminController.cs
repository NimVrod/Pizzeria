using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizzeria.Service;
using Pizzeria.Models;


namespace Pizzeria.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PizzaAdminController : ControllerBase
{
    private readonly PizzaService _pizzaService;
    private readonly int Token = 1234;
    
    public PizzaAdminController(PizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }
    
    [HttpGet]
    public IEnumerable<Cart> Get()
    {
        return _pizzaService.GetAllOrders();
    }
    
    [HttpPost("{token}")]
    public IActionResult Post(int token, [FromBody] Pizza pizza)
    {
        if (token != Token)
        {
            return Unauthorized();
        }
        _pizzaService.CreatePizza(pizza);
        return Ok();
    }
    
    [HttpDelete("{token}/{id}")]
    public IActionResult Delete(int token, int id)
    {
        if (token != this.Token)
        {
            return Unauthorized();
        }
        
        _pizzaService.DeletePizza(id);
        return Ok();
    }
    
}