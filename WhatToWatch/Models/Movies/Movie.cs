using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WhatToWatch.Models
{

    public class Movie
    {
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public object belongs_to_collection { get; set; }
        public int budget { get; set; }
        public List<Genre> genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public string imdb_id { get; set; }
        public List<string> origin_country { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public float popularity { get; set; }
        public string poster_path { get; set; }
        public List<Production_Company> production_companies { get; set; }
        public List<Production_Country> production_countries { get; set; }
        public string release_date { get; set; }
        public int revenue { get; set; }
        public int runtime { get; set; }
        public List<Spoken_Language> spoken_languages { get; set; }
        public string status { get; set; }
        public string tagline { get; set; }
        public string title { get; set; }
        public bool video { get; set; }
        public float vote_average { get; set; }
        public int vote_count { get; set; }
        public BitmapImage poster { get; set; }

        public double VotePercent
        {
            get { return vote_average * 10; }
        }

        public string ReleaseYear
        {
            get
            {
                if(release_date != null)
                {
                    DateTime releaseDate = DateTime.Parse(release_date);
                    return releaseDate.ToString("yyyy");
                }
                else
                {
                    return "-";
                }
            }
        }

        public string Runtime
        {
            get
            {
                if(runtime != null)
                {
                    return $"{runtime / 60} óra {runtime % 60} perc";
                }
                else
                {
                    return "-";
                }
                
            }
        }

        public string Budget
        {
            get
            {
                if(budget == 0)
                {
                    return "-";
                }
                else
                {
                    return $"{budget} $";
                }
            }
        }

        public string Revenue
        {
            get
            {
                if(revenue == 0)
                {
                    return "-";
                }
                else
                {
                    return $"{revenue} $";
                }
            }
        }
    }
}
