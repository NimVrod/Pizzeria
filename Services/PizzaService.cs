using Microsoft.EntityFrameworkCore;
using Pizzeria.Models;
using Pizzeria.Data;
using System;
using System.Data;
using Duende.IdentityServer.Extensions;

namespace Pizzeria.Service;

public class PizzaService
{
    private readonly PizzaDbContext _context;

    public PizzaService(PizzaDbContext context)
    {
        _context = context;
    }

    public Pizza GetPizzaById(int id)
    {
        var p = _context.Pizzas.Find(id);
        if (p == null)
        {
            throw new Exception("Pizza not found");
        }

        return p;
    }

    public IEnumerable<Pizza> GetAllPizzas()
    {
        return _context.Pizzas.ToList();
    }

    public void CreatePizza(Pizza pizza)
    {
        _context.Pizzas.Add(pizza);
        _context.SaveChanges();
    }

    public bool DeletePizza(int id)
    {
        var p = _context.Pizzas.Find(id);
        if (p == null)
        {
            throw new Exception("Pizza not found");
        }

        _context.Pizzas.Remove(p);
        _context.SaveChanges();
        return true;
    }

    public Cart Order(Pizza pizza, Cart cart)
    {
        var order = new Order {Cart = cart, Pizza = pizza};
        _context.Orders.Add(order);
        _context.SaveChanges();
        cart.Orders.Add(order);
        _context.SaveChanges();
        return cart;
    }
    
    public IEnumerable<Cart> GetAllOrders()
    {
        return _context.Carts.ToList();
    }

    public Cart intializecartifnotexists(string userid)
    {
        var cart = _context.Carts.FirstOrDefault(c => c.UserId == userid && c.Ordered == false);
        
        if (cart == null)
        {
            cart = new Cart { UserId = userid, Ordered = false };
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }
        
        return cart;

    }


    public IEnumerable<Order> getordersfromcart(int id)
    {
        var Orders = _context.Orders.Where(o => o.CartId == id);
        return Orders.ToList();
    }
    
    public bool DeleteOrder(int id)
    {
        var o = _context.Orders.Find(id);
        if (o == null)
        {
            throw new Exception("Order not found");
        }

        _context.Orders.Remove(o);
        _context.SaveChanges();
        return true;
    }

    public void OrderCart(Cart cart)
    {
        cart.Ordered = true;
        _context.SaveChanges();
    }
    
    public void deleteall(Cart cart)
    {
        var orders = _context.Orders.Where(o => o.CartId == cart.CartId);
        _context.Orders.RemoveRange(orders);
        _context.SaveChanges();
    }
}