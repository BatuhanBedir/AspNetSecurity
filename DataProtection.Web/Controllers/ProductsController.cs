﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataProtection.Web.Models;
using Microsoft.AspNetCore.DataProtection;

namespace DataProtection.Web.Controllers;

public class ProductsController : Controller
{
    private readonly ExampleDbContext _context;
    private readonly IDataProtector _dataProtector;
    public ProductsController(ExampleDbContext context, IDataProtectionProvider dataProtectionProvider)
    {
        _context = context;
        _dataProtector = dataProtectionProvider.CreateProtector("ProductsController"); //dprot nesnelerin birbirinden izolasyonu
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.ToListAsync();

        ITimeLimitedDataProtector timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();

        products.ForEach(x =>
        {
            //x.EncryptedId = _dataProtector.Protect(x.Id.ToString());
            x.EncryptedId = timeLimitedProtector.Protect(x.Id.ToString(), TimeSpan.FromSeconds(5));
        });
        return View(products);
    }

    [HttpPost]
    public IActionResult Index(string searchText) // ' OR '1'='1' --
    {
        //var products = _context.Products.Where(x=>x.Name == searchText).ToList();

        //var products = _context.Products.FromSqlRaw("select * from product where Name=" + "'" + searchText+"'").ToList();

        var products = _context.Products.FromSqlRaw("select * from product where name={0}",searchText).ToList();

        //var products = _context.Products.FromSqlInterpolated($"select * from product where name={searchText}").ToList();

        return View(products);
    }
    // GET: Products/Details/5
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        ITimeLimitedDataProtector timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();
        //int decryptedId = int.Parse(_dataProtector.Unprotect(id));
        int decryptedId = int.Parse(timeLimitedProtector.Unprotect(id));

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == decryptedId);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Products/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,Color,ProductCategoryId")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // POST: Products/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Color,ProductCategoryId")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Products == null)
        {
            return Problem("Entity set 'ExampleDbContext.Products'  is null.");
        }
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
      return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
