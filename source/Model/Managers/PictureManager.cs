﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using Dapper;
using FlickrNet;
using Toppuzzle.Model.DataMapping;
using Toppuzzle.Model.Entities;

namespace Toppuzzle.Model.Managers
{
    public class PictureManager : BaseManager {
        private readonly Flickr _api;

        public PictureManager(ISqlMapper mapper) : base(mapper) {
            _api = new Flickr("b7174fdfb7f58dde37f98791c9c725f9", "96080f898b0da2e3") {InstanceCacheDisabled = true};
        }

        public void GetRandomPicture() {
            var options = new PhotoSearchOptions {
                PerPage = 12,
                Page = 1,
                SortOrder = PhotoSearchSortOrder.DatePostedDescending,
                MediaType = MediaType.Photos,
                SafeSearch = SafetyLevel.Safe,
                Tags = "nature"
            };

            
            var photos = _api.PhotosSearch(options);
            var photo = photos[0];
            if (GetPictureByPictureId(photo.PhotoId) == null) {
                var image = GetImageFromUrl(photo.LargeUrl);
                Directory.CreateDirectory(@"C:\Web\toppuzzle\web\Content\localImage\");
                image.Save(@"C:\Web\toppuzzle\web\Content\localImage\" + photo.PhotoId + ".jpg");

                SavePicture(new Pictures {
                    PictureId = photo.PhotoId,
                    Picture = photo.PhotoId + ".jpg"
                });
            }
        }

        public Pictures GetPictureByPictureId(string id) {
            return SqlMapper.Execute<Pictures>("GetPictureByPictureId", new { Id = id }).SingleOrDefault();
        }

        public void SavePicture(Pictures picture) {
            SqlMapper.Execute("SavePicture", new { picture.PictureId, picture.Picture});
        }

        private static Image GetImageFromUrl(string url) {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            using (var httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse()) {
                using (var stream = httpWebReponse.GetResponseStream()) {
                    if (stream != null) return Image.FromStream(stream);
                }
            }
            return null;
        }
    }
}
