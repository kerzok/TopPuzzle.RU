using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Toppuzzle.Site.Infrastucture;

namespace Toppuzzle.Site.Models {
    public class CatalogModel : BaseModel {
        private const int ItemPerPage = 10;
        public List<PictureModel> Pictures { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int UserId { get; set; }

        public CatalogModel() {
            CurrentPage = 1;
        }

        public CatalogModel GetCatalogForMainPage() {
            var picturesForPage = ApplicationFacade.Instance.PictureManager.GetPictures(CurrentPage - 1);
            Pictures = new List<PictureModel>();
            foreach (var picture in picturesForPage) {
                var source = "Content/Puzzles/" + picture.Picture;
                var image = new Bitmap(HostingEnvironment.ApplicationPhysicalPath + source);
                Pictures.Add(new PictureModel {
                        Id = picture.Id,            
                        PictureId = source.Substring(source.LastIndexOf('/') + 1,
                        source.LastIndexOf('.') - (source.LastIndexOf('/') + 1)),
                        Height = image.Height, 
                        Width = image.Width, 
                        Source ="~/" + source
                    });
            }
            PageCount = ApplicationFacade.Instance.PictureManager.GetTotalPages();
            return this;
        }

        public CatalogModel GetCatalogForCabinet() {
            var af = ApplicationFacade.Instance;
            var scores = af.ScoreManager.GetUserScoreById(UserId).Distinct().ToList();
            Pictures = new List<PictureModel>();
            for (var index = (CurrentPage - 1) * ItemPerPage; index < Math.Min((CurrentPage - 1) * ItemPerPage + ItemPerPage, scores.Count); index++) {
                var score = scores[index];
                var picture = af.PictureManager.GetPictureById(score.PictureId);
                var path = HostingEnvironment.ApplicationPhysicalPath + @"Content/Puzzles/" + picture.Picture;
                var image = new Bitmap(path);
                Pictures.Add(new PictureModel {
                    Complexity = score.Complexity,
                    PictureId = picture.PictureId,
                    Height = image.Height,
                    Source = "~/Content/Puzzles/" + picture.Picture,
                    Score = score
                });
            }
            PageCount = (scores.Count() % ItemPerPage == 0) ? (scores.Count() / ItemPerPage) : (scores.Count() / ItemPerPage) + 1;
            return this;
        }
    }
}