using FILMESRESENHA.Repositories;
using FILMESRESENHA.Services;
using MongoDB.Driver;
using FILMESRESENHA.Models;
using Scalar.AspNetCore; 

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
{
    app.MapOpenApi();
    app.MapScalarApiReference(); 
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "AtividadeSemestral v1");
    });
}


// TODO [11]: Mapear endpoints de /filmes — GET (todos), GET (por id), POST, PUT, DELETE

app.MapGet("/filmes", async (IFilmeService filmeService) =>
{
    var filmes = await filmeService.GetAllFilmesAsync();
    return Results.Ok(filmes);
});

app.MapGet("/filmes/{id}", async (string id, IFilmeService filmeService) =>
{
    var filme = await filmeService.GetFilmeByIdAsync(id);
    return filme is not null ? Results.Ok(filme) : Results.NotFound($"Filme com ID '{id}' não encontrado");
});

app.MapPost("/filmes", async (Filme filme, IFilmeService filmeService) =>
{
    try
    {
        await filmeService.CreateFilmeAsync(filme);
        return Results.Created($"/filmes/{filme.Id}", filme);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/filmes/{id}", async (string id, Filme filme, IFilmeService filmeService) =>
{

    if (filme.Id != null && filme.Id != id)
    {
        return Results.BadRequest("O ID do filme no corpo da requisição não corresponde ao ID da URL");
    }
    await filmeService.UpdateFilmeAsync(id, filme);
    return Results.NoContent();
});

app.MapDelete("/filmes/{id}", async (string id, IFilmeService filmeService) =>
{
    try
    {
        await filmeService.DeleteFilmeAsync(id);
        return Results.NoContent();
    }
    catch (ArgumentException ex)
    {
        return Results.NotFound(ex.Message); 
    }
});

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