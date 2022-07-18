using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text.Json.Serialization;


namespace Pizzeria.Models;

public class Pizza
{

    
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string ImageUrl { get; set; }
    public string? Ingredients { get; set; }
    
}