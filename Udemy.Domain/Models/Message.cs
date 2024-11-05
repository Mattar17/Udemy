using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        //public string SenderId { get; set; }
        //public ApplicationUser SenderUser { get; set; }
        //public string ReceiverId { get; set; }
        //public ApplicationUser ReceiverUser { get; set; }
    }
}
