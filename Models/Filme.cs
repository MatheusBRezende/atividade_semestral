using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ATIVIDADESEMESTRAL.Models;

public class Filme
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(300, MinimumLength = 1)]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O diretor é obrigatório.")]
    [StringLength(100)]
    public string Diretor { get; set; } = string.Empty;

    [Required(ErrorMessage = "A sinopse é obrigatória.")]
    [StringLength(1000, MinimumLength = 1)]
    public string Sinopse { get; set; } = string.Empty;

    [Range(1888, 2030, ErrorMessage = "Ano de lançamento inválido.")]
    public int Ano { get; set; }

    [Required(ErrorMessage = "O gênero é obrigatório.")]
    [StringLength(100)]
    public string Genero { get; set; } = string.Empty;

    public bool Lancado { get; set; } = true;
}