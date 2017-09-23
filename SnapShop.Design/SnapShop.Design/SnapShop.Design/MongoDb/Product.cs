using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnapShop.Design.MongoDb
{
    public class Product
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string ParentId { get; set; }
        public string ImageData { get; set; }
        public List<string> Keywords { get; set; }
    }

    public class UnwoundProduct
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string ParentId { get; set; }
        public string ImageData { get; set; }
        public string Keywords { get; set; }
    }
}