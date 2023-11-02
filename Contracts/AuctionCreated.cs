using System;
namespace Contracts
{
	public class AuctionCreated
	{

        public Guid Id { get; set; }
        public int ReservePrice { get; set; }
        public string Seller { get; set; } = String.Empty;
        public string Winner { get; set; } = String.Empty;
        public int? SoldAmount { get; set; }
        public int? CurrentHighBid { get; set; }
        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public DateTime AuctionEnd { get; set; }
        public string Status { get; set; } = string.Empty;

        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Color { get; set; } = string.Empty;
        public int Mileage { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}

