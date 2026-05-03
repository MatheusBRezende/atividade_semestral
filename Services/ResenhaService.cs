using FILMESRESENHA.Models;
using FILMESRESENHA.Repositories;

namespace FILMESRESENHA.Services;

public class ResenhaService : IResenhaService
{
    private readonly IResenhaRepository _resenhaRepository;
    private readonly IFilmeRepository _filmeRepository;

    public ResenhaService(IResenhaRepository resenhaRepository, IFilmeRepository filmeRepository)
    {
        _resenhaRepository = resenhaRepository;
        _filmeRepository = filmeRepository;
    }

    public async Task<List<Resenha>> GetAllResenhasAsync()
    {
        return await _resenhaRepository.GetAllAsync();
    }

    public async Task<Resenha?> GetResenhaByIdAsync(string id)
    {
        return await _resenhaRepository.GetByIdAsync(id);
    }

    public async Task<List<Resenha>> GetResenhasByFilmeIdAsync(string filmeId)
    {
        return await _resenhaRepository.GetByFilmeIdAsync(filmeId);
    }

    public async Task CreateResenhaAsync(Resenha resenha)
    {
        if (resenha.Nota < 1 || resenha.Nota > 5)
        {
            throw new ArgumentException("A nota deve estar entre 1 e 5.");
        }

        if (string.IsNullOrEmpty(resenha.Texto) || resenha.Texto.Length < 10)
        {
            throw new ArgumentException("O texto da resenha deve ter pelo menos 10 caracteres.");
        }

        if (string.IsNullOrEmpty(resenha.AutorNome))
        {
            throw new ArgumentException("O nome do autor é obrigatório.");
        }

        var filmeExistente = await _filmeRepository.GetByIdAsync(resenha.FilmeId);
        if (filmeExistente == null)
        {
            throw new ArgumentException($"O filme com ID '{resenha.FilmeId}' não foi encontrado.");
        }

        await _resenhaRepository.CreateAsync(resenha);
    }

    public async Task UpdateResenhaAsync(string id, Resenha resenha)
    {
        var resenhaExistente = await _resenhaRepository.GetByIdAsync(id);
        if (resenhaExistente == null)
            throw new ArgumentException($"A resenha com ID '{id}' não foi encontrada.");

        if (resenha.Nota < 1 || resenha.Nota > 5)
            throw new ArgumentException("A nota deve estar entre 1 e 5.");

        if (string.IsNullOrEmpty(resenha.Texto) || resenha.Texto.Length < 10)
            throw new ArgumentException("O texto da resenha deve ter pelo menos 10 caracteres.");

        if (string.IsNullOrEmpty(resenha.AutorNome))
            throw new ArgumentException("O nome do autor é obrigatório.");

        await _resenhaRepository.UpdateAsync(id, resenha);
    }

    public async Task DeleteResenhaAsync(string id)
    {
        var resenhaExistente = await _resenhaRepository.GetByIdAsync(id);
        if (resenhaExistente == null)
            throw new ArgumentException($"A resenha com ID '{id}' não foi encontrada.");

        await _resenhaRepository.DeleteAsync(id);
    }
}