using System.ComponentModel.DataAnnotations.Schema;

namespace PromoWeb.Context.Entities
{
    public class Image : BaseEntity
    {
        public string ImageName { get; set; }
        public string Description { get; set; }

        public int AppInfoId { get; set; }
        public virtual AppInfo AppInfo { get; set; }

        public string ImagePath { get; set; }
    }
}
