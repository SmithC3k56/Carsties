using System;
using System.Net.NetworkInformation;

namespace AuctionService.Entities
{
	public class Auction
	{
		public Guid Id { get; set;}
		public int  ReservePrice { get; set; }
		public string Seller { get; set; } = String.Empty;
		public string Winner { get; set; } = String.Empty;
		public int? SoldAmount { get; set; }
		public int? CurrentHighBid { get; set; }
		public DateTime CreateAt { get; set; } 

        public DateTime UpdateAt { get; set; } 

        public DateTime AuctionEnd { get; set; } 
		public Status Status { get; set; }
        public Item? Item { get; set; }
    }
}

 