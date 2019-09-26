using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeeo.NearByMe.Model
{
    public class DeletePromotionImageDTO
    {
        public int promotionId { get; set; }
        public long imageId { get; set; }
 
    }

    public class DeleteMultiplePromotionImageDTO
    {
        public int promotionId { get; set; }
        public string imageIds { get; set; }

    }
}
