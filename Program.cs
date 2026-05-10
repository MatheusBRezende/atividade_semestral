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

app.MapGet("/resenhas", async (IResenhaService resenhaService) =>
{
    var resenhas = await resenhaService.GetAllResenhasAsync();
    return Results.Ok(resenhas);
});

app.MapGet("/resenhas/{id}", async (string id, IResenhaService resenhaService) =>
{
    var resenha = await resenhaService.GetResenhaByIdAsync(id);
    return resenha is not null ? Results.Ok(resenha) : Results.NotFound($"Resenha com ID '{id}' não encontrada");
});

app.MapGet("/resenhas/filme/{filmeId}", async (string filmeId, IResenhaService resenhaService) =>
{
    var resenhas = await resenhaService.GetResenhasByFilmeIdAsync(filmeId);
    return Results.Ok(resenhas);
});

app.MapPost("/resenhas", async (Resenha resenha, IResenhaService resenhaService) =>
{
    try
    {
        await resenhaService.CreateResenhaAsync(resenha);
        return Results.Created($"/resenhas/{resenha.Id}", resenha);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/resenhas/{id}", async (string id, Resenha resenha, IResenhaService resenhaService) =>
{
    if (resenha.Id != null && resenha.Id != id)
        return Results.BadRequest("O ID da resenha no corpo da requisição não corresponde ao ID da URL");

    try
    {
        await resenhaService.UpdateResenhaAsync(id, resenha);
        return Results.NoContent();
    }
    catch (ArgumentException ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.MapDelete("/resenhas/{id}", async (string id, IResenhaService resenhaService) =>
{
    try
    {
        await resenhaService.DeleteResenhaAsync(id);
        return Results.NoContent();
    }
    catch (ArgumentException ex)
    {
        return Results.NotFound(ex.Message);
    }
});

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