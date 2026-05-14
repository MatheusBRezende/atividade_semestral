using FILMESRESENHA.Models;
using FILMESRESENHA.Repositories;

namespace FILMESRESENHA.Services;

public class FilmeService : IFilmeService
{
    private readonly IFilmeRepository _filmeRepository;

    public FilmeService(IFilmeRepository filmeRepository)
    {
        _filmeRepository = filmeRepository;
    }

    public async Task<List<Filme>> GetAllFilmesAsync()
    {
        return await _filmeRepository.GetAllAsync();
    }

    public async Task<Filme?> GetFilmeByIdAsync(string id)
    {
        return await _filmeRepository.GetByIdAsync(id);
    }

    public async Task CreateFilmeAsync(Filme filme)
    {
        if (string.IsNullOrEmpty(filme.Titulo))
        {
            throw new ArgumentException("O título do filme é obrigatório.");
        }

        if (filme.Ano < 1888 || filme.Ano > 2030)
        {
            throw new ArgumentException("O ano de lançamento deve estar entre 1888 e 2030.");
        }

        if (string.IsNullOrEmpty(filme.Genero))
        {
            throw new ArgumentException("O gênero do filme é obrigatório.");
        }

        await _filmeRepository.CreateAsync(filme);
    }

    public async Task UpdateFilmeAsync(string id, Filme filme)
    {
        var filmeExistente = await _filmeRepository.GetByIdAsync(id);
        if (filmeExistente == null)
        {
            throw new ArgumentException($"O filme com ID '{id}' não foi encontrado.");
        }

        if (string.IsNullOrEmpty(filme.Titulo))
        {
            throw new ArgumentException("O título do filme é obrigatório.");
        }

        if (filme.Ano < 1888 || filme.Ano > 2030)
        {
            throw new ArgumentException("O ano de lançamento deve estar entre 1888 e 2030.");
        }

        if (string.IsNullOrEmpty(filme.Genero))
        {
            throw new ArgumentException("O gênero do filme é obrigatório.");
        }

        await _filmeRepository.UpdateAsync(id, filme);
    }

    public async Task DeleteFilmeAsync(string id)
    {
        var filmeExistente = await _filmeRepository.GetByIdAsync(id);
        if (filmeExistente == null)
        {
            throw new ArgumentException($"O filme com ID '{id}' não foi encontrado.");
        }

        await _filmeRepository.DeleteAsync(id);
    }
}