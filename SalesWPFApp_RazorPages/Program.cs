using Microsoft.AspNetCore.Authentication.Cookies;
using SalesWPFApp.Services.Implementations;
using SalesWPFApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Login"; // Redirect here if not authenticated
            options.LogoutPath = "/Logout"; // Redirect here after logout
            options.AccessDeniedPath = "/AccessDenied"; // Redirect here if not authorized
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set cookie expiration
            options.SlidingExpiration = true; // Reset the expiration time on each request
        });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookCategoryService, BookCategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
