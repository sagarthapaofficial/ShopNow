using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopNow.DAL.DomainClasses
{
    public partial class Order
    {
        public Order()
        {
            OrderLineItems = new HashSet<OrderLineItem>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal OrderAmount { get; set; }
        [Required]
        public int UserId { get; set; }

        public virtual ICollection<OrderLineItem> OrderLineItems { get; set; }
    }
}
