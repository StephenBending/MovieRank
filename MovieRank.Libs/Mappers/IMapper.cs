using System.Collections.Generic;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using MovieRank.Contracts;
using MovieRank.Libs.Models;

namespace MovieRank.Libs.Mappers
{
    public interface IMapper
    {
        IEnumerable<MovieResponse> ToMovieContract(ScanResponse response);
        IEnumerable<MovieResponse> ToMovieContract(QueryResponse response);
        MovieResponse ToMovieContract(GetItemResponse response);
    }

    /*
    public interface IMapper
    {
        IEnumerable<MovieResponse> ToMovieContract(IEnumerable<Document> items);
        MovieResponse ToMovieContract(Document item);
        Document ToDocumentModel(int userId, MovieRankRequest addRequest);
        Document ToDocumentModel(int userId, MovieResponse movieResponse, MovieUpdateRequest movieUpdateRequest);

    }
    */

    /*
    public interface PersistanceIMapper
    {
        IEnumerable<MovieResponse> ToMovieContract(IEnumerable<MovieDb> items);
        MovieResponse ToMovieContract(MovieDb movie);
        MovieDb ToMovieDbModel(int userId, MovieRankRequest movieRankRequest);
        MovieDb ToMovieDbModel(int userId, MovieDb movieDbRequest, MovieUpdateRequest movieUpdateRequest);
        MovieRankResponse ToMovieRankResponse(string movieName, double overallMovieRanking);
    }
    */
}
