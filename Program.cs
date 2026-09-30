using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PraiaStore.Api.Data;
using PraiaStore.Api.Handlers;
using PraiaStore.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------- Banco de dados (EF Core + SQLite) ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
        // A migration foi versionada junto com o código; evita que diferenças de
        // formatação do snapshot entre versões do EF impeçam a aplicação de subir.
        .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

// ---------- Camada de serviços ----------
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// ---------- Tratamento global de erros (ProblemDetails) ----------
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers();

// ---------- Versionamento: /api/v1/... ----------
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true; // devolve o header "api-supported-versions"
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";      // grupo "v1"
        options.SubstituteApiVersionInUrl = true; // mostra /api/v1/... no Swagger
    });

// ---------- Documentação OpenAPI ----------
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "PraiaStore API";
        document.Info.Version = "v1";
        document.Info.Description = "API RESTful para gestão de uma loja de moda praia (categorias, produtos e clientes).";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Cria o banco, aplica as migrations pendentes e insere dados de exemplo.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

app.UseExceptionHandler();
app.UseStatusCodePages();

// Swagger habilitado em todos os ambientes para facilitar a avaliação.
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "PraiaStore API v1");
    options.RoutePrefix = "swagger";
});

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapControllers();

app.Run();
