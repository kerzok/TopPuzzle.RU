using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        private const int ItemsPerPage = 9;

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
                Tags = "city"
            };

            
            var photos = _api.PhotosSearch(options);
            var photo = photos[0];
            if (GetPictureByPictureId(photo.PhotoId) != null) return;
            var image = GetImageFromUrl(photo.LargeUrl);
            Directory.CreateDirectory(@"C:\TopPuzzle\TopPuzzle.RU\source\Site\Content\Puzzles\");
            image.Save(@"C:\TopPuzzle\TopPuzzle.RU\source\Site\Content\Puzzles\" + photo.PhotoId + ".jpg");

            SavePicture(new Pictures {
                PictureId = photo.PhotoId,
                Picture = photo.PhotoId + ".jpg"
            });
        }

        public Pictures GetPictureByPictureId(string id) {
            return SqlMapper.Execute<Pictures>("GetPictureByPictureId", new { Id = id }).SingleOrDefault();
        }

        public int GetTotalPages() {
            var databaseResult = SqlMapper.Execute<int>("GetTotalPicturesCount").SingleOrDefault();
            return databaseResult % ItemsPerPage == 0 ? databaseResult / ItemsPerPage : (databaseResult / ItemsPerPage) + 1;
        }

        public void SavePicture(Pictures picture) {
            SqlMapper.Execute("SavePicture", new { picture.PictureId, picture.Picture});
        }

        public IEnumerable<Pictures> GetAllPictures(int page) {
            return SqlMapper.Execute<Pictures>("GetAllPictures", new {startRowIndex = page*ItemsPerPage, maximumIndex = ItemsPerPage});
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

        public IEnumerable<int> GetRandomList(int complexity)
        {
            var list = new List<int>();
            var count = 0;
            switch (complexity)
            {
                case 1:
                    count = 12;
                    break;
                case 2:
                    count = 24;
                    break;
                case 3:
                    count = 48;
                    break;
            }
            var r = new Random();
            var notUsed = new List<int>();
            for (var i = 0; i < count; ++i) notUsed.Add(i);
            for (var i = 0; i < count; ++i)
            {
                var ind = r.Next(0, notUsed.Count);
                var n = notUsed[ind];
                notUsed.RemoveAt(ind);
                list.Add(n);
            }
            return list;
        } 

        public IEnumerable<string> CutImage(Bitmap image, int complexity) {
            var list = new List<string>();
            int countW, countH;
            switch (complexity) {
                case 1:
                    countW = 4;
                    countH = 3;
                    break;
                case 2:
                    countW = 6;
                    countH = 4;
                    break;
                case 3:
                    countW = 8;
                    countH = 6;
                    break;
                default:
                    countW = 0;
                    countH = 0;
                    break;
            }
            var width = image.Width / countW;
            var height = image.Height / countH;
            using (var dst = new Bitmap(width, height, image.PixelFormat))
            using (var gfx = Graphics.FromImage(dst))
                for (var y = 0; y < countH; ++y) {
                    for (var x = 0; x < countW; ++x) {
                        var destRect = new Rectangle(0, 0, dst.Width, dst.Height);
                        var srcRec = new Rectangle(x * dst.Width, y * dst.Height, dst.Width, dst.Height);
                        gfx.DrawImage(image, destRect, srcRec, GraphicsUnit.Pixel);
                        string base64;
                        using (var ms = new MemoryStream())
                        {
                            dst.Save(ms, ImageFormat.Png);
                            base64 = Convert.ToBase64String(ms.ToArray());
                        }
                        list.Add(base64);
                    }
                }
            return list;
        }
    }
}
