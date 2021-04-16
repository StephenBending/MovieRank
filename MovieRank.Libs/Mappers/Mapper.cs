using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using MovieRank.Contracts;
using MovieRank.Libs.Models;

namespace MovieRank.Libs.Mappers
{
    public class Mapper : IMapper
    {
        public IEnumerable<MovieResponse> ToMovieContract(ScanResponse response)
        {
            return response.Items.Select(ToMovieContract);
        }

        public IEnumerable<MovieResponse> ToMovieContract(QueryResponse response)
        {
            return response.Items.Select(ToMovieContract);
        }

        private MovieResponse ToMovieContract(Dictionary<string, AttributeValue> item)
        {
            return new MovieResponse
            {
                MovieName = item["MovieName"].S,
                Description = item["Description"].S,
                Actors = item["Actors"].SS,
                Ranking = Convert.ToInt32(item["Ranking"].N),
                TimeRanked = item["RankedDateTime"].S
            };
        }

        public MovieResponse ToMovieContract(GetItemResponse response)
        {
            return new MovieResponse
            {
                MovieName = response.Item["MovieName"].S,
                Description = response.Item["Description"].S,
                Actors = response.Item["Actors"].SS,
                Ranking = Convert.ToInt32(response.Item["Ranking"].N),
                TimeRanked = response.Item["RankedDateTime"].S
            };
        }
    }

    /*
    public class Mapper : IMapper
    {
        public IEnumerable<MovieResponse> ToMovieContract(IEnumerable<Document> items)
        {
            return items.Select(ToMovieContract);
        }

        public MovieResponse ToMovieContract(Document item)
        {
            return new MovieResponse
            {
                MovieName = item["MovieName"],
                Description = item["Description"],
                Actors = item["Actors"].AsListOfString(),
                Ranking = Convert.ToInt32(item["Ranking"]),
                TimeRanked = item["RankedDateTime"]
            };
        }

        public Document ToDocumentModel(int userId, MovieRankRequest addRequest)
        {
            return new Document
            {
                ["UserId"] = userId,
                ["MovieName"] = addRequest.MovieName,
                ["Description"] = addRequest.Description,
                ["Actors"] = addRequest.Actors,
                ["RankedDateTime"] = DateTime.UtcNow.ToString(),
                ["Ranking"] = addRequest.Ranking
            };
        }

        public Document ToDocumentModel(int userId, MovieResponse movieResponse, MovieUpdateRequest movieUpdateRequest)
        {
            return new Document
            {
                ["UserId"] = userId,
                ["MovieName"] = movieResponse.MovieName,
                ["Description"] = movieResponse.Description,
                ["Actors"] = movieResponse.Actors,
                ["RankedDateTime"] = DateTime.UtcNow.ToString(),
                ["Ranking"] = movieUpdateRequest.Ranking
            }; 
        }
    }
    */

    /*
    public class PersistanceMapper //: IMapper
    {
        public IEnumerable<MovieResponse> ToMovieContract(IEnumerable<MovieDb> items)
        {
            return items.Select(ToMovieContract);
        }

        public MovieResponse ToMovieContract(MovieDb movie)
        {
            return new MovieResponse
            {
                MovieName = movie.MovieName,
                Description = movie.Description,
                Actors = movie.Actors,
                Ranking = movie.Ranking,
                TimeRanked = movie.RankedDateTime
            };
        }

        public MovieDb ToMovieDbModel(int userId, MovieRankRequest movieRankRequest)
        {
            return new MovieDb
            {
                UserId = userId,
                MovieName = movieRankRequest.MovieName,
                Description = movieRankRequest.Description,
                Actors = movieRankRequest.Actors,
                Ranking = movieRankRequest.Ranking,
                RankedDateTime = DateTime.UtcNow.ToString()
            };
        }

        public MovieDb ToMovieDbModel(int userId, MovieDb movieDbRequest, MovieUpdateRequest movieUpdateRequest)
        {
            return new MovieDb
            {
                UserId = movieDbRequest.UserId,
                MovieName = movieDbRequest.MovieName,
                Description = movieDbRequest.Description,
                Actors = movieDbRequest.Actors,
                Ranking = movieUpdateRequest.Ranking,
                RankedDateTime = DateTime.UtcNow.ToString()
            };
        }

        public MovieRankResponse ToMovieRankResponse(string movieName, double overallMovieRanking)
        {
            return new MovieRankResponse
            {
                MovieName = movieName,
                OverallRanking = overallMovieRanking
            };
        }
    }
    */
}
