using System;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IPublishEndpoint _publishEndpoint;

        public AuctionConroller(AuctionDbContext context, IMapper mapper,IPublishEndpoint publishEndpoint)
		{
			_context = context;
            this.mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

		[HttpGet]
		public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date)
		{
			var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

			if (!string.IsNullOrEmpty(date))
			{
				query = query.Where(x => x.UpdateAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
			}

			return await query.ProjectTo<AuctionDto>(mapper.ConfigurationProvider).ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
		{
			var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

			if (auction == null) return NotFound();

			return mapper.Map<AuctionDto>(auction);

		}

		[Authorize]
		[HttpPost]
		public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
		{
			var auction = mapper.Map<Auction>(auctionDto);
			//Todo: add current user as seller
			auction.Seller = User.Identity.Name;
			_context.Auctions.Add(auction);
            var newAuction = mapper.Map<AuctionDto>(auction);

            await _publishEndpoint.Publish(mapper.Map<AuctionCreated>(newAuction));

            var result = await _context.SaveChangesAsync() > 0;

			if (!result) return BadRequest("Could not save changes to the DB");

			return CreatedAtAction(nameof(GetAuctionById),new {auction.Id}, newAuction);
		}

		[Authorize]
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto) {

			var auction = await _context.Auctions.Include(x => x.Item)
				.FirstOrDefaultAsync();

			if (auction == null) return NotFound();

			if (auction.Seller != User.Identity.Name) return Forbid();


			auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
			auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;


			var reuslt = await _context.SaveChangesAsync() > 0;
			if (reuslt) return Ok();

            return BadRequest("Update false");
		}
		[Authorize]
		[HttpDelete]
		public async Task<IActionResult> DeleteAuction(Guid id)
		{
			var auction = await _context.Auctions.FindAsync(id);

			if (auction == null) return NotFound();

            if (auction.Seller != User.Identity.Name) return Forbid();

            _context.Auctions.Remove(auction);
			var reuslt = await _context.SaveChangesAsync() > 0;
			if (!reuslt) return BadRequest("Delete false");

			return Ok();
		}

	}
}

