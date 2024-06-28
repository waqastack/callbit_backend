using Api.Contexts;
using Api.Models;

using CallBit.Emails;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services
{
    public class AffiliatesService
    {
        DBContextModel db;

        public AffiliatesService(DBContextModel _db)
        {
            this.db = _db;
        }

        public List<Referral> InviteByEmail(List<Referral> referrals)
        {
            db.Referrals.AddRange(referrals);
            db.SaveChanges();
            return referrals;
        }
        public List<ReferralEarnings> GetInvited(string email)
        {
            var user = db.SignUpRequest.FirstOrDefault(x => x.email == email);
            var refferals = db.Referrals.Where(x => x.ReferredBy.email == email).ToList();
            var refferedUsers = db.SignUpRequest.Where(x => refferals.Select(y => y.Email).ToList().Contains(x.email)).ToList();
            List<ReferralEarnings> referralEarnings = new List<ReferralEarnings>();
            refferals.ForEach(x => {
                var refUser = db.SignUpRequest.FirstOrDefault(r => r.email == x.Email);
                double? earnings = 0;
                x.Status = "Sent";
                if (refUser != null)
                {
                    x.Status = "Joined";
                    var camps = db.Compaign.Where(c => c.userid == refUser.id && c.isActive == true);
                    earnings = camps.Count() > 0 ? (camps.Sum(c => c.totalAmount) * 0.03) : 0;

                }
                referralEarnings.Add(new ReferralEarnings
                {
                    Referral = x,
                    Earnings = earnings.HasValue ? earnings.Value : 0
                });
            });
            return referralEarnings;
        }
    }
}
