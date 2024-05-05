using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WhatToWatch.Models;

namespace WhatToWatch.Services
{
    internal class ApiService
    {
        private readonly Uri serverUrl = new Uri("https://api.themoviedb.org/3");

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

        public async Task<MovieList> GetPopularMovieListAsync()
        {
            var options = new RestClientOptions("https://api.themoviedb.org/3/movie/popular?language=en-US&page=1");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwNzJlYzkzNTJkNjA4MzM5YTU1NDNhY2M3ODM0YjRiYSIsInN1YiI6IjY2MzI1ZGY2YWY0MzI0MDEyYjUzYjE4NCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.tmk90JPOZ7RcseOtUQPFOeIqUr7acixGXhmF_pQQtvs");
            var response = await client.GetAsync(request);
            MovieList result = JsonConvert.DeserializeObject<MovieList>(response.Content);
            return result;
            //return await GetAsync<MovieList>(new Uri(serverUrl, "movie/popular"));
        }
    }
}
