using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Data;
using Pizzeria.Models;
using Pizzeria.Service;
using System;
using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Pizzeria.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PizzaController : ControllerBase
{
    PizzaService _pizzaService;

    public PizzaController(PizzaService service)
    {
        _pizzaService = service;
    }

    [HttpGet]
    public ActionResult Get()
    {
        return Ok(_pizzaService.GetAllPizzas());
    }

    [HttpGet("cart")]
    [Authorize]
    public ActionResult GetCart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = _pizzaService.intializecartifnotexists(userId);
        var orders = _pizzaService.getordersfromcart(cart.CartId);
        foreach (var order in orders)
        {
            var pizza = _pizzaService.GetPizzaById(order.PizzaId);
            order.Pizza = pizza;
        }

        return Ok(orders);
    }

    [HttpPost("{id}")]
    [Authorize]
    public ActionResult Post(int id)
    {
        //get the current user
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //return Ok("Pizza with id " + id + " added to cart for user " + userId);
        var cart = _pizzaService.intializecartifnotexists(userId);
        var pizza = _pizzaService.GetPizzaById(id);
        var updated = _pizzaService.Order(pizza, cart);
        return Ok(updated);
    }

    [HttpDelete("cart/{id}")]
    [Authorize]
    public ActionResult DeleteOrder(int id)
    {
        //Todo: check if the user is the owner of the order
        return Ok(_pizzaService.DeleteOrder(id));
    }

    [HttpPut("cart")]
    [Authorize]
    public ActionResult PostCart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = _pizzaService.intializecartifnotexists(userId);
        var orders = _pizzaService.getordersfromcart(cart.CartId);
        if (orders.IsNullOrEmpty())
        {
            return BadRequest("No pizzas in cart");
        }

        _pizzaService.OrderCart(cart);
        return Ok(cart);
    }

    [HttpDelete("cart/deleteall")]
    [Authorize]
    public ActionResult DeleteAll()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = _pizzaService.intializecartifnotexists(userId);
        _pizzaService.deleteall(cart);
        return Ok();
    }
}