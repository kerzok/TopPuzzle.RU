using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Hosting;
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

        public Pictures GetRandomPicture() {
            var random = new Random();
            var options = new PhotoSearchOptions {
                PerPage = 12,
                Page = 1,
                SortOrder = PhotoSearchSortOrder.DatePostedDescending,
                MediaType = MediaType.Photos,
                SafeSearch = SafetyLevel.Safe,
                Tags = "city, car, nature, supercars"
            };

            var photos = _api.PhotosSearch(options);
            var photo = photos[random.Next(photos.Count - 1)];
            var currentPicture = GetPictureByPictureId(photo.PhotoId);
            if (currentPicture != null) return currentPicture;
            var image = GetImageFromUrl(photo.LargeUrl);
            Directory.CreateDirectory(HostingEnvironment.ApplicationPhysicalPath + @"\Content\Puzzles\");
            NormalizeAndSaveImage(image, HostingEnvironment.ApplicationPhysicalPath + @"\Content\Puzzles\" + photo.PhotoId + ".jpg");
            return InsertPicture(new Pictures {
                PictureId = photo.PhotoId,
                Picture = photo.PhotoId + ".jpg"
            });
        }

        public Pictures GetPictureByPictureId(string pictureId) {
            return SqlMapper.Execute<Pictures>("GetPictureByPictureId", new { pictureId }).SingleOrDefault();
        }

        public int GetTotalPages() {
            var databaseResult = SqlMapper.Execute<int>("GetPicturesCount").SingleOrDefault();
            return databaseResult % ItemsPerPage == 0 ? databaseResult / ItemsPerPage : (databaseResult / ItemsPerPage) + 1;
        }

        public Pictures InsertPicture(Pictures picture) {
            return SqlMapper.Execute<Pictures>("InsertPicture", new { picture.PictureId, picture.Picture}).SingleOrDefault();
        }

        public Pictures GetPictureById(int id) {
            return SqlMapper.Execute<Pictures>("GetPictureById", new {id}).SingleOrDefault();
        }

        public IEnumerable<Pictures> GetPictures(int page) {
            return SqlMapper.Execute<Pictures>("GetPictures", new {startRowIndex = page*ItemsPerPage, maximumIndex = ItemsPerPage});
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

        public void NormalizeAndSaveImage(Image image, string path)
        {
            var modWidth = image.Width%48;
            var modHeight = image.Height%24;
            var width = image.Width - modWidth;
            var height = image.Height - modHeight;
            using (var dst = new Bitmap(width, height, image.PixelFormat))
            {
                using (var gfx = Graphics.FromImage(dst))
                {
                    var destRect = new Rectangle(0, 0, dst.Width, dst.Height);
                    var srcRec = new Rectangle(modWidth/2, modHeight/2, dst.Width, dst.Height);
                    gfx.DrawImage(image, destRect, srcRec, GraphicsUnit.Pixel);
                }
                dst.Save(path);
            }
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
