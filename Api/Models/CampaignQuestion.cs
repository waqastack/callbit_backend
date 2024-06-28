using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
	[Table("CampaignQuestion")]
	public class CampaignQuestion
	{
		[Key]
		public int Id { get; set; }
		public string Question { get; set; }
		public int Compaignid { get; set; }
	}
}
