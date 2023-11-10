using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.Authority = builder.Configuration["IdentityServiceUrl"];
        option.RequireHttpsMetadata = false;
        option.TokenValidationParameters.ValidateAudience = false;
        option.TokenValidationParameters.NameClaimType = "username";
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("customPolicy", b =>
    {
        b.AllowAnyHeader().
        AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins(builder.Configuration["ClientApp"]);
    });
});



var app = builder.Build();


app.UseCors();
app.MapReverseProxy();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.Run();
