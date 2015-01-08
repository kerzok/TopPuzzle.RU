using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlickrNet;

namespace PictureLoader
{
    public class PictureManager {
        private Flickr api;

        public PictureManager() {
            api = new Flickr("b7174fdfb7f58dde37f98791c9c725f9", "96080f898b0da2e3");

        }
    }
}
