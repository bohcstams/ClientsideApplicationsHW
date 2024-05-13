using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToWatch.Models
{

    /// <summary>
    /// Egy színész szerepléseit összefoglaló osztály
    /// </summary>
    public class ActorCast
    {
        /// <summary>
        /// Visszaadja, vagy beállítja azokat a filmeket, amikben játszott
        /// </summary>
        public List<Movie> cast { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja azokat a filmeket, amikben közreműködött
        /// </summary>
        public List<Movie> crew { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész azonosítóját
        /// </summary>
        public int id { get; set; }
    }
}
