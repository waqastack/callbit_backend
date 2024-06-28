using Api.Models;
using Api.Services;

using CallBit.Emails;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AffiliatesController : BaseController
    {
        IDBService _DBService;
        AffiliatesService _AffiliatesService;

        public AffiliatesController(IDBService dBService, AffiliatesService affiliatesService)
        {
            _DBService = dBService;
            _AffiliatesService = affiliatesService;
        }

        [HttpPost]
        [Route("inviteByEmailList")]
        public async Task<IActionResult> InviteByEmailList(List<string> emails)
        {
            System.Diagnostics.Debug.WriteLine(base._GetEmail());
            var user = await _DBService.GetUser(_GetEmail());
            if (user == null)
            {
                return Unauthorized();
            }
            var referrals = new List<Referral>();
            emails.ForEach(email =>
            {
                if (_DBService.IsEmailUsed(email))
                    throw new Exception("One or emails are already in use");
                referrals.Add(new Referral
                {
                    ReferredBy = user,
                    Email = email,
                    ReferredOn = DateTime.UtcNow,
                    Status = "Sent"
                });
            });
            if (referrals.Count > 0)
            {
                CallBitEmail email = new CallBitEmail();

                if (email.SendEmail(referrals.Select(x => x.Email).ToList()))
                {
                    var responseReferrals = _AffiliatesService.InviteByEmail(referrals);
                    return Ok(responseReferrals);
                }
            }
            return NoContent();
        }

        [HttpGet]
        [Route("GetInvited")]
        public IActionResult GetInvited()
        {
            return Ok(_AffiliatesService.GetInvited(_GetEmail()));
        }
    }
}
