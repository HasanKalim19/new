using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeeo.NearByMe.Model
{
   public class NearByMeUserPromotionsPackages
    {
        public int packageId { get; set; }
        public int promotionId { get; set; }
        public short numberOfDays { get; set; }
        public string countryIds { get; set; }
        public bool useUserBalanceForPayment { get; set; }

    }

    public class UserPromotionsPackages
    {
        public int userPromotionsPackageID { get; set; }
        public string countryIds { get; set; }
 
    }
}
