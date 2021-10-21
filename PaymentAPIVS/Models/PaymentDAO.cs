using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentAPI.Models
{
    public class PaymentDAO
    {
        [Key]
        public int PaymentDetailId { get; set; }
        [Required]
        public string CardOwnerName { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string ExpirationDate { get; set; }
        [Required]
        public string SecurityCode { get; set; }
    }
}