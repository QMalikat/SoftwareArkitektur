using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeonApi.Data;
using NeonApi.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly NeonDbContext _context;

    public ProductsController(NeonDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        // Retrieve all products from the database
        return await _context.Products.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        // Retrieve a single product by ID
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound(); // Return 404 if not found
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        // Add a new product to the database
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Return 201 Created status with the newly created product
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        // Ensure the ID in the URL matches the ID of the provided product
        if (id != product.Id) return BadRequest();

        // Mark the product as modified
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        // Return 204 No Content status after a successful update
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        // Find the product by ID
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound(); // Return 404 if not found

        // Remove the product from the database
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        // Return 204 No Content status after successful deletion
        return NoContent();
    }
}