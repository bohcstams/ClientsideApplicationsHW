using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToWatch.Models
{
    /// <summary>
    /// Egy film vagy sorozat stáblistáját összefoglaló osztály
    /// </summary>
    public class Credits
    {
        /// <summary>
        /// /// <summary>
        /// Visszaadja vagy beállítja a stáblista azonosítóját
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja a stáblista színészeinek listáját
        /// </summary>
        public List<Cast> cast { get; set; }
        /// <summary>
        /// Visszaadja vagy beállítja a stáblista munkatársainak listáját
        /// </summary>
        public List<Crew> crew { get; set; }
    }

    public class Cast
    {
        /// <summary>
        /// Visszaadja, vagy beállítja a színész azonosítóját
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész nevét
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész eredeti nevét
        /// </summary>
        public string original_name { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész által játszott szereplő nevét
        /// </summary>
        public string character { get; set; }
    }

    public class Crew
    {
        /// <summary>
        /// Visszaadja, vagy beállítja a színész azonosítóját
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész nevét
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész eredeti nevét
        /// </summary>
        public string original_name { get; set; }
        /// <summary>
        /// Visszaadja, vagy beállítja a színész által végzett munkát
        /// </summary>
        public string job { get; set; }
    }

}
