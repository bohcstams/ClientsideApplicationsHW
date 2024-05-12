using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WhatToWatch.Models
{

    public class Series
    {
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public List<Created_By> created_by { get; set; }
        public List<int> episode_run_time { get; set; }
        public string first_air_date { get; set; }
        public List<Genre> genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public bool in_production { get; set; }
        public List<string> languages { get; set; }
        public string last_air_date { get; set; }
        public Last_Episode_To_Air last_episode_to_air { get; set; }
        public string name { get; set; }
        public object next_episode_to_air { get; set; }
        public List<Network> networks { get; set; }
        public int number_of_episodes { get; set; }
        public int number_of_seasons { get; set; }
        public List<string> origin_country { get; set; }
        public string original_language { get; set; }
        public string original_name { get; set; }
        public string overview { get; set; }
        public float popularity { get; set; }
        public string poster_path { get; set; }
        public List<Production_Company> production_companies { get; set; }
        public List<Production_Country> production_countries { get; set; }
        public List<Season> seasons { get; set; }
        public List<Spoken_Language> spoken_languages { get; set; }
        public string status { get; set; }
        public string tagline { get; set; }
        public string type { get; set; }
        public float vote_average { get; set; }
        public int vote_count { get; set; }

        public BitmapImage poster { get; set; }
    }

    public class Last_Episode_To_Air
    {
        public int id { get; set; }
        public string overview { get; set; }
        public string name { get; set; }
        public int vote_average { get; set; }
        public int vote_count { get; set; }
        public string air_date { get; set; }
        public int episode_number { get; set; }
        public string episode_type { get; set; }
        public string production_code { get; set; }
        public object runtime { get; set; }
        public int season_number { get; set; }
        public int show_id { get; set; }
        public object still_path { get; set; }
    }

    public class Created_By
    {
        public int id { get; set; }
        public string credit_id { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public int gender { get; set; }
        public string profile_path { get; set; }
    }

    public class Network
    {
        public int id { get; set; }
        public string logo_path { get; set; }
        public string name { get; set; }
        public string origin_country { get; set; }
    }

    public class Season
    {
        public string air_date { get; set; }
        public int episode_count { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public int season_number { get; set; }
        public int vote_average { get; set; }
    }
}
