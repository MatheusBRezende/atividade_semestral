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

    public async Task<Usuario?> GetUsuariosByIDAsync(string id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    public async Task<Usuario?> GetUsuariosByEmailAsync(string email)
    {
        return await _usuarioRepository.GetByEmailAsync(email);
    }

    public async Task CreateUsuariosAsync(Usuario usuario)
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

    public async Task UpdateUsuariosAsync(string id, Usuario usuario)
    {
        var usuarioexiste = await _usuarioRepository.GetByIdAsync(id);
        if (usuarioexiste == null)
        {
            throw new ArgumentException($"O usuario de ID '{id}' não existe.");
        }

        if (string.IsNullOrEmpty(usuario.Nome))
        {
            throw new ArgumentException("O nome do usuario é obrigatório");
        }

        if (string.IsNullOrEmpty(usuario.Email))
        {
            throw new ArgumentException("O email do usuario é obrigatório");
        }

        await _usuarioRepository.UpdateAsync(id , usuario);
    }

    public async Task DeleteUsuariosAsync(string id)
    {
        var usuarioexiste = await _usuarioRepository.GetByIdAsync(id);
        if (usuarioexiste == null)
        {
            throw new ArgumentException($"O usuario de ID '{id}' não existe.");
        }

        await _usuarioRepository.DeleteAsync(id);
    }
}