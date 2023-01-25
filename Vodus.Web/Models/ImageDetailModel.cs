using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vodus.Web.Models
{
    public class ImageDetailModel
    {
        public string page { get; set; }
        public string promoTitle { get; set; }
        public string promoDescription { get; set; }
        public string TandC { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string imageUrl { get; set; }
    }

    public class VodusFileInfo
    {
        public string fileUrl { get; set; }
    }

    public class VodusSearchInfo
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
