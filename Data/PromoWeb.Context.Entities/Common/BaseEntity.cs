using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PromoWeb.Context.Entities
{
    /// <summary>
    /// Base entity
    /// </summary>
    [Index("Uid", IsUnique = true)]
    public abstract class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        [Required]
        public virtual Guid Uid { get; set; } = Guid.NewGuid();
    }
}
