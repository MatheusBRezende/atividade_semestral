using ATIVIDADESEMESTRAL.Models;
namespace ATIVIDADESEMESTRAL.Repositories;

public interface IFilmeRepository
{
    Task<List<Filme>> GetAllAsync();
    Task<Filme?> GetByIdAsync(string id);
    Task<List<Filme>> GetByGeneroAsync(string genero);

    Task CreateAsync(Filme filme);
    Task UpdateAsync(string id, Filme filme);
    Task DeleteAsync(string id);
    
}