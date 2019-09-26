using Common;
using Common.Controllers;
using LibNeeo.NearByMe;
using LibNeeo.NearByMe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Configuration;
using System.Web.Hosting;
using System.Linq;
using System.Data;
using LibNeeo.NearByMe.Model;

namespace PowerfulPal.Neeo.NearByMeApi.Controllers
{
    /// <summary>
    /// This near by me service for Neeo Messenger.
    /// </summary>
    [RoutePrefix("api/v1/near-by-me-promotion")]
    public class NearByMePromotionController : NeeoApiController
    {
        readonly NearByMePromotionManager nearByMePromotionManager = new NearByMePromotionManager();

        [Route("uploadImage")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadImage()
        {
            try
            {

                LogRequest("Im here");
                Logger.LogManager.CurrentInstance.InfoLogger.LogInfo(System.Reflection.MethodBase.GetCurrentMethod().GetType(), "uzair");

                var httpContext = HttpContext.Current;
                List<FileDetails> images = new List<FileDetails>();
                // Check for any uploaded file  
                if (httpContext.Request.Files.Count > 0)
                {
                    LogRequest("Count");
                    //Loop through uploaded files  
                    for (int i = 0; i < httpContext.Request.Files.Count; i++)
                    {
                        LogRequest("Count1");

                        HttpPostedFile httpPostedFile = httpContext.Request.Files[i];
                        if (httpPostedFile != null /*&& httpPostedFile.ContentType==*/)
                        {
                            //var ext = httpPostedFile.FileName.Substring(httpPostedFile.FileName.Length-3);
                            LogRequest(httpPostedFile.FileName);
                            var ext = httpPostedFile.FileName.Substring(httpPostedFile.FileName.LastIndexOf("."));
                            LogRequest("Count3");
                            // Construct file save path
                            //string newfileName = httpPostedFile.FileName.Remove(httpPostedFile.FileName.Length - 4, 4) + "_u_" + Guid.NewGuid() + "."+ext;
                            string newfileName = httpPostedFile.FileName.Substring(0, httpPostedFile.FileName.LastIndexOf(".")) + "_u_" + Guid.NewGuid() + ext;
                            var fileSavePath = Path.Combine(HostingEnvironment.MapPath("~" + ConfigurationManager.AppSettings["PromotionImagesPath"]), newfileName);
                            var returnpath = ConfigurationManager.AppSettings["imagesBaseUrl"] + ConfigurationManager.AppSettings["PromotionImagesPath"] + "/"+ newfileName;
                            // Save the uploaded file  
                            httpPostedFile.SaveAs(fileSavePath);
                            LogRequest("Count5");
                            FileDetails currentImage = new FileDetails();
                            currentImage.Name = httpPostedFile.FileName;
                            currentImage.Path = returnpath;
                            images.Add(currentImage);
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = images });
                }
                return Request.CreateResponse(HttpStatusCode.LengthRequired);
            }
            catch (ApplicationException applicationException)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), applicationException.Message, applicationException);
                throw;
                //return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
            }
            catch (Exception exception)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                throw;
                //Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                //return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        [Route("TESTING")]
        [HttpPost]
        public void TESTING()
        {
            try
                {
                LogRequest("Im here");
                Logger.LogManager.CurrentInstance.InfoLogger.LogInfo(System.Reflection.MethodBase.GetCurrentMethod().GetType(), "uzair");
                throw new Exception();

            }
            catch(Exception ex)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), ex.Message, ex);
            }


        }

        [Route("AddPromotionData")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddPromotionData([FromBody]NearByMePromotion promotion)
        {
            try
            {                
                LogRequest(promotion);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                bool operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.UpsertPromotion(promotion));
                var userPromotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetPromotionByUserName(promotion.username));
                return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = userPromotions });
            }
            catch (ApplicationException applicationException)
            {
                return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
            }
            catch (Exception exception)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private bool isFileExist(string file)
        {

            return File.Exists(file);
        }

        [Route("AddPromotionDataWithImages")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddPromotionDataNew()
        {
            try
            {

                LogRequest("Im here in new with images");

                Logger.LogManager.CurrentInstance.InfoLogger.LogInfo(System.Reflection.MethodBase.GetCurrentMethod().GetType(), "uzair");

                string data2 = "XXXXXXXX123";
                var httpContext = HttpContext.Current;
                dynamic data =  httpContext.Request.Form;
                if (data["username"] == null || data["username"]=="")
                {
                    data2 = data2 + "username data is null";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = "Username Null" });

                }

                LogRequest("X123" + data);
                if (data["featuredImageName"] == null)
                {
                    data2 = data2 + "featured image data is null";

                }
                else
                {

                    data2 = data2 + "featured image value is :" + data["featuredImageName"].ToString() + ";";

                }


                if (data["username"] == null)
                {
                    data2 = data2 + "username data is null";


                }
                else
                {

                    data2 = data2 + "username value is :" + data["username"].ToString() + ";"; 

                }

                if (data["status"] == null)
                {
                    data2 = data2 + "status data is null";

                }
                else
                {

                    data2 = data2 + "status value is :" + data["status"].ToString() + ";"; 

                }

                if (data["description"] == null)
                {
                    data2= data2 + "description data is null";

                }
                else
                {

                    data2 = data2 + "description value is :" + data["description"].ToString() + ";"; 

                }
                data2 = data2 + ";Number of Files:"+ (HttpContext.Current.Request.Files == null ? "0" : HttpContext.Current.Request.Files.Count.ToString());

                LogRequest(data2);
                if (HttpContext.Current.Request.Files == null || HttpContext.Current.Request.Files.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = "No images are in attachment." + data });
                }
                else {

                    //Loop through uploaded files  
                    for (int i = 0; i < httpContext.Request.Files.Count; i++)
                    {
                        data2 = httpContext.Request.Files[i].FileName;
                        LogRequest(data2);

                    }

                    }

                    string originalFileName = "";
              
                //Near by me promotion Values
                
                //binding Near by me promotion DTO
                LogRequest("1");
                string featuredImageName = Convert.ToString(data["featuredImageName"]);
                NearByMePromotion promotion = new NearByMePromotion();
                promotion.promotionId = 0;
                promotion.username = Convert.ToString(data["username"]);
                promotion.description = Convert.ToString(data["description"]);
                promotion.status = Convert.ToByte(data["status"]);
                //promotion.createdDate = Convert.ToDateTime(data["createdDate"]);
                //promotion.updatedDate = Convert.ToDateTime(data["updatedDate"]);
                promotion.ImagesXml = new List<NearByMePromotionImage>();
                LogRequest(promotion);
                LogRequest("2");
                //Getting user promotions
                var userPromotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetPromotionByUserName(promotion.username));
                bool featuredImageNameMatched = false;

                //Minimum 1 feature image needed at first time
                if (userPromotions == null && ((featuredImageName == null || featuredImageName == "") || httpContext.Request.Files.Count == 0))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = "There must be at least one featured image" });
                }

                //Check whern feature image name given but Files length 0
                //else if (featuredImageName != null && httpContext.Request.Files.Count == 0)
                //{
                //    return Request.CreateResponse(HttpStatusCode.LengthRequired, new { returnValue = "Given Featured Image does not exist in the provided list of image" });
                //}
                else
                {
                    LogRequest("4");
                    List<FileDetails> images = new List<FileDetails>();
                    // Check for any uploaded file  
                    if (httpContext.Request.Files.Count > 0)
                    {
                        string ext;
                        for (int i = 0; i < httpContext.Request.Files.Count; i++)
                        {
                            HttpPostedFile httpPostedFile = httpContext.Request.Files[i];
                            //fileContentType=httpPostedFile.ContentType.Split('/')[1];

                            ext = httpPostedFile.FileName.Substring(httpPostedFile.FileName.LastIndexOf("."));
                            var ggg = httpPostedFile.FileName.Substring(0, httpPostedFile.FileName.Length - (ext.Length));
                            if (httpPostedFile.FileName.Substring(0, httpPostedFile.FileName.Length - (ext.Length)) == featuredImageName)
                            {
                                featuredImageNameMatched = true;
                            }
                        }

                        if ((featuredImageNameMatched == false && featuredImageName != "") || (featuredImageNameMatched == false && featuredImageName != ""))
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = "Given Featured Image does not exist in the provided list of image" });
                        }

                        //Loop through uploaded files  
                        for (int i = 0; i < httpContext.Request.Files.Count; i++)
                        {
                            HttpPostedFile httpPostedFile = httpContext.Request.Files[i];

                            string filesExtention = (ConfigurationManager.AppSettings["nearByMeFilesExtention"]);
                            ext = httpPostedFile.FileName.Substring(httpPostedFile.FileName.LastIndexOf("."));
                            string[] filesExtentionSplit = filesExtention.Split(',');

                            LogRequest("5");
                           // if (httpPostedFile != null && filesExtentionSplit.Contains(ext.ToLower()))
                            {
                               

                                NearByMePromotionImage currentImage = new NearByMePromotionImage();
                                currentImage.ImageName = httpPostedFile.FileName;
                                ext = httpPostedFile.FileName.Substring(httpPostedFile.FileName.LastIndexOf("."));
                                string newfileName = httpPostedFile.FileName.Remove(httpPostedFile.FileName.Length - (ext.Length + 1), (ext.Length + 1)) + "" + ext /*".jpg"*/;


                                int w = 0;
                                while (isFileExist(Path.Combine(HostingEnvironment.MapPath("~" + ConfigurationManager.AppSettings["PromotionImagesPath"]), newfileName).ToString()) == true)
                                {
                                    ++w;
                                    //  newfileName = httpPostedFile.FileName.Remove(httpPostedFile.FileName.Length - 4, 4) + "(" + w + ").jpg";
                                    newfileName = httpPostedFile.FileName.Remove(httpPostedFile.FileName.Length - (ext.Length + 1), (ext.Length + 1)) + "(" + w + ")" + ext;
                                }



                                var fileSavePath = Path.Combine(HostingEnvironment.MapPath("~" + ConfigurationManager.AppSettings["PromotionImagesPath"]), newfileName);

                                // Save the uploaded file  
                                httpPostedFile.SaveAs(fileSavePath);
                                var returnpath = ConfigurationManager.AppSettings["imagesBaseUrl"] + ConfigurationManager.AppSettings["PromotionImagesPath"] + "/" + newfileName;

                                currentImage.imageCaption = newfileName;
                                currentImage.imagePath = returnpath;
                                promotion.ImagesXml.Add(currentImage);
                                if (httpPostedFile.FileName.Substring(0, httpPostedFile.FileName.Length - (ext.Length)) == featuredImageName)

                                //                                if (httpPostedFile.FileName.Remove(httpPostedFile.FileName.Length - (ext.Length + 1), (ext.Length + 1)) == featuredImageName)
                                {
                                    currentImage.featuredImage = true;
                                }

                            }
                        }
                    }
                    LogRequest("6");
                    bool operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.UpsertPromotionNew(promotion));
                    userPromotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetPromotionByUserName(promotion.username));
                    return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = userPromotions });
                }
            }
            catch (ApplicationException applicationException)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), applicationException.Message, applicationException);
                string mode = null;

                IEnumerable<String> headerValues;
                if (Request.Headers.TryGetValues("mode", out headerValues))
                {
                    mode = headerValues.FirstOrDefault();
                }
                if (mode != null && mode.ToLower() == "debug")
                {
                    throw;
                }
                else
                {
                    return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
                }
                // return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
            }
            catch (Exception exception)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                string mode = null;

                IEnumerable<String> headerValues;
                if (Request.Headers.TryGetValues("mode", out headerValues))
                {
                    mode = headerValues.FirstOrDefault();
                }
                if (mode != null && mode.ToLower() == "debug")
                {
                    throw;
                }
                else
                {
                    {

                        return Request.CreateResponse(HttpStatusCode.InternalServerError);
                    }
                    //Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                    //return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }
        }



  

        [HttpPut]
        [Route("UpdateFeaturedImage/{promotionId}/{imageId}")]
        public async Task<HttpResponseMessage> UpdateFeaturedImage([FromUri]int promotionId, [FromUri]long imageId)
        {
            try
            {
                LogRequest("promotionId" + promotionId + ",imageId " + imageId);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                bool operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.UpdateFeaturedImage(promotionId, imageId));
                var userPromotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetPromotionById(promotionId));
                return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = userPromotions });
                //return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = operationCompleted });
            }
            catch (ApplicationException applicationException)
            {
                return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
            }
            catch (Exception exception)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetPromotionByUserName/{username}")]
        public async Task<HttpResponseMessage> GetPromotionByUserName([FromUri]string username)
        {
            try
            {
                LogRequest("GetPromotionByUserName: " + username);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                var userPromotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetPromotionByUserName(username));

                return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = userPromotions });
            }
            catch (ApplicationException applicationException)
            {
                return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
            }
            catch (Exception exception)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetTOPNPromotions/{username}/{top}")]
        public async Task<HttpResponseMessage> GetTOPNPromotions([FromUri]string username, [FromUri]int top)
        {
            try
            {
                LogRequest("UId: " + username);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                var promotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetTOPNPromotions(username, top));

                return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = promotions } );
            }
            catch (ApplicationException applicationException)
            {
                return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
            }
            catch (Exception exception)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        #region Testing 
        //[Route("DeleteNearByMePromotionImage2/{promotionId}/{imageId}")]
        //[HttpDelete]
        //public async Task<HttpResponseMessage> DeleteNearByMePromotionImage2(int promotionId,long imageId )
        //{
        //    try
        //    {
        //        LogRequest("Image ID for Del: " + imageId);
        //        //if (!ModelState.IsValid)
        //        //{
        //        //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //        //}


        //        int operationCompleted = 0;
        //        int featuredImageId = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetFeaturedImage(promotionId));

        //        if (featuredImageId == imageId)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = "Featured image cannot be deleted." });

        //        }
        //      //  throw new ApplicationException();
        //        string fullImageUrl = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetImageById(imageId));
        //        string imageUrl = Path.Combine(HostingEnvironment.MapPath("~" + ConfigurationManager.AppSettings["PromotionImagesPath"]) , fullImageUrl.Substring(fullImageUrl.LastIndexOf('/')+1));

        //        //if (fullImageUrl != "" && System.IO.File.Exists(imageUrl))
        //        //{
        //            operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.DeleteNearByMePromotionImage(promotionId, imageId));
        //            if (operationCompleted == 1)
        //            {
        //            //    System.IO.File.Delete(imageUrl);
        //            }
        //            var userPromotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetPromotionById(promotionId));
        //            return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = userPromotions });

        //            // return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = operationCompleted });
        //        //}
        //        //else
        //        //{
        //        //    return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = operationCompleted }  );
        //        //}
        //    }
        //    catch (ApplicationException applicationException)
        //    {
        //        //throw;
        //        return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
        //    }
        //    catch (Exception exception)
        //    {
        //       Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
        //       return Request.CreateResponse(HttpStatusCode.InternalServerError);

        //        // return Request.CreateResponse(HttpStatusCode.InternalServerError);
        //      //  throw;

        //    }
        //}
        #endregion

        #region DeleteNearByMePromotionImage
        [Route("DeleteNearByMePromotionImage/{promotionId}/{imageId}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteNearByMePromotionImage(int promotionId, long imageId)
        {
            try
            {
                LogRequest("Image ID for Del: " + imageId);
                //if (!ModelState.IsValid)
                //{
                //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                //}


                int operationCompleted = 0;
                int featuredImageId = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetFeaturedImage(promotionId));

                if (featuredImageId == imageId)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = "Featured image cannot be deleted." });

                }
                //  throw new ApplicationException();
                string fullImageUrl = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetImageById(imageId));
                string imageUrl = Path.Combine(HostingEnvironment.MapPath("~" + ConfigurationManager.AppSettings["PromotionImagesPath"]), fullImageUrl.Substring(fullImageUrl.LastIndexOf('/') + 1));

                if (fullImageUrl != "" && System.IO.File.Exists(imageUrl))
                {
                    operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.DeleteNearByMePromotionImage(promotionId, imageId));
                    if (operationCompleted == 1)
                    {
                        System.IO.File.Delete(imageUrl);
                    }
                    var userPromotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetPromotionById(promotionId));
                    return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = userPromotions });
                }
                // return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = operationCompleted });
                //}
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = operationCompleted });
                }
            }
            catch (ApplicationException applicationException)
            {
                //throw;
                return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
            }
            catch (Exception exception)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

                // return Request.CreateResponse(HttpStatusCode.InternalServerError);
                //  throw;

            }
        }
        #endregion

        //bOdy 
        [Route("DeleteNearByMePromotionImage2")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteNearByMePromotionImage2(DeletePromotionImageDTO dto)
        {
            try
            {
                
                LogRequest("Image ID for Del: " + dto. imageId);
                //if (!ModelState.IsValid)
                //{
                //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                //}


                int operationCompleted = 0;
                int featuredImageId = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetFeaturedImage(dto.promotionId));

                if (featuredImageId == dto.imageId)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = "Featured image cannot be deleted." });

                }
                //  throw new ApplicationException();
                string fullImageUrl = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetImageById(dto.imageId));
                string imageUrl = Path.Combine(HostingEnvironment.MapPath("~" + ConfigurationManager.AppSettings["PromotionImagesPath"]), fullImageUrl.Substring(fullImageUrl.LastIndexOf('/') + 1));

                if (fullImageUrl != "" && System.IO.File.Exists(imageUrl))
                {
                    operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.DeleteNearByMePromotionImage(dto.promotionId, dto.imageId));
                    if (operationCompleted == 1)
                    {
                        System.IO.File.Delete(imageUrl);
                    }
                    var userPromotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetPromotionById(dto.promotionId));
                    return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = userPromotions });
                }
                // return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = operationCompleted });
                //}
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = operationCompleted });
                }
            }
            catch (ApplicationException applicationException)
            {
                //throw;
                return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
            }
            catch (Exception exception)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

                // return Request.CreateResponse(HttpStatusCode.InternalServerError);
                //  throw;

            }
        }
        #region DeleteNearByMePromotionImages
        [Route("DeleteNearByMePromotionImages/{promotionId}/{imageIds}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteNearByMePromotionImages(int promotionId, string imageIds)
        {
            try
            {
                LogRequest("Image ID for Del: " + imageIds);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                int featuredImageId = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetFeaturedImage(promotionId));


                if ((imageIds + ",").Contains(featuredImageId.ToString() + ","))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = "Featured image cannot be deleted." });

                }
                int operationCompleted = 0;
                var images = imageIds.Split(new char[] { ',' });

                foreach (var imageidStr in images)
                {
                    int imageId = Convert.ToInt32(imageidStr);
                    string fullImageUrl = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetImageById(imageId));
                    string imageUrl = Path.Combine(HostingEnvironment.MapPath("~" + ConfigurationManager.AppSettings["PromotionImagesPath"]), fullImageUrl.Substring(fullImageUrl.LastIndexOf('/') + 1));
                    operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.DeleteNearByMePromotionImage(promotionId, imageId));
                    if (fullImageUrl != "" && System.IO.File.Exists(imageUrl))
                    {
                        if (operationCompleted == 1)
                        {
                            System.IO.File.Delete(imageUrl);
                        }
                    }
                }
                var userPromotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetPromotionById(promotionId));
                return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = userPromotions });
            }
            catch (ApplicationException applicationException)
            {
                return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
            }
            catch (Exception exception)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            }
        }

        //body
        #endregion
        [Route("DeleteNearByMePromotionImages2")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteNearByMePromotionImages2(DeleteMultiplePromotionImageDTO dto)
        {
            try
            {
                LogRequest("Image ID for Del: " + dto.imageIds);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                int featuredImageId = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetFeaturedImage(dto.promotionId));


                if ((dto.imageIds + ",").Contains(featuredImageId.ToString() + ","))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = "Featured image cannot be deleted." });

                }
                int operationCompleted = 0;
                var images = dto.imageIds.Split(new char[] { ',' });

                foreach (var imageidStr in images)
                {
                    int imageId = Convert.ToInt32(imageidStr);
                    string fullImageUrl = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetImageById(imageId));
                    string imageUrl = Path.Combine(HostingEnvironment.MapPath("~" + ConfigurationManager.AppSettings["PromotionImagesPath"]), fullImageUrl.Substring(fullImageUrl.LastIndexOf('/') + 1));
                    operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.DeleteNearByMePromotionImage(dto.promotionId, imageId));
                    if (fullImageUrl != "" && System.IO.File.Exists(imageUrl))
                    {
                        if (operationCompleted == 1)
                        {
                            System.IO.File.Delete(imageUrl);
                        }
                    }
                }
                var userPromotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetPromotionById(dto.promotionId));
                return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = userPromotions });
            }
            catch (ApplicationException applicationException)
            {
                return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
            }
            catch (Exception exception)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            }
        }

        //by uzair

        [Route("DeleteNearByMePromotion/{username}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteNearByMePromotion(string username)
        {
            try
            {
                LogRequest("Image ID for Del: " + username);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                DataTable userPromotions = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.GetPromotionImagesByUserName(username));
                bool operationCompleted = false;
                if (userPromotions != null)
                {
                    operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionManager.DeleteNearByMePromotion(username));

                    if (operationCompleted == true)
                    {
                        string imagePath;
                        foreach (DataRow r in userPromotions.Rows)
                        {
                            imagePath = Path.Combine(HostingEnvironment.MapPath("~" + ConfigurationManager.AppSettings["PromotionImagesPath"]), Convert.ToString(r["imagePath"]).Substring(Convert.ToString(r["imagePath"]).LastIndexOf('/') + 1));

                            string str = Convert.ToString(r["imagePath"]).Substring(Convert.ToString(r["imagePath"]).LastIndexOf('/') + 1);
                            str = str.Substring(0, str.IndexOf(""));

                            if (File.Exists(imagePath))
                            {
                                File.Delete(imagePath);
                            }
                        }
                    }
                }
                else
                {
                    operationCompleted = true;
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = operationCompleted });
            }
            catch (ApplicationException applicationException)
            {
                return Request.CreateErrorResponse((HttpStatusCode)Convert.ToInt16(applicationException.Message), NeeoDictionaries.HttpStatusCodeDescriptionMapper[Convert.ToInt16(applicationException.Message)]);
            }
            catch (Exception exception)
            {
                Logger.LogManager.CurrentInstance.ErrorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod().GetType(), exception.Message, exception);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }




    }

   
}
