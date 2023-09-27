using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopNow.DAL.DomainClasses
{
    public partial class OrderLineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int QtyOrdered { get; set; }
        [Required]
        public int QtySold { get; set; }
        [Required]
        public int QtyBackOrdered { get; set; }
        [Required]
        public decimal SellingPrice { get; set; }
        [Required]
        public string ProductId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; } = null!;
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
