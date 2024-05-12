using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToWatch.Models
{
    public class SeriesGroup
    {
        public string Title { get; set; }
        public List<Series> Series { get; set; }
    }
}
