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
        public IEnumerable<PictureModel> Pictures { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }

        public CatalogModel GetCatalogForMainPage(int page) {
            Pictures = (from picture in ApplicationFacade.Instance.PictureManager.GetAllPictures(page - 1)
                            select "Content/Puzzles/" + picture.Picture
                                into source
                                let image = new Bitmap(HostingEnvironment.ApplicationPhysicalPath + source)
                                select new PictureModel {
                                    Id = source.Substring(source.LastIndexOf('/') + 1, source.LastIndexOf('.') - (source.LastIndexOf('/') + 1)),
                                    Height = image.Height, Width = image.Width, Source ="~/" + source
                                }).ToList();
            CurrentPage = page;
            PageCount = ApplicationFacade.Instance.PictureManager.GetTotalPages();
            return this;
        }

        public CatalogModel GetCatalogForCabinet(int page) {
            CurrentPage = page;
            var af = ApplicationFacade.Instance;
            var scores = af.ScoreManager.GetUserScoreById(af.GetCurrentUser().Id).ToList();
            Pictures = (from score in scores
                            let picture = af.PictureManager.GetPictureByPictureId(score.PictureId)
                            let path = HostingEnvironment.ApplicationPhysicalPath + @"Content/Puzzles/" + picture.Picture
                            let image = new Bitmap(path)
                            select new PictureModel {
                                Complexity = score.Complexity,
                                Id = picture.PictureId,
                                Height = image.Height,
                                Source = "~/Content/Puzzles/" + picture.Picture,
                                Score = score
                            }).Skip((page - 1) * ItemPerPage).Take(ItemPerPage).ToList();
            PageCount = (scores.Count() / ItemPerPage) + 1;
            return this;
        }
    }
}