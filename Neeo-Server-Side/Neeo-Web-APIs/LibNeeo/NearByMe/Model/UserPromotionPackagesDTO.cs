using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeeo.NearByMe.Model
{
   public class UserPromotionPackagesDTO
    {

        public long userPromotionsPackageID { get; set; }
        public double consumed { get; set; }
        public double budget { get; set; }
        public int packageId { get; set; }
        public int promotionId { get; set; }
        public DateTime expiryDate { get; set; }
        public int numberOfDays { get; set; }
        public Byte status { get; set; }
        public DateTime createdDate { get; set; }


    }
}
