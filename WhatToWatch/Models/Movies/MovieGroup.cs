using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToWatch.Models
{
    /// <summary>
    /// Filmek egy csoportját összefoglaló osztály
    /// </summary>
    public class MovieGroup
    {
        /// <summary>
        /// Visszaadja vagy beálljtja a csoport megnevezését
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja a csoportba tartozó filmeket
        /// </summary>
        public List<Movie> Movies { get; set; }
    }
}
