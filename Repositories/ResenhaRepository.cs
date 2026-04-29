using MongoDB.Driver;
using ATIVIDADESEMESTRAL.Models;
namespace ATIVIDADESEMESTRAL.Repositories;

public class ResenhaRepository : IResenhaRepository
{
    private readonly IMongoCollection<Resenha> _collection;

    public ResenhaRepository(IMongoDatabase db)
    {
        _collection = db.GetCollection<Resenha>("resenhas");
    }

    public async Task<List<Resenha>> GetAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<Resenha?> GetByIdAsync(string id) =>
        await _collection.Find(r => r.Id == id).FirstOrDefaultAsync();

    public async Task<List<Resenha>> GetByFilmeIdAsync(string filmeId) =>
        await _collection
            .Find(r => r.FilmeId == filmeId)
            .SortByDescending(r => r.DataCriacao)
            .ToListAsync();
    public async Task CreateAsync(Resenha resenha) =>
        await _collection.InsertOneAsync(resenha);

    public async Task UpdateAsync(string id, Resenha resenha) =>
        await _collection.ReplaceOneAsync(r => r.Id == id, resenha);

    public async Task DeleteAsync(string id) =>
        await _collection.DeleteOneAsync(r => r.Id == id);
}