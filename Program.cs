using FILMESRESENHA.Repositories;
using FILMESRESENHA.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration["MongoDb:ConnectionString"]
    ?? throw new InvalidOperationException("MongoDb:ConnectionString não configurado.");

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
builder.Services.AddSingleton<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>()
      .GetDatabase(builder.Configuration["MongoDb:DatabaseName"]));

builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IResenhaRepository, ResenhaRepository>();

builder.Services.AddScoped<IFilmeService, FilmeService>();
builder.Services.AddScoped<IResenhaService, ResenhaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

// TODO [11]: Mapear endpoints de /filmes — GET (todos), GET (por id), POST, PUT, DELETE
// TODO [12]: Mapear endpoints de /resenhas — GET (todos), GET (por id), GET (por filmeId), POST, PUT, DELETE

app.MapGet("/mongo-test", async (IMongoDatabase db) =>
{
    try
    {
        var collections = await db.ListCollectionNamesAsync();
        var list = await collections.ToListAsync();
        return Results.Ok(new { Message = "Conexão com o MongoDB estabelecida!", Database = db.DatabaseNamespace.DatabaseName, Collections = list });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Falha ao conectar com o MongoDB: {ex.Message}");
    }
});

app.Run();