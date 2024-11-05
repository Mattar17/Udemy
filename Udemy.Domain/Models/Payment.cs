using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Domain.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentMethodId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ApplicationUser Buyer { get; set; }
        public string BuyerId { get; set; }

        public Course Course { get; set; }
        public string CourseId { get; set; }

    }
}
