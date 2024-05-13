using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToWatch.Models
{

    /// <summary>
    /// Filmek listáját összefoglaló osztály
    /// </summary>
    public class MovieList
    {
        /// <summary>
        /// Visszaadja vagy beállítja a lista oldalának számát
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja a listához tartozó filmeket
        /// </summary>
        public List<Movie> results { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja a lista összes oldalának számát
        /// </summary>
        public int total_pages { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja a lista összes filmjének számát
        /// </summary>
        public int total_results { get; set; }
    }
}
