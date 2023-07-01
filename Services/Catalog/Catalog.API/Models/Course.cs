using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.API.Models;

internal class Course
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] //yollanan string'i object Id ye çevirecek.
    public string Id { get; set; }
    public string Name { get; set; }
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Price { get; set; }
    public string? Description { get; set; }
    [BsonRepresentation(BsonType.String)]
    public string UserId { get; set; }

    public string? Picture { get; set; }
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedAt { get; set; }
    public Feature Feature { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; }
    [BsonIgnore] //kendi içerisinde kullanıldığını belirtmek için 
    public Category Category { get; set; } //Mongodb tarafında ilgili collectionda bir karşılığı olmayacak.
}
