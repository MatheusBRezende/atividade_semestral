namespace FILMESRESENHA.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Usuario
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id {get;set;}
    public string Nome {get;set;} = string.Empty;
    public string Email {get;set;} = string.Empty;
    public string SenhaHash {get;set;} = string.Empty;
    public string TokenRecuperacao {get;set;} = string.Empty;
    public List<string> Favoritos { get; set; } = [];
    public List<string> AssisitrDepois { get; set; } = [];
    public string Role {get;set;} = "User";
}