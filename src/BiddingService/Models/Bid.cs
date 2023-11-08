using MongoDB.Entities;

namespace BiddingService.Models;

public class Bid: Entity
{
    public string AuctionId { get; set; }
    public string Bidder { get; set; }
    public DateTime BidDate { get; set; }= DateTime.UtcNow;
    public int Amount { get; set; }
    // public BidSatus BidStatus { get; set; }
}
