using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using MovieRank.Contracts;
using MovieRank.Libs.Models;

namespace MovieRank.Libs.Repositories
{
    public interface IMovieRankRepository
    {
        Task<ScanResponse> GetAllItems();
        Task<GetItemResponse> GetMovie(int userId, string movieName);
        Task<QueryResponse> GetUsersRankedMoviesByMovieTitle(int userId, string movieName);
        Task AddMovie(int userId, MovieRankRequest movieRankRequest);
        Task UpdateMovie(int userId, MovieUpdateRequest updateRequest);
        Task<QueryResponse> GetMovieRank(string movieName);
        Task CreateDynamoDbTable(string tableName);
        Task DeleteDynamoDbTable(string tableName);
    }

    /*
    public interface IMovieRankRepository
    {
        Task<IEnumerable<Document>> GetAllItems();
        Task<Document> GetMovie(int userId, string movieName);
        Task<IEnumerable<Document>> GetUsersRankedMoviesByMovieTitle(int userId, string movieName);
        Task AddMovie(Document documentModel);
        Task UpdateMovie(Document documentModel);
        Task<IEnumerable<Document>> GetMovieRank(string movieName);
    }
    */

    /*
    public interface PersistanceIMovieRankRepository
    {
        Task<IEnumerable<MovieDb>> GetAllItems();
        Task<MovieDb> GetMovie(int userId, string movieName);
        Task<IEnumerable<MovieDb>> GetUsersRankedMoviesByMovieTitle(int userId, string movieName);
        Task AddMovie(MovieDb movieDb);
        Task UpdateMovie(MovieDb request);
        Task<IEnumerable<MovieDb>> GetMovieRank(string movieName);
    }
    */
}
