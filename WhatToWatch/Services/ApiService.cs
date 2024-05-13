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

        public async Task<BitmapImage> GetPosterAsync(string path)
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

        private async Task GetPostersForMovieListAsync(MovieList movieList)
        {
            foreach (var movie in movieList.results)
            {
                var image = await GetPosterAsync(movie.poster_path);
                if (image != null)
                {
                    movie.poster = image;
                }
            }
        }

        private async Task GetPosterForSeriesListAsync(SeriesList seriesList)
        {
            foreach (var series in seriesList.results)
            {
                var image = await GetPosterAsync(series.poster_path);
                if (image != null)
                {
                    series.poster = image;
                }
            }
        }

        private async Task<MovieList> GetMovieListWithPosterAsync(string path)
        {
            var resultList = await GetAsync<MovieList>(path);
            if (resultList != default)
            {
                await GetPostersForMovieListAsync(resultList);
            }
            return resultList;
        }

        private async Task<SeriesList> GetSeriesListWithPosterAsync(string path)
        {
            var resultList = await GetAsync<SeriesList>(path);
            if(resultList != default)
            {
                await GetPosterForSeriesListAsync(resultList);
            }
            return resultList;
        }

        public async Task<MovieList> GetPopularMoviesAsync()
        {

            return await GetMovieListWithPosterAsync("movie/popular?language=hu&page=1");
        }

        public async Task<MovieList> GetNowPlayingMoviesAsync()
        {
            return await GetMovieListWithPosterAsync("movie/now_playing?language=hu&page=1");
        }

        public async Task<MovieList> GetTopRatedMoviesAsync()
        {
            return await GetMovieListWithPosterAsync("movie/top_rated?language=hu&page=1");
        }

        public async Task<MovieList> GetUpcomingMoviesAsync()
        {
            return await GetMovieListWithPosterAsync("movie/upcoming?language=hu&page=1");
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
            return await GetMovieListWithPosterAsync($"search/movie?query={queryString}&include_adult=false&language=hu&page=1");
        }

        public async Task<MovieList> GetRelatedMoviesAsync(int movieId)
        {
            return await GetMovieListWithPosterAsync($"movie/{movieId}/recommendations?language=hu&page=1");
        }

        public async Task<SeriesList> GetPopularSeriesAsync()
        {
            return await GetSeriesListWithPosterAsync("tv/popular?language=hu&page=1");
        }

        public async Task<SeriesList> GetOnTheAirSeriesAsync()
        {
            return await GetSeriesListWithPosterAsync("tv/on_the_air?language=hu&page=1");
        }

        public async Task<SeriesList> GetAiringTodaySeriesAsync()
        {
            return await GetSeriesListWithPosterAsync("tv/airing_today?language=hu&page=1");
        }

        public async Task<SeriesList> GetTopRatedSeriesAsync()
        {
            return await GetSeriesListWithPosterAsync("tv/top_rated?language=hu&page=1");
        }

        public async Task<SeriesList> GetSeriesSearchResultAsync(string queryString)
        {
            return await GetSeriesListWithPosterAsync($"search/tv?query={queryString}&include_adult=false&language=hu&page=1");
        }

        public async Task<Series> GetSeriesDetailsAsync(int seriesId)
        {
            var series = await GetAsync<Series>($"tv/{seriesId}?language=hu");
            foreach(Season season in series.seasons)
            {
                var seasonDetails = await GetAsync<Season>($"tv/{series.id}/season/{season.season_number}?language=hu");
                season.episodes = new List<Episode>();
                foreach(var episode in seasonDetails.episodes)
                {
                    season.episodes.Add(episode);
                }
            }
            return series;
        }
    }
}
