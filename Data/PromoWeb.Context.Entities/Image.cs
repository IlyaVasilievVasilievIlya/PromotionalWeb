using System.ComponentModel.DataAnnotations.Schema;

namespace PromoWeb.Context.Entities
{
    public class Image : BaseEntity //как это хранить?
    {
        public string Description { get; set; }

        public int AppInfoId { get; set; }
        public virtual AppInfo AppInfo { get; set; }
        public byte[] Bytes { get; set; }
        public string FileExtension { get; set; }
    }
}
