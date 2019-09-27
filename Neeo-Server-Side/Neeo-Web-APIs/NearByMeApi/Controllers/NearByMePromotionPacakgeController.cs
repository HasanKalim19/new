using Compression;
using LibNeeo.NearByMe.Model;
using PowerfulPal.Neeo.NearByMeApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Common.Controllers;

namespace PowerfulPal.Neeo.NearByMeApi.Controllers
{
    [RoutePrefix("api/v1/near-by-me-promotion-package")]
    public class NearByMePromotionPacakgeController : NeeoApiController
    {
     
        NearByMePackageManager nearByMePromotionPacakge = new NearByMePackageManager();

        [HttpGet]
        [Route("GetAllPromotionPackages")]
        public async Task<HttpResponseMessage> GetNearByMePromotionPacakages()
        {
            try
            {

             LogRequest("GetNearByMePromotionPacakages");
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                List<GetNearByMePromotionPackages> pacakages = await System.Threading.Tasks.Task.Run(() => nearByMePromotionPacakge.GetNearByMePromotionPackages());
                return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = pacakages } );
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

        [HttpGet]
        [Route("GetUserBalance/{username}")]
        public async Task<HttpResponseMessage> GetUserBalance([FromUri]string username)
        {
            try
            {

                LogRequest("GetUserBalance" + username);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                decimal balance = await System.Threading.Tasks.Task.Run(() => nearByMePromotionPacakge.GetUserBalance(username));
                return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = balance } );
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

        [HttpGet]
        [Route("GetUserPromotionPackages/{username}")]
        public async Task<HttpResponseMessage> GetUserPromotionPackages([FromUri]string username)
        {
            try
            {

                LogRequest("GetUserPromotionPackages" + username);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

               
                List<UserPromotionPackagesDTO> packages = await System.Threading.Tasks.Task.Run(() => nearByMePromotionPacakge.GetUserPromotionPackages(username));
                return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = packages } );
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

        [HttpPost]
        [Route("AddUserPromotionPackage")]
        public async Task<HttpResponseMessage> AddUserPromotionPackage([FromBody] NearByMeUserPromotionsPackages promotion)
        {
            try
            {
              
                LogRequest(promotion);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                if(promotion.useUserBalanceForPayment == true)
                {
                    int ret = -5;
                    return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = ret } );
                }

                #region SPLIT 
                //var country = promotion.countryIds.Split(new char[] { ',' });
                //if (country.Count() > 5)
                //{
                //    return Request.CreateResponse(HttpStatusCode.BadRequest, new { returnValue = "A package cannot be bound with more than five countries." });
                //}
                #endregion

                long operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionPacakge.AddUserPromotionPackage(promotion.packageId, promotion.promotionId, promotion.numberOfDays, promotion.countryIds));

                if (operationCompleted < 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = operationCompleted } );
                }
                else
                {

                    List<UserPromotionPackagesDTO> packages = await System.Threading.Tasks.Task.Run(() => nearByMePromotionPacakge.GetUserPromotionPackages(operationCompleted.ToString()));

                    return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = packages } );
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
        [Route("UpdateUserPromotionPackage")]
        public async Task<HttpResponseMessage> UpdateUserPromotionPackage([FromBody] UserPromotionsPackages package)
        {
            try
            {

                LogRequest(package);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }



                long operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionPacakge.UpdateUserPromotionPackage(package.userPromotionsPackageID, package.countryIds));
                if (operationCompleted < 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = operationCompleted });
                }
                else
                {

                    List<UserPromotionPackagesDTO> packages = await System.Threading.Tasks.Task.Run(() => nearByMePromotionPacakge.GetUserPromotionPackages(operationCompleted.ToString()));

                    return Request.CreateResponse(HttpStatusCode.OK, new { returnValue = packages });
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

        [Route("DeleteNearByMeUserPromotionPackage/{userPromotionsPackageID}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteNearByMePromotion(int userPromotionsPackageID)
        {
            try
            {
                LogRequest("userPromotionsPackageID ID for Del: " + userPromotionsPackageID);
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

               int operationCompleted = await System.Threading.Tasks.Task.Run(() => nearByMePromotionPacakge.DeleteNearByMeUserPromotionPackage(userPromotionsPackageID));

                   
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
