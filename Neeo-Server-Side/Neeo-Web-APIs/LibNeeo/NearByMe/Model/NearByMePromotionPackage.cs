using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeeo.NearByMe.Model
{
    public class NearByMePromotionPackage
    {
        public int packageId { get; set; }
        public int locationId { get; set; }
        public string description { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }
        public string PackageTitle { get; set; }
        public decimal price { get; set; }
        public Boolean enabled { get; set; }
        public Boolean isDeleted { get; set; }
    }

    public class GetNearByMePromotionPackages
    {
        public int packageId { get; set; }
        public string description { get; set; }
        public string PackageTitle { get; set; }
        public decimal perDayCost { get; set; }

    }

    //public class GetNearByMePromotionPackages
    //{
    //    public int packageId { get; set; }
    //    public string description { get; set; }
    //    public string PackageTitle { get; set; }
    //    public decimal perDayCost { get; set; }

    //}
}
