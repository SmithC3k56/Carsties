using System;
using System.Collections.Generic;
using System.Net.Http;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services
{
	public class AuctionSvcHttpClient
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _config;

        public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<Item>> GetItemsForSearchDb()
        {
            try
            {
                var lastUpdated = await DB.Find<Item, string>()
               .Sort(x => x.Descending(x => x.UpdateAt))
               .Project(x => x.UpdateAt.ToString())
               .ExecuteFirstAsync();
                var baseAuctionURL = _config["AuctionServiceUrl"];
                //_httpClient.BaseAddress = new Uri("http://localhost:7001");
                //var response = await _httpClient.GetAsync("api/auctions?date=" + lastUpdated == null ? "" : lastUpdated);
                var response = await _httpClient.GetAsync("http://localhost:7001/api/auctions?date=");
                var result = await response.Content.ReadFromJsonAsync<List<Item>>();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Item>();
            }
        }
    }
}

