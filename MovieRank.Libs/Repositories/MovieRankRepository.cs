using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using MovieRank.Contracts;
using MovieRank.Libs.Models;

namespace MovieRank.Libs.Repositories
{
    public class MovieRankRepository : IMovieRankRepository
    {
        private const string TableName = "MovieRank";
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public MovieRankRepository(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<ScanResponse> GetAllItems()
        {
            var scanRequest = new ScanRequest(TableName);

            return await _dynamoDbClient.ScanAsync(scanRequest);
        }

        public async Task<GetItemResponse> GetMovie(int userId, string movieName)
        {
            var request = new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"UserId", new AttributeValue { N = userId.ToString()} },
                    {"MovieName", new AttributeValue {S = movieName} }
                }
            };

            return await _dynamoDbClient.GetItemAsync(request);
        }

        public async Task<QueryResponse> GetUsersRankedMoviesByMovieTitle(int userId, string movieName)
        {
            var request = new QueryRequest
            {
                TableName = TableName,
                KeyConditionExpression = "UserId = :userId and begins_with (MovieName, :movieName)",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":userId", new AttributeValue {N = userId.ToString()} },
                    {":movieName", new AttributeValue {S = movieName} }
                }
            };

            return await _dynamoDbClient.QueryAsync(request);
        }

        public async Task AddMovie(int userId, MovieRankRequest movieRankRequest)
        {
            var request = new PutItemRequest
            {
                TableName = TableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "UserId", new AttributeValue { N = userId.ToString() } },
                    { "MovieName", new AttributeValue { S = movieRankRequest.MovieName } },
                    { "Description", new AttributeValue { S = movieRankRequest.Description } },
                    { "Actors", new AttributeValue { SS = movieRankRequest.Actors } },
                    { "Ranking", new AttributeValue { N = movieRankRequest.Ranking.ToString() } },
                    { "RankedDateTime", new AttributeValue { S = DateTime.UtcNow.ToString() } }
                }
            };

            await _dynamoDbClient.PutItemAsync(request);
        }

        public async Task UpdateMovie(int userId, MovieUpdateRequest updateRequest)
        {
            var request = new UpdateItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"UserId", new AttributeValue { N = userId.ToString()} },
                    {"MovieName", new AttributeValue { S = updateRequest.MovieName} }
                },
                AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
                {
                    {"Ranking", new AttributeValueUpdate
                    {
                        Action = AttributeAction.PUT,
                        Value = new AttributeValue {N = updateRequest.Ranking.ToString()}
                    }
                    },
                    {"RankedDateTime", new AttributeValueUpdate
                    {
                        Action = AttributeAction.PUT,
                        Value = new AttributeValue {S = DateTime.UtcNow.ToString()}
                    }
                    }
                }
            };

            await _dynamoDbClient.UpdateItemAsync(request);
        }

        public async Task<QueryResponse> GetMovieRank(string movieName)
        {
            var request = new QueryRequest
            {
                TableName = TableName,
                IndexName = "MovieName-index",
                KeyConditionExpression = "MovieName = :movieName",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":movieName", new AttributeValue { S = movieName } }
                }
            };

            return await _dynamoDbClient.QueryAsync(request);
        }

        public async Task CreateDynamoDbTable(string tableName)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                }
            };

            await _dynamoDbClient.CreateTableAsync(request);
        }

        public async Task DeleteDynamoDbTable(string tableName)
        {
            var request = new DeleteTableRequest
            {
                TableName = tableName
            };

            await _dynamoDbClient.DeleteTableAsync(request);
        }
    }

    /*
    public class MovieRankRepository : IMovieRankRepository
    {
        private const string TableName = "MovieRank";
        private readonly Table _table;

        public MovieRankRepository(IAmazonDynamoDB dynamoDbClient)
        {
            _table = Table.LoadTable(dynamoDbClient, TableName);
        }

        public async Task<IEnumerable<Document>> GetAllItems()
        {
            var config = new ScanOperationConfig();

            return await _table.Scan(config).GetRemainingAsync();
        }

        public async Task<Document> GetMovie(int userId, string movieName)
        {
            return await _table.GetItemAsync(userId, movieName);
        }

        public async Task<IEnumerable<Document>> GetUsersRankedMoviesByMovieTitle(int userId, string movieName)
        {
            var filter = new QueryFilter("UserId", QueryOperator.Equal, userId);
            filter.AddCondition("MovieName", QueryOperator.BeginsWith, movieName);

            return await _table.Query(filter).GetRemainingAsync();
        }

        public async Task AddMovie(Document documentModel)
        {
            await _table.PutItemAsync(documentModel);
        }

        public async Task UpdateMovie(Document documentModel)
        {
            await _table.UpdateItemAsync(documentModel);
        }

        public async Task<IEnumerable<Document>> GetMovieRank(string movieName)
        {
            var filter = new QueryFilter("MovieName", QueryOperator.Equal, movieName);

            var config = new QueryOperationConfig()
            {
                IndexName = "MovieName-index",
                Filter = filter
            };

            return await _table.Query(config).GetRemainingAsync();
        }
    }
    */

    /*
    public class PersitanceMovieRankRepository //: IMovieRankRepository
    {
        private readonly DynamoDBContext _context;

        public PersitanceMovieRankRepository(IAmazonDynamoDB dynamicDbClient)
        {
            _context = new DynamoDBContext(dynamicDbClient);
        }

        public async Task<IEnumerable<MovieDb>> GetAllItems()
        {
            return await _context.ScanAsync<MovieDb>(new List<ScanCondition>()).GetRemainingAsync();
        }

        public async Task<MovieDb> GetMovie(int userId, string movieName)
        {
            return await _context.LoadAsync<MovieDb>(userId, movieName);
        }

        public async Task<IEnumerable<MovieDb>> GetUsersRankedMoviesByMovieTitle(int userId, string movieName)
        {
            var config = new DynamoDBOperationConfig
            {
                QueryFilter = new List<ScanCondition>
                {
                    new ScanCondition("MovieName", ScanOperator.BeginsWith, movieName)
                }
            };

            return await _context.QueryAsync<MovieDb>(userId, config).GetRemainingAsync();
        }

        public async Task AddMovie(MovieDb movieDb)
        {
            await _context.SaveAsync(movieDb);
        }

        public async Task UpdateMovie(MovieDb request)
        {
            await _context.SaveAsync(request);
        }

        public async Task<IEnumerable<MovieDb>> GetMovieRank(string movieName)
        {
            var config = new DynamoDBOperationConfig
            {
                IndexName = "MovieName-index"
            };

            return await _context.QueryAsync<MovieDb>(movieName, config).GetRemainingAsync();
        }
    }
    */
}
