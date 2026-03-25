using Microsoft.EntityFrameworkCore;
using SocialCrap.Data;
using SocialCrap.Service;

var builder = WebApplication.CreateBuilder(args);

// Registro de servicos principais da API.
builder.Services.AddControllers();

// CORS liberado para consumir a API do front.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Conexao com banco via EF Core.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro da camada de servico.
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICrapService, CrapService>();
builder.Services.AddScoped<IPoopService, PoopService>();
builder.Services.AddScoped<IAmizadeService, AmizadeService>();
builder.Services.AddScoped<INoticiaService, NoticiaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Swagger para testar endpoints.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline HTTP.
app.UseHttpsRedirection();

// Swagger sempre disponivel na raiz.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SocialCrap API V1");
    c.RoutePrefix = string.Empty; // Serve o Swagger em "/"
});

// Habilita CORS antes dos endpoints.
app.UseCors("AllowAll");
app.MapControllers();

app.Run();
