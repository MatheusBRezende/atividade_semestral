using MongoDB.Driver;
using FILMESRESENHA.Models;
namespace FILMESRESENHA.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IMongoCollection<Usuario> _collection;

    public UsuarioRepository(IMongoDatabase db)
    {
        _collection = db.GetCollection<Usuario>("usuarios");
    }

    public async Task<Usuario?> GetByIdAsync(string id) =>
        await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();

    public async Task<Usuario?> GetByEmailAsync(string email) =>
        await _collection.Find(u => u.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
    
    public async Task CreateAsync(Usuario usuario) =>
            await _collection.InsertOneAsync(usuario);

    public async Task UpdateAsync(string id, Usuario usuario) =>
        await _collection.ReplaceOneAsync(u => u.Id == id, usuario);

    public async Task DeleteAsync(string id) =>
        await _collection.DeleteOneAsync(u => u.Id == id);
}