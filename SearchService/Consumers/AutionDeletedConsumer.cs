using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
	public class AutionDeletedConsumer : IConsumer<AuctionDeleted>
    {
        private readonly IMapper _mapper;

        public AutionDeletedConsumer(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            Console.WriteLine("--> Consuming auction created: " + context.Message.Id);

            var item = await DB.DeleteAsync<Item>(context.Message.Id);
            if (!item.IsAcknowledged) throw new MessageException(typeof(AuctionDeleted),"Problem deleting auction");
           
        }
    }
}

