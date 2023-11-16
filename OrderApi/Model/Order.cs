using System.ComponentModel.DataAnnotations;

namespace OrderApi.Model
{
    public class Order
    {
        [Range(0, int.MaxValue)]
        public int OrderId { get; set; }
        [Required]
        public string CustomerName { get; set; } = "";
    }

}
