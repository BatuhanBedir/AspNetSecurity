using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowSites", builder =>
    {
        builder.WithOrigins("https://localhost:7270","https://mysitem.com").AllowAnyHeader().AllowAnyMethod();
        //sadece belirtilen siteden gelen isteklere cevap verecek
    });

    //--**--
    //sub-domaini ne olursa olsun kabul et
    //opts.AddPolicy("AllowSites", builder =>
    //{
    //    builder.WithOrigins("https:*.example.com").SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader().AllowAnyMethod();
    //});
    //--**--
    
    //--**--
    //opts.AddPolicy("AllowSites2", builder =>
    //{
    //    builder.WithOrigins("https://www.mysite2.com").WithHeaders(HeaderNames.ContentType,"x-custom-header");
    //});
    //--**--

    //--**--
    //opts.AddDefaultPolicy(builder =>
    //{
    //    //builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    //    ////AllowAnyOrigin: hangi originden gelirse gelsin cevap versin
    //    ////AllowAnyHeader: Header'ýnda ne olursa olsun
    //    ////AllowAnyMethod: Get,Post,Delete.. hangi metot gelirse gelsin izin versin
    //});
    //--**--
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSites");
//app.UseCors("AllowSites2");

//app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
