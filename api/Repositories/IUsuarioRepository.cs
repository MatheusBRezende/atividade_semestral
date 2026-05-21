using FILMESRESENHA.Models;

namespace FILMESRESENHA.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(string id);
    Task<Usuario?> GetByEmailAsync(string email);

    Task CreateAsync(Usuario usuario);
    Task UpdateAsync(string id, Usuario usuario);
    Task DeleteAsync(string id);
}