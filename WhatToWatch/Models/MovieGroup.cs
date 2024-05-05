using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToWatch.Models
{
    public class MovieGroup
    {
        public string Title { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
