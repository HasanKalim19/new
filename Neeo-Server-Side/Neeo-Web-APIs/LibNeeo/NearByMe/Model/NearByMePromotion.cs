using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeeo.NearByMe.Models
{
    public class NearByMePromotion
    {
        public int promotionId { get; set; }
        public string username { get; set; }
        public string description { get; set; }
        public Byte status { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }
        public List<NearByMePromotionImage> ImagesXml { get; set; }
    }
}
