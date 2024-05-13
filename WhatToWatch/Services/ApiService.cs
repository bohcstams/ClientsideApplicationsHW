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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace WhatToWatch.Services
{
    /// <summary>
    /// A hálózati hívások lebonyoljtásáért felelős osztály
    /// </summary>
    internal class ApiService
    {
        /// <summary>
        /// A szerver azonosjtója
        /// </summary>
        private readonly Uri serverUrl = new Uri("https://api.themoviedb.org/3");
        /// <summary>
        /// A poszterek elérésének url azonosítója
        /// </summary>
        private readonly Uri posterUrl = new Uri("https://image.tmdb.org/t/p/w500");
        /// <summary>
        /// A szervehez tartozó api kulcs, amit a program fájlból olvas bee
        /// </summary>
        private readonly string apiKey = "";

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="ApiKeyPath">a fájlnak az elérési útvonala, ami az api kulcsot tartalmazza</param>
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

        /// <summary>
        /// Letölti a szerverről az útvonalon szereplő tartalmatd
        /// </summary>
        /// <typeparam name="T">Annak az objektumnak a típusa, amit a válasznak tartalmaznia kell</typeparam>
        /// <param name="path">A szerveren belüli útvonal</param>
        /// <returns>A paraméterben átadott típusú objektum az eredménnyel</returns>
        private async Task<T> GetAsync<T>(string path)
        {
            var options = new RestClientOptions($"{serverUrl.ToString()}/{path}");
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

        /// <summary>
        /// Elküld egy REST API kérést a megadott útvonalra
        /// </summary>
        /// <param name="uri">A szerveren belüli azonosító</param>
        /// <returns>Http üzenet, amely tartalmazza az eredményt</returns>
        private async Task<HttpResponseMessage> GetResponseAsync(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                return response;
            }
        }

        /// <summary>
        /// Letölti az útvonalon szereplő képet
        /// </summary>
        /// <param name="path">A kép útvonala</param>
        /// <returns>A letöltött kép</returns>
        public async Task<BitmapImage> GetPosterAsync(string path)
        {
            var response = await GetResponseAsync(new Uri($"{posterUrl.OriginalString}/{path}"));
            if (response.IsSuccessStatusCode)
            {
                // Read the content as a byte array
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                BitmapImage bitmapImage = new BitmapImage();
                if (imageBytes == null)
                {
                    try
                    {
                        bitmapImage.UriSource = new System.Uri("/Assets/movie-poster-placeholder.png");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                {
                    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                    {
                        await stream.WriteAsync(imageBytes.AsBuffer());
                        stream.Seek(0);
                        await bitmapImage.SetSourceAsync(stream);
                    }
                }
                //bitmapImage.UriSource = new System.Uri("ms-appx:///Assets/movie-poster-placeholder.png");
                return bitmapImage;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Letölti a listában szereplő filmek posztereit
        /// </summary>
        /// <param name="movieList">A filmek listája</param>
        /// <returns></returns>
        private async Task GetPostersForMovieListAsync(MovieList movieList)
        {
            foreach (var movie in movieList.results)
            {
                BitmapImage image = new BitmapImage();
                if(movie.poster_path == null)
                {
                    try
                    {
                        image.UriSource = new System.Uri("ms-appx:///Assets/movie-poster-placeholder.png");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                {
                    image = await GetPosterAsync(movie.poster_path);
                }
                if (image != null)
                {
                    movie.poster = image;
                }
            }
        }

        /// <summary>
        /// Letölti a listában szereplő sorozatok posztereit
        /// </summary>
        /// <param name="seriesList">Sorozatok listája</param>
        /// <returns></returns>
        private async Task GetPosterForSeriesListAsync(SeriesList seriesList)
        {
            foreach (var series in seriesList.results)
            {
                BitmapImage image = new BitmapImage();
                if (series.poster_path == null)
                {
                    try
                    {
                        image.UriSource = new System.Uri("ms-appx:///Assets/movie-poster-placeholder.png");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                {
                    image = await GetPosterAsync(series.poster_path);
                }
                if (image != null)
                {
                    series.poster = image;
                }
            }
        }

        /// <summary>
        /// Letölti a filmek listáját poszterekkel együtt
        /// </summary>
        /// <param name="path">A lista elérési útvonala</param>
        /// <returns>Filmek listája poszterekkel</returns>
        private async Task<MovieList> GetMovieListWithPosterAsync(string path)
        {
            var resultList = await GetAsync<MovieList>(path);
            if (resultList != default)
            {
                await GetPostersForMovieListAsync(resultList);
            }
            return resultList;
        }

        /// <summary>
        /// Letölti a sorozatok listáját poszterekkel együtt
        /// </summary>
        /// <param name="path">A lista elérési útvonala</param>
        /// <returns>Sorozatok listája poszterekkel</returns>
        private async Task<SeriesList> GetSeriesListWithPosterAsync(string path)
        {
            var resultList = await GetAsync<SeriesList>(path);
            if(resultList != default)
            {
                await GetPosterForSeriesListAsync(resultList);
            }
            return resultList;
        }

        /// <summary>
        /// Letölti a népszerű filmeket
        /// </summary>
        /// <returns>Népszerű filmek listája</returns>
        public async Task<MovieList> GetPopularMoviesAsync()
        {

            return await GetMovieListWithPosterAsync("movie/popular?language=hu&page=1");
        }

        /// <summary>
        /// Letölti a most játszott filmeket
        /// </summary>
        /// <returns>Most játszott filmek listája</returns>
        public async Task<MovieList> GetNowPlayingMoviesAsync()
        {
            return await GetMovieListWithPosterAsync("movie/now_playing?language=hu&page=1");
        }

        /// <summary>
        /// Letölti a legjobbra értékelt filmeket
        /// </summary>
        /// <returns>Legjobbra értékelt filmek listája</returns>
        public async Task<MovieList> GetTopRatedMoviesAsync()
        {
            return await GetMovieListWithPosterAsync("movie/top_rated?language=hu&page=1");
        }

        /// <summary>
        /// Letölti az újdonságnak számító filmeket
        /// </summary>
        /// <returns>Újdonságnak számító filmek listája</returns>
        public async Task<MovieList> GetUpcomingMoviesAsync()
        {
            return await GetMovieListWithPosterAsync("movie/upcoming?language=hu&page=1");
        }

        /// <summary>
        /// Letölti a film részletes adatait
        /// </summary>
        /// <param name="id">A film azonosítója</param>
        /// <returns>Film részletes adatokkal</returns>
        public async Task<Movie> GetMovieDetailsAsync(int id)
        {
            return await GetAsync<Movie>($"movie/{id}?language=hu");
        }

        /// <summary>
        /// Letölti egy film stáblistáját
        /// </summary>
        /// <param name="id">A film azonosítója</param>
        /// <returns>A film stáblistája</returns>
        public async Task<Credits> GetMovieCastAsync(int id)
        {
            return await GetAsync<Credits>($"movie/{id}/credits?language=hu");
        }

        /// <summary>
        /// Letölti egy színész részletes adatait
        /// </summary>
        /// <param name="id">A színész azonsítója</param>
        /// <returns>A színész részletes adatai</returns>
        public async Task<Actor> GetActorDetailsAsync(int id)
        {
            return await GetAsync<Actor>($"person/{id}?language=hu");
        }

        /// <summary>
        /// Letölti a színész közreműködésével elkészült filmeket
        /// </summary>
        /// <param name="id">A színész azonosítója</param>
        /// <returns>Filmek melyben a színész közreműködött</returns>
        public async Task<ActorCast> GetActorCastAsync(int id)
        {
            return await GetAsync<ActorCast>($"person/{id}/movie_credits?language=hu");
        }

        /// <summary>
        /// Letölti a keresett filmeket
        /// </summary>
        /// <param name="queryString">Keresendő filmcím</param>
        /// <returns>A keresésnek megfelelő című filmek</returns>
        public async Task<MovieList> GetMovieSearchResultAsync(string queryString)
        {
            return await GetMovieListWithPosterAsync($"search/movie?query={queryString}&include_adult=false&language=hu&page=1");
        }

        /// <summary>
        /// Letölti a kapcsolódó filmeket
        /// </summary>
        /// <param name="movieId">A film azonosítója</param>
        /// <returns>A megadott filmhez kapcsolódó filmek listája</returns>
        public async Task<MovieList> GetRelatedMoviesAsync(int movieId)
        {
            return await GetMovieListWithPosterAsync($"movie/{movieId}/recommendations?language=hu&page=1");
        }

        /// <summary>
        /// Letölti a népszerű sorozatokat
        /// </summary>
        /// <returns>Nészerű sorozatok listája</returns>
        public async Task<SeriesList> GetPopularSeriesAsync()
        {
            return await GetSeriesListWithPosterAsync("tv/popular?language=hu&page=1");
        }

        /// <summary>
        /// Letölti a jelenleg futó sorozatokat
        /// </summary>
        /// <returns>Jelenleg futó sorozatok listája</returns>
        public async Task<SeriesList> GetOnTheAirSeriesAsync()
        {
            return await GetSeriesListWithPosterAsync("tv/on_the_air?language=hu&page=1");
        }

        /// <summary>
        /// Letölti a ma futó sorozatokat
        /// </summary>
        /// <returns>Ma futó sorozatok listája</returns>
        public async Task<SeriesList> GetAiringTodaySeriesAsync()
        {
            return await GetSeriesListWithPosterAsync("tv/airing_today?language=hu&page=1");
        }

        /// <summary>
        /// Letölti a legjobbra értékelt sorozatokat
        /// </summary>
        /// <returns>Legjobbra értékelt sorozatok listája</returns>
        public async Task<SeriesList> GetTopRatedSeriesAsync()
        {
            return await GetSeriesListWithPosterAsync("tv/top_rated?language=hu&page=1");
        }

        /// <summary>
        /// Letölti a keresett sorozatokat
        /// </summary>
        /// <param name="queryString">Keresendő sorozatím</param>
        /// <returns>A keresésnek megfelelő című sorozatok</returns>
        public async Task<SeriesList> GetSeriesSearchResultAsync(string queryString)
        {
            return await GetSeriesListWithPosterAsync($"search/tv?query={queryString}&include_adult=false&language=hu&page=1");
        }

        /// <summary>
        /// Letölti a sorozat részletes adatait
        /// </summary>
        /// <param name="seriesId">A sorozat azonosítója</param>
        /// <returns>Sorozat részletes adatokkal</returns>
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

        /// <summary>
        /// Letölti egy sorozat stáblistáját
        /// </summary>
        /// <param name="id">A sorozat azonosítója</param>
        /// <returns>A sorozat stáblistája</returns>
        public async Task<Credits> GetSeriesCastAsync(int seriesId)
        {
            return await GetAsync<Credits>($"tv/{seriesId}/credits?language=hu");
        }
        /// <summary>
        /// Letölti a kapcsolódó sorozatokat
        /// </summary>
        /// <param name="movieId">A sorozat azonosítója</param>
        /// <returns>A megadott sorozathoz kapcsolódó sorozatok listája</returns>
        public async Task<SeriesList> GetRecommendedSeriesAsync(int seriesId)
        {
            return await GetSeriesListWithPosterAsync($"tv/{seriesId}/recommendations?language=hu&page=1");
        }

        /// <summary>
        /// Letölti egy színész közreműködéseit
        /// </summary>
        /// <param name="actorId">Színész azonosítója</param>
        /// <returns>Sorozatok, melyben a színész közreműködött</returns>
        public async Task<ActorCast> GetActorSeriesCreditsAsync(int actorId)
        {
            return await GetAsync<ActorCast>($"person/{actorId}/tv_credits?language=hu");
        }
    }
}
