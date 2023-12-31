using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(opts =>
{
    opts.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    //her metot �zerinde [ValidateAntiForgeryToken] belirtmeye gerek kalmayacak.
    //[IgnoreAntiforgeryToken] -> kontrol� yapmak istenmedi�i.
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); //production ortam�nda laz�ms
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
