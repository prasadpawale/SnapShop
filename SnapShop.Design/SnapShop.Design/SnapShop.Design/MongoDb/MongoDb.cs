using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnapShop.Design.MongoDb
{
    public class MongoDb<T> : IRepository<T>
    {
        private MongoClient mongoClient;
        private IMongoDatabase mongoDb;

        public MongoDb()
        {
            CreateMongoClient();
        }

        /// <summary>
        /// Create Mongo database instance
        /// </summary>
        private void CreateMongoClient()
        {
            //string connectionStringItems = "mongodb://localhost/?safe=true";
            string connectionStringItems = "mongodb://thesnapshopapp:sz94BSTRMYIoNDhN4rsYCmgUZh55AsR1P9qAcfvow3tZ6OPLEqIAKid9hj2nAG6UJyX6INUqmBZC7zg9nG2wzA==@thesnapshopapp.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            string database = "SnapShop";
            mongoClient = new MongoClient(new MongoUrlBuilder(connectionStringItems) { MaxConnectionIdleTime = TimeSpan.FromMinutes(1) }.ToMongoUrl());
            if (mongoDb == null || mongoDb.DatabaseNamespace.DatabaseName != database)
                mongoDb = mongoClient.GetDatabase(database);
            
        }

        public void Add(T product, string collection)
        {
            mongoDb.GetCollection<T>(collection).InsertOne(product);
        }

        public List<T> GetByKeywords(List<string> keywords)
        {
            IAggregateFluent<T> query = GetFilter(keywords);
            List<T>  items = query.ToList() as List<T>;
            return items;
        }

        public List<T> GetAllProducts()
        {
            IAggregateFluent<T> query = mongoDb.GetCollection<T>("test").Aggregate();
            List<T> items = query.ToList() as List<T>;
            return items;
        }

        public IAggregateFluent<T> GetFilter(List<string> keywords)
        {
            IAggregateFluent<T> aggregateQuery = mongoDb.GetCollection<T>("test").Aggregate();
            FieldDefinition<T> groupUnwindVersions = "Keywords";
            aggregateQuery = aggregateQuery.Unwind<T>(groupUnwindVersions);

            //get keywords group collection
            var matchFilter = Builders<T>.Filter.In("Keywords", keywords);
            aggregateQuery = aggregateQuery.Match(matchFilter);

            var groupDefinition = new BsonDocument
            {
                 { "_id", "$_id" },
                 { "Name", new BsonDocument("$first","$Name")},
                 { "ParentId", new BsonDocument("$first","$ParentId")},
                 { "ImageData", new BsonDocument("$first","$ImageData")},
                 { "Price", new BsonDocument("$first","$Price")},
                 { "Category", new BsonDocument("$first","$Category")},
                 { "Description", new BsonDocument("$first","$Description")},   
                 //{ "Keywords", new BsonDocument("$push","$Keywords")}
            };
            aggregateQuery = aggregateQuery.Group<T>(groupDefinition);
            return aggregateQuery;
        }
    }
}