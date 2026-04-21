using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Registrar cliente e banco como serviços
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration["MongoDb:ConnectionString"]));

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

app.MapGet("/mongo-test", (IMongoDatabase db) =>
{
    var collections = db.ListCollectionNames().ToList();
    return Results.Ok(new {
        Message = "Conexão com o MongoDB estabelecida!",
        Database = db.DatabaseNamespace.DatabaseName,
        Collections = collections
    });
})
.WithName("GetMongoConnection");

// TODO: Remover o endpoint /mongo-test quando os endpoints reais estiverem funcionando

app.Run();