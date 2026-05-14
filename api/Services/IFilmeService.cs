using FILMESRESENHA.Models;

namespace FILMESRESENHA.Services;

public interface IFilmeService
{
    Task<List<Filme>> GetAllFilmesAsync();
    Task<Filme?> GetFilmeByIdAsync(string id);
    Task CreateFilmeAsync(Filme filme);
    Task UpdateFilmeAsync(string id, Filme filme);
    Task DeleteFilmeAsync(string id);
}