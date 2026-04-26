using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ATIVIDADESEMESTRAL.Models;

public class Resenha
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required(ErrorMessage = "O ID do filme é obrigatório.")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string FilmeId { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome do autor é obrigatório.")]
    [StringLength(100, MinimumLength = 1)]
    public string AutorNome { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "A nota deve ser entre 1 e 5.")]
    public int Nota { get; set; }

    [Required(ErrorMessage = "O texto da resenha é obrigatório.")]
    [StringLength(2000, MinimumLength = 10)]
    public string Texto { get; set; } = string.Empty;

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}