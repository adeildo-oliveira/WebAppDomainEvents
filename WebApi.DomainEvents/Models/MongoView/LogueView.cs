using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WebApi.DomainEvents.Models.MongoView
{
    [BsonIgnoreExtraElements]//ignora as property que não foram mapeadas no objeto
    public class LogueView
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
