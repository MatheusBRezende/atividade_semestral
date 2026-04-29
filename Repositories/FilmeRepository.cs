using MongoDB.Driver;
using ATIVIDADESEMESTRAL.Models;
namespace ATIVIDADESEMESTRAL.Repositories;
public class FilmeRepository : IFilmeRepository
{

  private readonly IMongoCollection<Filme> _collection;

  public FilmeRepository(IMongoDatabase db)
  {
    _collection = db.GetCollection<Filme>("filmes");
  }

    public async Task<List<Filme>> GetAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<Filme?> GetByIdAsync(string id) =>
        await _collection.Find(f => f.Id == id).FirstOrDefaultAsync();

    public async Task<List<Filme>> GetByGeneroAsync(string genero) =>
        await _collection.Find(f => f.Genero.ToLower() == genero.ToLower())
        .ToListAsync();

    public async Task CreateAsync(Filme filme) =>
        await _collection.InsertOneAsync(filme);

    public async Task UpdateAsync(string id, Filme filme) =>
        await _collection.ReplaceOneAsync(f => f.Id == id, filme);
    public async Task DeleteAsync(string id) =>
        await _collection.DeleteOneAsync(f => f.Id == id);
}