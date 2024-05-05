using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToWatch.Models
{

    public class MovieList
    {
        public Dates dates { get; set; }
        public int page { get; set; }
        public List<Movie> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }

        public MovieList()
        {
            this.dates = new Dates();
            this.page = 0;
            this.results = new List<Movie>();
            this.total_pages = 0;
            this.total_results = 0;
        }
    }

    public class Dates
    {
        public string maximum { get; set; }
        public string minimum { get; set; }

        public Dates() {
            this.minimum = "";
            this.maximum = "";
        }
    }
}
