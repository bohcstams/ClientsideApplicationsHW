using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using WhatToWatch.Models;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace WhatToWatch.Services
{
    internal class ApiService
    {
        private readonly Uri serverUrl = new Uri("https://api.themoviedb.org/3");
        private readonly Uri posterUrl = new Uri("https://image.tmdb.org/t/p/w500");
        private readonly string apiKey = "";

        public ApiService(string ApiKeyPath) {
            try
            {
                using(StreamReader sr = new StreamReader(ApiKeyPath))
                {
                    apiKey = sr.ReadToEnd();
                    Debug.WriteLine($"String read: {apiKey}");

                }
            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task<T> GetAsync<T>(string path)
        {
            var options = new RestClientOptions($"https://api.themoviedb.org/3/{path}");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            else 
            {
                return default;
            }
        }

        private async Task<HttpResponseMessage> GetResponseAsync(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                return response;
            }
        }

        public async Task<BitmapImage> GetMoviePosterAsync(string path)
        {
            var response = await GetResponseAsync(new Uri($"{posterUrl.OriginalString}/{path}"));
            if (response.IsSuccessStatusCode)
            {
                // Read the content as a byte array
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                // Create a BitmapImage from the byte array
                BitmapImage bitmapImage = new BitmapImage();
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    await stream.WriteAsync(imageBytes.AsBuffer());
                    stream.Seek(0);
                    await bitmapImage.SetSourceAsync(stream);
                }

                return bitmapImage;
            }
            else
            {
                return null;
            }
        }

        public async Task<MovieList> GetPopularMovieListAsync()
        {
            var popularList = await GetAsync<MovieList>("movie/popular?language=hu&page=1");
            if(popularList != default)
            {
                await GetPostersForMovieListAsync(popularList);
            }
            return popularList;
        }

        public async Task<MovieList> GetNowPlayingMoviesAsync()
        {
            var nowPlayingList = await GetAsync<MovieList>("movie/now_playing?language=hu&page=1");
            if (nowPlayingList != default)
            {
                await GetPostersForMovieListAsync(nowPlayingList);
            }
            return nowPlayingList;
        }

        public async Task<MovieList> GetTopRatedMoviesAsync()
        {
            var topRatedList = await GetAsync<MovieList>("movie/top_rated?language=hu&page=1");
            if (topRatedList != default)
            {
                await GetPostersForMovieListAsync(topRatedList);
            }
            return topRatedList;
        }

        public async Task<MovieList> GetUpcomingMoviesAsync()
        {
            var upcomingList = await GetAsync<MovieList>("movie/upcoming?language=hu&page=1");
            if (upcomingList != default)
            {
                await GetPostersForMovieListAsync(upcomingList);
            }
            return upcomingList;
        }

        public async Task<Movie> GetMovieDetailsAsync(int id)
        {
            return await GetAsync<Movie>($"movie/{id}?language=hu");
        }

        public async Task<MovieCast> GetMovieCastAsync(int id)
        {
            return await GetAsync<MovieCast>($"movie/{id}/credits?language=hu");
        }

        public async Task<Actor> GetActorDetailsAsync(int id)
        {
            return await GetAsync<Actor>($"person/{id}?language=hu");
        }

        public async Task<ActorCast> GetActorCastAsync(int id)
        {
            return await GetAsync<ActorCast>($"person/{id}/movie_credits?language=hu");
        }

        public async Task<MovieList> GetMovieSearchResultAsync(string queryString)
        {
            var searchResult =  await GetAsync<MovieList>($"search/movie?query={queryString}&include_adult=false&language=hu&page=1");
            if (searchResult != default)
            {
                await GetPostersForMovieListAsync(searchResult);
            }
            return searchResult;
        }

        public async Task<MovieList> GetRelatedMoviesAsync(int movieId)
        {
            var relatedList = await GetAsync<MovieList>($"movie/{movieId}/recommendations?language=hu&page=1");
            if (relatedList != default)
            {
                await GetPostersForMovieListAsync(relatedList);
            }
            return relatedList;
        }

        private async Task GetPostersForMovieListAsync(MovieList movieList)
        {
            foreach (var movie in movieList.results)
            {
                var image = await GetMoviePosterAsync(movie.poster_path);
                if (image != null)
                {
                    movie.poster = image;
                }
            }
        }
    }
}
