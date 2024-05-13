using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace WhatToWatch.Models
{

    /// <summary>
    /// Egy  sorozat évadának adatait összefoglaló osztály
    /// </summary>
    public class Season
    {
        /// <summary>
        /// Visszaadja vagy beállítja az évad lejátsztásának dátumát
        /// </summary>
        public string air_date { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az évad epizódjainak számát
        /// </summary>
        public int episode_count { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az évad epizódjait
        /// </summary>
        public List<Episode> episodes { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az évad címét
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az évad áttekintését
        /// </summary>
        public string overview { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az évad azonosítóját
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az évad poszterének útvonalát
        /// </summary>
        public string poster_path { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az évad sorszámát
        /// </summary>
        public int season_number { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az évad szavazatainak átlagát
        /// </summary>
        public float vote_average { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az évad poszterét
        /// </summary>
        public BitmapImage poster { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az évad címét az epizódok számával együtt
        /// </summary>
        public string NameWithEpisodeCount
        {
            get
            {
                return $"{name} ({episodes.Count})";
            }
        }
    }

    /// <summary>
    /// Egy epizód adatait összefoglaló osztály
    /// </summary>
    public class Episode
    {
        /// <summary>
        /// Visszaadja vagy beállítja az epizód lejátsztásának dátumát
        /// </summary>
        public string air_date { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az epizód sorszámát
        /// </summary>
        public int episode_number { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az epizód típusát
        /// </summary>
        public string episode_type { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az epizód azonosítóját
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az epizód címét
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja az epizód áttekintését
        /// </summary>
        public string overview { get; set; }
    }

}
