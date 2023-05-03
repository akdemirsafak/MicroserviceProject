using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Models;

internal class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] //yollanan string'i object Id ye çevirecek.
    public string Id { get; set; }
    public string Name { get; set; }
}
