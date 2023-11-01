using System;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
	[ApiController]
	[Route("api/auctions")]
	public class AuctionConroller: ControllerBase
	{
		private readonly AuctionDbContext _context;
        private readonly IMapper mapper;

        public AuctionConroller(AuctionDbContext context, IMapper mapper)
		{
			_context = context;
            this.mapper = mapper;
        }

		[HttpGet]
		public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
		{
			var auctions = await _context.Auctions
				.Include(x => x.Item)
				.OrderBy(x => x.Item.Make)
				.ToListAsync();

			return mapper.Map<List<AuctionDto>>(auctions);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
		{
			var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

			if (auction == null) return NotFound();

			return mapper.Map<AuctionDto>(auction);
		}
		[HttpPost]
		public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
		{
			var auction = mapper.Map<Auction>(auctionDto);
			//Todo: add current user as seller
			auction.Seller = "Test";
			_context.Auctions.Add(auction);

			var result = await _context.SaveChangesAsync() > 0;
			if (!result) return BadRequest("Could not save changes to the DB");

			return CreatedAtAction(nameof(GetAuctionById),new {auction.Id}, mapper.Map<AuctionDto>(auction));
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto) {

			var auction = await _context.Auctions.Include(x => x.Item)
				.FirstOrDefaultAsync();

			if (auction == null) return NotFound();

			//Todo: check seller == username

			auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
			auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;


			var reuslt = await _context.SaveChangesAsync() > 0;
			if (reuslt) return Ok();

            return BadRequest("Update false");
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteAuction(Guid id)
		{
			var auction = await _context.Auctions.FindAsync(id);

			if (auction == null) return NotFound();

			//todo: check seller == username
			_context.Auctions.Remove(auction);
			var reuslt = await _context.SaveChangesAsync() > 0;
			if (!reuslt) return BadRequest("Delete false");

			return Ok();
		}

	}
}

