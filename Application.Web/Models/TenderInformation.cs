using System.Collections.Generic;

namespace Application.Web.Models
{
    public class TenderInformation
    {
        public string Owner { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public IEnumerable<string> BuyPrice { get; set; }
        public string ProposePrice { get; set; }
        public string Winner { get; set; }
        public string Amount { get; set; }
    }
}