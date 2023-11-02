using System;
namespace Contracts
{
	public class AuctionUpdated
	{

        public string Id { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public int? Year { get; set; }

        public string Color { get; set; } = string.Empty;

        public int? Mileage { get; set; }
    }
}

