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
            using(var client = new HttpClient())
            {   
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }

        private async Task<HttpResponseMessage> GetResponseAsync(Uri uri)
        {
            using(var client = new HttpClient())
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
            
            //return await GetAsync<MovieList>(new Uri(serverUrl, "movie/popular"));
        }
    }
}
