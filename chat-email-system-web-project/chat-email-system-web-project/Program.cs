using chat_email_system_web_project.Hubs;
using chat_email_system_web_project.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register Configuration for Dependency Injection
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Register UserRepository as a Scoped Service
builder.Services.AddScoped<UserRepository>();

// Add Session Services (Configured via appsettings.json)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(builder.Configuration.GetValue<int>("Session:IdleTimeout"));
    options.Cookie.HttpOnly = builder.Configuration.GetValue<bool>("Session:CookieHttpOnly");
    options.Cookie.IsEssential = true;
});

// Add CORS Policy (Configured via appsettings.json)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>())
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add Controllers, Views, and SignalR
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure Error Handling for Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Enable Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable CORS
app.UseCors("AllowSpecificOrigins");

// Enable Sessions Before Authorization
app.UseSession();
app.UseAuthorization();

// Map SignalR Hub
app.MapHub<ChatHub>("/chatHub");

// Map Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
