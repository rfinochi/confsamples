using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSamples
{
    public static class Transformations
    {
        #region Select

        public static ClaimSummary[] FindClaimsFor(string recipientId)
        {
            var claims = GetClaimsForRecipient(recipientId);
            var claimSummaries = new List<ClaimSummary>();

            foreach (Claim claim in claims)
            {
                claimSummaries.Add(new ClaimSummary
                {
                    RecipientId = claim.ClaimRecipient.RecipientId,
                    ClaimDate = claim.ClaimDate,
                    ClaimTotal = claim.GetClaimTotal()
                });
            }

            return claimSummaries.ToArray();
        }

        public static ClaimSummary[] FindClaimsForLinq(string recipientId)
        {
            return GetClaimsForRecipient(recipientId)
                .Select(claim => new ClaimSummary
                {
                    RecipientId = claim.ClaimRecipient.RecipientId,
                    ClaimDate = claim.ClaimDate,
                    ClaimTotal = claim.GetClaimTotal()
                })
                .ToArray();
        }

        public static ClaimSummary[] FindClaimsForExpression(string recipientId)
        {
            return (from claim in GetClaimsForRecipient(recipientId)
                   select new ClaimSummary
                    {
                        RecipientId = claim.ClaimRecipient.RecipientId,
                        ClaimDate = claim.ClaimDate,
                        ClaimTotal = claim.GetClaimTotal()
                    }).ToArray();
        }

        #endregion

        #region SelectMany

        public static ClaimLineSummary[] FindExpensiveClaimsFor(string recipientId)
        {
            var claims = GetClaimsForRecipient(recipientId);
            var claimLineSummaries = new List<ClaimLineSummary>();

            foreach (Claim claim in claims)
            {
                foreach (var line in claim.ClaimLines)
                {
                    if (line.ClaimPrice > 50.0)
                    {
                        claimLineSummaries.Add(new ClaimLineSummary
                        {
                           RecipientId = claim.ClaimRecipient.RecipientId,
                           ClaimDate = claim.ClaimDate,
                           ClaimPrice = line.ClaimPrice
                        });
                    }
                }
            }

            return claimLineSummaries.ToArray();
        }

        public static ClaimLineSummary[] FindExpensiveClaimsForLinq(string recipientId)
        {
            return GetClaimsForRecipient(recipientId)
                .SelectMany(claim => TransformClaimLines(claim))
                .Where(line => line.ClaimPrice > 50.0)
                .ToArray();
        }

        public static ClaimLineSummary[] FindExpensiveClaimsForExpression(string recipientId)
        {
            return (from claim in GetClaimsForRecipient(recipientId)
                    from line in TransformClaimLines(claim)
                    where line.ClaimPrice > 50.0
                    select line).ToArray();
        }

        #endregion

        private static IEnumerable<ClaimLineSummary> TransformClaimLines(Claim claim)
        {
            return from line in claim.ClaimLines
                   select new ClaimLineSummary
                   {
                       RecipientId = claim.ClaimRecipient.RecipientId,
                       ClaimDate = claim.ClaimDate,
                       ClaimPrice = line.ClaimPrice
                   };
        }

        private static IEnumerable<Claim> GetClaimsForRecipient(string recipientId)
        {
            return new List<Claim>
           {
               new Claim { ClaimDate = DateTime.Now, ClaimId = Guid.NewGuid().ToString(), 
                   ClaimRecipient = new Recipient { RecipientId = recipientId },
                   ClaimLines = new List<ClaimLine>
                    {
                        new ClaimLine { ClaimPrice = 0.5 },
                        new ClaimLine { ClaimPrice = 1.5 },
                        new ClaimLine { ClaimPrice = 99.55 }
                    }
               },
               new Claim { ClaimDate = DateTime.Now, ClaimId = Guid.NewGuid().ToString(), 
                   ClaimRecipient = new Recipient { RecipientId = recipientId },
                   ClaimLines = new List<ClaimLine>
                    {
                        new ClaimLine { ClaimPrice = 0.5 },
                        new ClaimLine { ClaimPrice = 1.5 },
                        new ClaimLine { ClaimPrice = 85.2 }
                    }
               },
               new Claim { ClaimDate = DateTime.Now, ClaimId = Guid.NewGuid().ToString(), 
                   ClaimRecipient = new Recipient { RecipientId = recipientId },
                   ClaimLines = new List<ClaimLine>
                    {
                        new ClaimLine { ClaimPrice = 0.5 },
                        new ClaimLine { ClaimPrice = 1.5 },
                        new ClaimLine { ClaimPrice = 25.44 }
                    }
               }
           };
        }

    }


}
