using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Contexts
{
    public class DBContextModel : DbContext
    {


        public DBContextModel(DbContextOptions<DBContextModel> options) : base(options)
        {

        }

        public virtual DbSet<SignUpRequest> SignUpRequest { get; set; }
        public virtual DbSet<Compaign> Compaign { get; set; }
        public virtual DbSet<SubmitProposal> SubmitProposal { get; set; }
        public virtual DbSet<profileInformation> profileInformation { get; set; }
        public virtual DbSet<ProfilePicture> UploadProfilePicture { get; set; }
        public virtual DbSet<PortFolioInfoRequest> PortfolioInfo { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Invitation> Invitation { get; set; }
        public virtual DbSet<hiredCallCenter> hiredCallCenters { get; set; }
        public virtual DbSet<leadSubmisssionQst> _leadSubmisssionQst { get; set; }
        public virtual DbSet<leadSubmisssion> _leadSubmisssion { get; set; }
        public virtual DbSet<EarningNotification> _earningNotification { get; set; }
        public virtual DbSet<UserAllow> UserAllow { get; set; }
        public virtual DbSet<Referral> Referrals { get; set; }
        public virtual DbSet<ContractSign> ContractSign { get; set; }

        public virtual DbSet<ProposalDetails> ProposalDetails { get; set; }
        public virtual DbQuery<ProposalDetails> ProposalDetail { get; set; }
        //public virtual DbSet<ChangePasswordRequest> ChangePassword { get; set; }
        public virtual DbSet<ContractSaleSubmitted> contractSaleSub { get; set; }
        public virtual DbSet<payment> _payment { get; set; }
        public virtual DbSet<tbl_proposals_info> _tbl_proposals_info { get; set; }
        public virtual DbSet<CampaignAnswer> CampaignAnswers { get; set; }
        public virtual DbSet<CampaignQuestion> CampaignQuestion { get; set; }
        public virtual DbSet<FeedbackContract> FeedbackContract { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<StarRating> StarRating { get; set; }
        public virtual DbSet<KeywordsTag> KeywordsTag { get; set; }
        public virtual DbSet<customerStripe> customerStripe { get; set; }
        public virtual DbSet<cardStripe> cardStripe { get; set; }
        public virtual DbSet<ClientPayCampaign> ClientPayCampaign { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SignUpRequest>(entity =>
            {
                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.email)
                    .HasColumnName("email")
                    .IsUnicode(false);
            });
            modelBuilder.Entity<Compaign>(entity =>
            {
                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.name)
                    .HasColumnName("name")
                    .IsUnicode(false);
            });
            modelBuilder.Entity<SubmitProposal>(entity =>
            {
                entity.Property(e => e.proposalID).HasColumnName("proposalID");
            });
            modelBuilder.Entity<ProfilePicture>(entity =>
            {
                entity.Property(e => e.profileId).HasColumnName("profileId");
            });
            modelBuilder.Entity<profileInformation>(entity =>
            {
                entity.Property(e => e.profileID);
            });
            modelBuilder.Entity<PortFolioInfoRequest>(entity =>
            {
                entity.Property(e => e.portFolioID);
            });
            modelBuilder.Entity<Messages>(entity =>
            {
                entity.Property(e => e.messageID);
            });
            modelBuilder.Entity<Invitation>(entity =>
            {
                entity.Property(e => e.invitationID);
            });
            modelBuilder.Entity<hiredCallCenter>(entity =>
            {
                entity.Property(e => e.hiredID);
            });
            modelBuilder.Entity<leadSubmisssion>(entity =>
            {
                entity.Property(e => e.leadSubID);
            });
            modelBuilder.Entity<leadSubmisssionQst>(entity =>
            {
                entity.Property(e => e.leadSubQstID);
            });
            modelBuilder.Entity<EarningNotification>(entity =>
            {
                entity.Property(e => e.earningNotifID);
            });
            modelBuilder.Entity<UserAllow>(entity =>
            {
                entity.Property(e => e.id);
            });
            modelBuilder.Entity<ContractSign>(entity =>
            {
                entity.Property(e => e.ContractID);
            });
            modelBuilder.Entity<ContractSaleSubmitted>(entity =>
            {
                entity.Property(e => e.SaleSubmittedID);
            });
            modelBuilder.Entity<payment>(entity =>
            {
                entity.Property(e => e.payment_id);
            });
            modelBuilder.Entity<tbl_proposals_info>(entity =>
            {
                entity.Property(e => e.purchased_proposals_id);
            });
            modelBuilder.Entity<FeedbackContract>(entity =>
            {
                entity.Property(e => e.feedbackID);
            });
            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.Property(e => e.notificationID);
            });
            modelBuilder.Entity<StarRating>(entity =>
            {
                entity.Property(e => e.RatingId);
            });
            modelBuilder.Entity<KeywordsTag>(entity =>
            {
                entity.Property(e => e.KeywordID);
            });
            modelBuilder.Entity<CampaignQuestion>(entity =>
            {
                entity.Property(e => e.Id);
            });
            modelBuilder.Entity<CampaignAnswer>(entity =>
            {
                entity.Property(e => e.Id);
            });
            modelBuilder.Entity<customerStripe>(entity =>
            {
                entity.Property(e => e.id);
            });
            modelBuilder.Entity<cardStripe>(entity =>
            {
                entity.Property(e => e.id);
            });
            modelBuilder.Entity<ClientPayCampaign>(entity =>
            {
                entity.Property(e => e.payment_campaign_id);
            });
        }
    }
}
