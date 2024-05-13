using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WhatToWatch.Models
{

    /// <summary>
    /// Egy film adatait összefoglaló osztály
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Visszaadja, vagy beállítja hogy a film felnőttfilm-e
        /// </summary>
        public bool adult { get; set; }

        /// <summary>
        /// Visszaadja, vagy beállítja a film hátterének útvonalát
        /// </summary>
        public string backdrop_path { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film költségét
        /// </summary>
        public int budget { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film műfajainak listájátd
        /// </summary>
        public List<Genre> genres { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film azonosítóját
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film nevét
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film áttekintését
        /// </summary>
        public string overview { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film népszerűségét
        /// </summary>
        public float popularity { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film poszterének útvonalát
        /// </summary>
        public string poster_path { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film kiadásának dátumát
        /// </summary>
        public string release_date { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film bevételét
        /// </summary>
        public int revenue { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film hosszát
        /// </summary>
        public int runtime { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film mottóját
        /// </summary>
        public string tagline { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film címét
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a filmre adott szavazatok átlagát
        /// </summary>
        public float vote_average { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a filmre adott szavazatok számát
        /// </summary>
        public int vote_count { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a film poszterét
        /// </summary>
        public BitmapImage poster { get; set; }
        /// <summary>
        /// Visszaadja a filmre adott szavazatok átlágát százalékban
        /// </summary>
        public double VotePercent
        {
            get { return vote_average * 10; }
        }
        /// <summary>
        /// Visszaadja a film kiadási dátumát formázva
        /// </summary>
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
        /// <summary>
        /// Visszaadja a film hosszát formázva
        /// </summary>
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

        /// <summary>
        /// Visszaadja a film költségét formázva
        /// </summary>
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
        /// <summary>
        /// Visszaadja a film bevételét formázva
        /// </summary>
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
