using ATIVIDADESEMESTRAL.Models;
namespace ATIVIDADESEMESTRAL.Repositories;

public interface IResenhaRepository{

Task<List<Resenha>> GetAllAsync();
Task<Resenha?> GetByIdAsync(string id);
Task<List<Resenha>> GetByFilmeIdAsync(string filmeId);
Task CreateAsync(Resenha resenha);
Task UpdateAsync(string id, Resenha resenha);
Task DeleteAsync(string id);

}