using FILMESRESENHA.Models;
using FILMESRESENHA.Repositories;
using FILMESRESENHA.Services;
using MongoDB.Driver;

//BUILD
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["MongoDb:ConnectionString"]
    ?? throw new InvalidOperationException("MongoDb:ConnectionString não configurado.");

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
builder.Services.AddSingleton<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>()
    .GetDatabase(builder.Configuration["MongoDb:DatabaseName"]));

builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IResenhaRepository, ResenhaRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IFilmeService, FilmeService>();
builder.Services.AddScoped<IResenhaService, ResenhaService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(app =>
{

    app.SwaggerDoc("v1", new()
    {
        Title = "Filmes e Resenha API",
        Version = "v1",
        Description = "Api de gerenciamento de Filmes e Resenhas"
    });
});

var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

//FILMES

var filmesApi = app.MapGroup("/api").WithTags("Filmes");

filmesApi.MapGet("/filmes", async (IFilmeService filmeService) =>
Results.Ok(await filmeService.GetAllFilmesAsync()))
.WithDescription("GET para buscar todos os filmes")
.WithSummary("GET para buscar todos os filmes")
.Produces<List<Filme>>(200)
.Produces(404);

filmesApi.MapGet("/filmes/{id}", async (string id, IFilmeService filmeService) =>
{
    var filme = await filmeService.GetFilmeByIdAsync(id);
    return filme is not null ? Results.Ok(filme) : Results.NotFound($"Filme com ID '{id}' não encontrado");
})
.WithDescription("GET para pegar um filme específico com seu ID")
.WithSummary("GET para pegar um filme específico com seu ID")
.Produces<Filme>(200)
.Produces(404);

filmesApi.MapPost("/filmes", async (Filme filme, IFilmeService filmeService) =>
{
    try
    {
        await filmeService.CreateFilmeAsync(filme);
        return Results.Created($"/filmes/{filme.Id}", filme);
    }
    catch (ArgumentException ex) { return Results.BadRequest(ex.Message); }
})
.WithDescription("POST para criar um filme")
.WithSummary("POST para criar um filme")
.Produces<Filme>(201)
.Produces(404);

filmesApi.MapPut("/filmes/{id}", async (string id, Filme filme, IFilmeService filmeService) =>
{
    if (filme.Id != null && filme.Id != id)
        return Results.BadRequest("O ID do filme no corpo da requisição não corresponde ao ID da URL");

    await filmeService.UpdateFilmeAsync(id, filme);
    return Results.NoContent();
})
.WithDescription("PUT para atualizar um filme utilizando seu ID")
.WithSummary("PUT para atualizar um filme")
.Produces<Filme>(204)
.Produces(404);

filmesApi.MapDelete("/filmes/{id}", async (string id, IFilmeService filmeService) =>
{
    try
    {
        await filmeService.DeleteFilmeAsync(id);
        return Results.NoContent();
    }
    catch (ArgumentException ex) { return Results.NotFound(ex.Message); }
})
.WithDescription("DELETE para deletar um filme utilizando seu ID")
.WithSummary("DELETE para deletar um filme")
.Produces(204)
.Produces(404);

//RESENHAS
var resenhasApi = app.MapGroup("/api")
    .WithTags("Resenhas");


resenhasApi.MapGet("/resenhas", async (IResenhaService resenhaService) =>
    Results.Ok(await resenhaService.GetAllResenhasAsync()))
    .WithDescription("GET para buscar todas as resenhas")
    .WithSummary("GET para buscar todas as resenhas")
    .Produces<List<Resenha>>(200)
    .Produces(404);

resenhasApi.MapGet("/resenhas/{id}", async (string id, IResenhaService resenhaService) =>
{
    var resenha = await resenhaService.GetResenhaByIdAsync(id);
    return resenha is not null ? Results.Ok(resenha) : Results.NotFound($"Resenha com ID '{id}' não encontrada");
})
.WithDescription("GET para buscar resenhas pelo ID")
.WithSummary("GET para buscar resenhas por ID")
    .Produces<Resenha>(200)
    .Produces(404);


resenhasApi.MapGet("/resenhas/filme/{filmeId}", async (string filmeId, IResenhaService resenhaService) =>
    Results.Ok(await resenhaService.GetResenhasByFilmeIdAsync(filmeId)))
    .WithDescription("GET para buscar resenhas pelo ID de filmes")
    .WithSummary("GET para buscar resenhas pelo id de filmes")
    .Produces<List<Resenha>>(200)
    .Produces(404);

resenhasApi.MapPost("/resenhas", async (Resenha resenha, IResenhaService resenhaService) =>
{
    try
    {
        await resenhaService.CreateResenhaAsync(resenha);
        return Results.Created($"/resenhas/{resenha.Id}", resenha);
    }
    catch (ArgumentException ex) { return Results.BadRequest(ex.Message); }
})
.WithDescription("POST para criar uma resenha")
.WithSummary("POST para criar uma resenha")
    .Produces<Resenha>(201)
    .Produces(404);

resenhasApi.MapPut("/resenhas/{id}", async (string id, Resenha resenha, IResenhaService resenhaService) =>
{
    if (resenha.Id != null && resenha.Id != id)
        return Results.BadRequest("O ID da resenha no corpo da requisição não corresponde ao ID da URL");

    try
    {
        await resenhaService.UpdateResenhaAsync(id, resenha);
        return Results.NoContent();
    }
    catch (ArgumentException ex) { return Results.NotFound(ex.Message); }
})
.WithDescription("PUT para atualizar uma resenha")
.WithSummary("PUT para atualizar uma resenha")
    .Produces<Resenha>(204)
    .Produces(404);

resenhasApi.MapDelete("/resenhas/{id}", async (string id, IResenhaService resenhaService) =>
{
    try
    {
        await resenhaService.DeleteResenhaAsync(id);
        return Results.NoContent();
    }
    catch (ArgumentException ex) { return Results.NotFound(ex.Message); }
})
.WithDescription("DELETE para deletar uma resenha")
.WithSummary("DELETE para deletar uma resenha")
    .Produces(204)
    .Produces(404);

//TESTES
var testeMongo = app.MapGroup("/api").WithTags("Teste MongoDB");

testeMongo.MapGet("/mongo-test", async (IMongoDatabase db) =>
{
    try
    {
        var collections = await (await db.ListCollectionNamesAsync()).ToListAsync();
        return Results.Ok(new
        {
            Message = "Conexão com o MongoDB estabelecida!",
            Database = db.DatabaseNamespace.DatabaseName,
            Collections = collections
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Falha ao conectar com o MongoDB: {ex.Message}");
    }
})
.WithDescription("TESTE para verificar se foi estabelecidade corretamente com o MongoDB")
.WithSummary("TESTE para verificar conexão com o banco");
app.Run();