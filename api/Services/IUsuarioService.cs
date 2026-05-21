using FILMESRESENHA.Models;

namespace FILMESRESENHA.Services;

public interface IUsuarioService
{
    Task<Usuario?> GetUsuariosByIDAsync(string id);
    Task <Usuario?> GetUsuarioByEmailAsync(string email);
    Task CreateUsuariosAsync(Usuario usuario);
    Task UpdateUsuariosAsync(string id, Usuario usuario);
    Task DeleteUsuariosAsync(string id);
}