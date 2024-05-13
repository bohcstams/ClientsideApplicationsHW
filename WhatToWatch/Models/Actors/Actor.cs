using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToWatch.Models
{
    /// <summary>
    /// Egy színész adatait összefoglaló osztály
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// Visszaadja vagy beállítja, hogy felnőttfilmes-e a színész vagy sem
        /// </summary>
        public bool adult { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja, hogy milyen neveken ismert még a színész
        /// </summary>
        public List<string> also_known_as { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész életrajzát
        /// </summary>
        public string biography { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész születésnapját
        /// </summary>
        public string birthday { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész halálának dátumát
        /// </summary>
        public object deathday { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész nemét 
        /// </summary>
        public int gender { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész kezdőlapját
        /// </summary>
        public object homepage { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész azonosítóját
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész imdb azonosítóját
        /// </summary>
        public string imdb_id { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész munkakörét
        /// </summary>
        public string known_for_department { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész nevét
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész születési helyét
        /// </summary>
        public string place_of_birth { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész népszerűségét
        /// </summary>
        public float popularity { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész profiljának útvonalát
        /// </summary>
        public string profile_path { get; set; }
    }

}
