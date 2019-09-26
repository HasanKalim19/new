using DAL;
using LibNeeo.NearByMe.Model;
using LibNeeo.NearByMe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeeo.NearByMe
{
   public  class NearByMePromotionManager
    {
        private DbManager _dbManager = new DbManager();
        public async Task<bool> UpsertPromotionNew(NearByMePromotion promotion)
        {
            StringBuilder ImagesXml = new StringBuilder("<ImageList>");
            foreach (NearByMePromotionImage item in promotion.ImagesXml)
            {

                //string str = item.imagePath;
                //str = str.Substring(str.LastIndexOf('/') + 1);
                //str = str.Substring(0, str.IndexOf("_u_")) /*+ ".jpg"*/;
                //var ex = str.Substring(str.Length - 3);
                //str = str + "." + ex;
                ImagesXml.Append("<Image>" +
                        "<ImageCaption>" + item.imageCaption + "</ImageCaption>" +
                        "<ImagePath>" + item.imagePath + "</ImagePath>" +
                        "<FeaturedImage>" + item.featuredImage + "</FeaturedImage>" +
                         "<ImageName>" + item.ImageName + "</ImageName>" +
                     "</Image>");

            }
            ImagesXml.Append("</ImageList>");
            return await System.Threading.Tasks.Task.Run(() => _dbManager.UpsertPromotion(promotion.username, promotion.description, promotion.status, ImagesXml.ToString()));
        }

        public async Task<bool> UpsertPromotion(NearByMePromotion promotion)
        {
            StringBuilder ImagesXml = new StringBuilder("<ImageList>");
            foreach (NearByMePromotionImage item in promotion.ImagesXml)
            {

                string str = item.imagePath;

                var ext = item.imagePath.Substring(item.imagePath.LastIndexOf("."));

                // Construct file save path
                //string newfileName = httpPostedFile.FileName.Remove(httpPostedFile.FileName.Length - 4, 4) + "_u_" + Guid.NewGuid() + "."+ext;
               // string newfileName = httpPostedFile.FileName.Substring(0, httpPostedFile.FileName.LastIndexOf(".")) + "_u_" + Guid.NewGuid() + "." + ext;
               // var ex = str.Substring(str.Length - 3);
                str = str.Substring(str.LastIndexOf('/') + 1);
                str = str.Substring(0, str.IndexOf("_u_")) /*+ ".jpg"*/;
               
                str = str +  ext;
                ImagesXml.Append("<Image>" +
                        "<ImageCaption>" + item.imageCaption + "</ImageCaption>" +
                        "<ImagePath>" + item.imagePath + "</ImagePath>" +
                        "<FeaturedImage>" + item.featuredImage + "</FeaturedImage>" +
                         "<ImageName>" + str + "</ImageName>" +
                     "</Image>");

            }
            ImagesXml.Append("</ImageList>");
            return await System.Threading.Tasks.Task.Run(() => _dbManager.UpsertPromotion(promotion.username, promotion.description, promotion.status, ImagesXml.ToString()));
        }
        public async Task<bool> UpdateFeaturedImage(int promotionId, long imageId)
        {
            return await System.Threading.Tasks.Task.Run(() => _dbManager.UpdateFeaturedImage(promotionId, imageId));
        }
        public async Task<Object> GetTOPNPromotions(string username, int top)
        {
            DataTable result = await System.Threading.Tasks.Task.Run(() => _dbManager.GetTOPNPromotions(username, top));
            var promotions = (from row in result.AsEnumerable()
                              select new
                              {
                                  promotionId = Convert.ToInt32(row["promotionId"]),
                                  username = Convert.ToString(row["username"]),
                                  description = Convert.ToString(row["description"]),
                                  imageCaption = Convert.ToString(row["imageCaption"]),
                                  imagePath = Convert.ToString(row["imagePath"]),
                                  imageFileName = Convert.ToString(row["imageFileName"]),
                                  featuredImage = (row["featuredImage"] != DBNull.Value) ? Convert.ToBoolean(row["featuredImage"]) : (Boolean?)null,

                              }).ToList();
            return promotions;
        }
        public async Task<Object> GetPromotionByUserName(string username)
        {
            DataSet result = await System.Threading.Tasks.Task.Run(() => _dbManager.GetPromotionByUserName(username));
            var promotion = result.Tables[0];
            var images = result.Tables[1];
            var promotions = (from row in promotion.AsEnumerable()
                              select new
                              {
                                  promotionId = Convert.ToInt32(row["promotionId"]),
                                  username = Convert.ToString(row["username"]),
                                  description = Convert.ToString(row["description"]),
                                  status = (row["status"] != DBNull.Value) ? Convert.ToByte(row["status"]) : (Byte?)null,
                                  createdDate = Convert.ToDateTime(row["createdDate"]),
                                  updatedDate = Convert.ToDateTime(row["updatedDate"]),
                                  ImagesXml = (from row1 in images.AsEnumerable()
                                            select new
                                            {
                                                imageId = Convert.ToInt64(row1["imageId"]),
                                                imageCaption = Convert.ToString(row1["imageCaption"]),
                                                imagePath = Convert.ToString(row1["imagePath"]),
                                                imageFileName = Convert.ToString(row1["imageFileName"]),
                                                promotionId = Convert.ToInt32(row1["promotionId"]),
                                                featuredImage = (row1["featuredImage"] != DBNull.Value) ? Convert.ToBoolean(row1["featuredImage"]) : (Boolean?)null,
                                                createdDate = Convert.ToDateTime(row1["createdDate"]),
                                                updatedDate = Convert.ToDateTime(row1["updatedDate"]),
                                            }).ToList(),
                              }).FirstOrDefault();

            return promotions;
        }

        public async Task<Object> GetPromotionById(int promotionId)
        {
            DataSet result = await System.Threading.Tasks.Task.Run(() => _dbManager.GetPromotionById(promotionId));
            var promotion = result.Tables[0];
            var images = result.Tables[1];
            var promotions = (from row in promotion.AsEnumerable()
                              select new
                              {
                                  promotionId = Convert.ToInt32(row["promotionId"]),
                                  username = Convert.ToString(row["username"]),
                                  description = Convert.ToString(row["description"]),
                                  status = (row["status"] != DBNull.Value) ? Convert.ToByte(row["status"]) : (Byte?)null,
                                  createdDate = Convert.ToDateTime(row["createdDate"]),
                                  updatedDate = Convert.ToDateTime(row["updatedDate"]),
                                  ImagesXml = (from row1 in images.AsEnumerable()
                                               select new
                                               {
                                                   imageId = Convert.ToInt64(row1["imageId"]),
                                                   imageCaption = Convert.ToString(row1["imageCaption"]),
                                                   imagePath = Convert.ToString(row1["imagePath"]),
                                                   imageFileName = Convert.ToString(row1["imageFileName"]),
                                                   promotionId = Convert.ToInt32(row1["promotionId"]),
                                                   featuredImage = (row1["featuredImage"] != DBNull.Value) ? Convert.ToBoolean(row1["featuredImage"]) : (Boolean?)null,
                                                   createdDate = Convert.ToDateTime(row1["createdDate"]),
                                                   updatedDate = Convert.ToDateTime(row1["updatedDate"]),
                                               }).ToList(),
                              }).FirstOrDefault();

            return promotions;
        }
        public async Task<string> GetImageById(long imageid)
        {
            DataTable result = await System.Threading.Tasks.Task.Run(() => _dbManager.GetImageById(imageid));
            
            var image = (from row in result.AsEnumerable()
                              select new
                              {
                                
                                  imagePath = Convert.ToString(row["imagePath"]),
                                  imageFileName = Convert.ToString(row["imageFileName"]),

                              }).FirstOrDefault();
            if (image == null)
            {
                return "";
            }
            else
            {
                return image.imagePath;
            }
           
        }

        public async Task<int> GetFeaturedImage(int promotionId)
        {
            DataTable result = await System.Threading.Tasks.Task.Run(() => _dbManager.GetFeaturedImage(promotionId));

            var image = (from row in result.AsEnumerable()
                         select new
                         {
                             imageId = Convert.ToInt32(row["imageId"]),
                             imagePath = Convert.ToString(row["imagePath"]),

                         }).FirstOrDefault();
            if (image == null)
            {
                return 0;
            }
            else
            {
                return image.imageId;
            }

        }

        public async Task<int> DeleteNearByMePromotionImage(int promotionId,long imageId)
        {
           
            return await System.Threading.Tasks.Task.Run(() => _dbManager.DeleteNearByMePromotionImage(promotionId,imageId));
        }

        public async Task<int> DeleteNearByMePromotionImage(int promotionId, string imageId)
        {

            return await System.Threading.Tasks.Task.Run(() => _dbManager.DeleteNearByMePromotionImage(promotionId, imageId));
        }

        //by uzair

        public async Task<DataTable> GetPromotionImagesByUserName(string username)
        {
            DataSet result = await System.Threading.Tasks.Task.Run(() => _dbManager.GetPromotionByUserName(username));
            return result.Tables[1];

        }

        public async Task<bool> DeleteNearByMePromotion(string username)
        {
            return await System.Threading.Tasks.Task.Run(() => _dbManager.DeleteNearByMePromotion(username));
        }





    }
}
