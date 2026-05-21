using FILMESRESENHA.Models;
using FILMESRESENHA.Repositories;

namespace FILMESRESENHA.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(string id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
    {
        return await _usuarioRepository.GetByEmailAsync(email);
    }

       public async Task CreateUsuarioAsync(Usuario usuario)
    {
        if (string.IsNullOrEmpty(usuario.Nome))
        {
            throw new ArgumentException("O nome é obrigátorio");
        }

        if (string.IsNullOrEmpty(usuario.Email))
        {
            throw new ArgumentException("O email é obrigátorio");
        }

        await _usuarioRepository.CreateAsync(usuario);
    }
}