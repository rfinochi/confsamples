using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSamples
{
    public class Claim
    {
        public string ClaimId { get; set; }

        public DateTime ClaimDate { get; set; }

        public Recipient ClaimRecipient { get; set; }

        public IEnumerable<ClaimLine> ClaimLines { get; set; }

        public double GetClaimTotal()
        {
            double amount = 0.0;
            foreach (var line in ClaimLines)
                amount += line.ClaimPrice;

            return amount;
        }

        public double GetClaimTotalLinq()
        {
            return ClaimLines.Sum(line => line.ClaimPrice);
        }
    }

    public class Recipient
    {
        public string RecipientId { get; set; }
    }

    public class ClaimSummary
    {
        public string RecipientId { get; set; }

        public DateTime ClaimDate { get; set; }

        public double ClaimTotal { get; set; }
    }

    public class ClaimLine
    {
        public string ClaimCode { get; set; }
        public double ClaimPrice { get; set; }
    }

    public class ClaimLineSummary
    {
        public string RecipientId { get; set; }
        public DateTime ClaimDate { get; set; }
        public double ClaimPrice { get; set;  }
    }
}
