﻿using SpotifyAPI.Web.Models;
using System.Collections.Generic;

namespace Songify_Slim.Models
{
    public class TrackInfo
    {
        public string Artists { get; set; }
        public string Title { get; set; }
        public List<Image> albums { get; set; }
        public string SongID { get; set; }
        public int DurationMS { get; set; }
        public bool isPlaying { get; set; }
        public string url { get; set; }
        public int DurationPercentage { get; set; }
        public int DurationTotal { get; set; }
        public int Progress { get; set; }
    }
}