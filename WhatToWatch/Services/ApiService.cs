using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
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
        private readonly string apiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwNzJlYzkzNTJkNjA4MzM5YTU1NDNhY2M3ODM0YjRiYSIsInN1YiI6IjY2MzI1ZGY2YWY0MzI0MDEyYjUzYjE4NCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.tmk90JPOZ7RcseOtUQPFOeIqUr7acixGXhmF_pQQtvs";

        private async Task<T> GetAsync<T>(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
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
            var options = new RestClientOptions("https://api.themoviedb.org/3/movie/popular?language=hu&page=1");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MovieList result = JsonConvert.DeserializeObject<MovieList>(response.Content);
                return result;
            }
            else
            {
                return new MovieList();
            }
        }

        public async Task<MovieList> GetNowPlayingMoviesAsync()
        {
            var options = new RestClientOptions("https://api.themoviedb.org/3/movie/now_playing?language=hu&page=1");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MovieList result = JsonConvert.DeserializeObject<MovieList>(response.Content);
                return result;
            }
            else
            {
                return new MovieList();
            }
        }

        public async Task<MovieList> GetTopRatedMoviesAsync()
        {
            var options = new RestClientOptions("https://api.themoviedb.org/3/movie/top_rated?language=hu&page=1");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MovieList result = JsonConvert.DeserializeObject<MovieList>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<MovieList> GetUpcomingMoviesAsync()
        {
            var options = new RestClientOptions("https://api.themoviedb.org/3/movie/upcoming?language=hu&page=1");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MovieList result = JsonConvert.DeserializeObject<MovieList>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<Movie> GetMovieDetailsAsync(int id)
        {
            var options = new RestClientOptions($"https://api.themoviedb.org/3/movie/{id}?language=hu");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Movie result = JsonConvert.DeserializeObject<Movie>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<MovieCast> GetMovieCastAsync(int id)
        {
            var options = new RestClientOptions($"https://api.themoviedb.org/3/movie/{id}/credits?language=hu");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MovieCast result = JsonConvert.DeserializeObject<MovieCast>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<Actor> GetActorDetailsAsync(int id)
        {
            var options = new RestClientOptions($"https://api.themoviedb.org/3/person/{id}?language=hu");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Actor result = JsonConvert.DeserializeObject<Actor>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<ActorCast> GetActorCastAsync(int id)
        {
            var options = new RestClientOptions($"https://api.themoviedb.org/3/person/{id}/movie_credits?language=hu");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                ActorCast result = JsonConvert.DeserializeObject<ActorCast>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<MovieList> GetMovieSearchResultAsync(string queryString)
        {
            var options = new RestClientOptions($"https://api.themoviedb.org/3/search/movie?query={queryString}&include_adult=false&language=hu&page=1");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                MovieList result = JsonConvert.DeserializeObject<MovieList>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<MovieList> GetRelatedMoviesAsync(int movieId)
        {
            var options = new RestClientOptions($"https://api.themoviedb.org/3/movie/{movieId}/recommendations?language=hu&page=1");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            var response = await client.GetAsync(request);
            if(response.IsSuccessStatusCode)
            {
                MovieList result = JsonConvert.DeserializeObject<MovieList>(response.Content);
                await GetPostersForMovieListAsync(result);
                return result;
            }
            else
            {
                return null;
            }
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
