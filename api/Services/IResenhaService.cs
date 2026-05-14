using FILMESRESENHA.Models;

namespace FILMESRESENHA.Services;

public interface IResenhaService
{
    Task<List<Resenha>> GetAllResenhasAsync();
    Task<Resenha?> GetResenhaByIdAsync(string id);
    Task<List<Resenha>> GetResenhasByFilmeIdAsync(string filmeId);
    Task CreateResenhaAsync(Resenha resenha);
    Task UpdateResenhaAsync(string id, Resenha resenha);
    Task DeleteResenhaAsync(string id);
}