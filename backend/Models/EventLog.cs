using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class EventLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Product")]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        public EventType Event { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual Product Product { get; set; }
    }

    public enum EventType
    {
        list_view,
        view,
        add_to_cart,
        checkout
    }
    
}