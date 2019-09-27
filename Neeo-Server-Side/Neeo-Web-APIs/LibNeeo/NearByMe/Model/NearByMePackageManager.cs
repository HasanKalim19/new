using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeeo.NearByMe.Model
{
    public  class NearByMePackageManager
    {

        private DbManager _dbManager = new DbManager();

        public async Task<List<GetNearByMePromotionPackages>> GetNearByMePromotionPackages()
        {
            List<GetNearByMePromotionPackages> pacakages = new List<GetNearByMePromotionPackages>();
            DataTable dtNearbyMePromotion = await System.Threading.Tasks.Task.Run(() => _dbManager.GetNearByMePromotionPackages());

            pacakages = (from row in dtNearbyMePromotion.AsEnumerable()
                         select new GetNearByMePromotionPackages()
                         {
                             packageId = Convert.ToInt32(row["packageId"]),
                             description = Convert.ToString(row["description"]),
                             PackageTitle = Convert.ToString(row["PackageTitle"]),
                             perDayCost = Convert.ToDecimal(row["perDayCost"]),

                         }).ToList();
            return pacakages;
        }

        public async Task<List<UserPromotionPackagesDTO>> GetUserPromotionPackages(string username)
        {
            List<UserPromotionPackagesDTO> pacakages = new List<UserPromotionPackagesDTO>();
            DataTable dtUserPromotionPackages = await System.Threading.Tasks.Task.Run(() => _dbManager.GetUserPromotionPackages(username));

            pacakages = (from row in dtUserPromotionPackages.AsEnumerable()
                         select new UserPromotionPackagesDTO()
                         {
                             userPromotionsPackageID = Convert.ToInt64(row["userPromotionsPackageID"]),
                             consumed = Convert.ToDouble(row["consumed"]),
                             budget = Convert.ToDouble(row["budget"]),
                             packageId = Convert.ToInt32(row["packageId"]),
                             promotionId = Convert.ToInt32(row["promotionId"]),
                             expiryDate = Convert.ToDateTime(row["expiryDate"]),
                             numberOfDays = Convert.ToInt32(row["numberOfDays"]),
                             status =Convert.ToByte(row["status"]),
                             createdDate = Convert.ToDateTime(row["createdDate"]),

                          }).ToList();
                          return pacakages;
          }

        public async Task<long> AddUserPromotionPackage(int packageId, int promotionId, short numberOfDays, string country)
        {
            


            return await System.Threading.Tasks.Task.Run(() => _dbManager.AddUserPromotionPackage(packageId, promotionId, numberOfDays, country));
        }

        public async Task<int> DeleteNearByMeUserPromotionPackage(int userPromotionsPackageID)
        {
            return await System.Threading.Tasks.Task.Run(() => _dbManager.DeleteNearByMeUserPromotionPackage(userPromotionsPackageID));
        }

        public async Task<decimal> GetUserBalance(string username)
        {
            return await System.Threading.Tasks.Task.Run(() => _dbManager.GetUserBalance(username));
        }

        public async Task<long> UpdateUserPromotionPackage(int userPromotionsPackageID,  string countryIds)
        {

            return await System.Threading.Tasks.Task.Run(() => _dbManager.UpdateUserPromotionPackage(userPromotionsPackageID, countryIds));
        }

    }
}
