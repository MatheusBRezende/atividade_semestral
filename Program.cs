using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


var connectionString = builder.Configuration["MongoDb:ConnectionString"]
    ?? throw new InvalidOperationException("MongoDb: ConnectionString não configurado");

var databaseName = builder.Configuration["MongoDb:DatabaseName"]
    ?? throw new InvalidOperationException("MongoDb:DatabaseName não configurado.");

builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
builder.Services.AddSingleton<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>()
      .GetDatabase(builder.Configuration["MongoDb:DatabaseName"]));

// TODO: Registrar os repositórios/services das entidades (ex: builder.Services.AddScoped<IFilmeRepository, FilmeRepository>())

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// TODO: Mapear os endpoints de Filmes (GET, POST, PUT, DELETE em /filmes)
// TODO: Mapear os endpoints de Resenhas (GET, POST, PUT, DELETE em /resenhas)

app.MapGet("/mongo-test", async (IMongoDatabase db) =>
{
    try
    {
        var collections = await db.ListCollectionNamesAsync();
        var list = await collections.ToListAsync();
        return Results.Ok(new
        {
            Message = "Conexão com o MongoDB estabelecida!",
            Database = db.DatabaseNamespace.DatabaseName,
            Collections = list
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Falha ao conectar com o MongoDB: {ex.Message}");
    }
});

// TODO: Remover o endpoint /mongo-test quando os endpoints reais estiverem funcionando

app.Run();