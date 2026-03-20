using Microsoft.EntityFrameworkCore;
using SocialCrap.Data;

var builder = WebApplication.CreateBuilder(args);

// Registro de servicos
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline HTTP
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SocialCrap API V1");
    c.RoutePrefix = string.Empty; // Serve o Swagger em "/"
});

app.UseCors("AllowAll");
app.MapControllers();

app.Run();
