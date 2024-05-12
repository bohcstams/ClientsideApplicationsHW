using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToWatch.Models
{

    public class SeriesList
    {
        public int page { get; set; }
        public List<Series> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}
