using api_project.DTO;
using api_project.Interfaces;
using api_project.Models;

namespace api_project.Services
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepository _giftRepository;
        public GiftService(IGiftRepository giftRepository)
        {
            _giftRepository = giftRepository;
        }

        public async Task<IEnumerable<GiftReadDTO?>> GetAllGiftsAsync()
        {
            var gifts = await _giftRepository.GetAllGiftsAsync();
            return gifts.Select(g => new GiftReadDTO
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Price = g.Price,
                ImageUrl = g.ImageUrl,
                DonorId=g.DonorId,
                CategoryName=g.CategoryName,
                Descriptoin=g.Description

            }).ToList();
        }

        public async Task<GiftReadDTO?> GetGiftByIdAsync(int id)
        {
            var gift = await _giftRepository.GetGiftByIdAsync(id);
            if (gift == null) return null;
            return new GiftReadDTO
            {
                Id = gift.Id,
                GiftName = gift.GiftName,
                Price = gift.Price,
                ImageUrl = gift.ImageUrl,
                DonorId = gift.DonorId,
                CategoryName = gift.CategoryName
            };
        }

        public async Task<GiftReadDTO> CreateGiftAsync(GiftCreateDTO giftCreateDTO)
        {
            var gift = new Gift
            {
                GiftName = giftCreateDTO.GiftName,
                Price = giftCreateDTO.Price,
                Description = giftCreateDTO.Description,
                ImageUrl = giftCreateDTO.ImageUrl,
                CategoryName = giftCreateDTO.CategoryName,
                DonorId = giftCreateDTO.DonorId
            };
            var createdGift = await _giftRepository.CreateGift(gift);
            return new GiftReadDTO
            {
                Id = createdGift.Id,
                GiftName = createdGift.GiftName,
                Price = createdGift.Price,
                ImageUrl = createdGift.ImageUrl
            };
        }

        public async Task<GiftReadDTO?> UpdateGiftAsync(int id, GiftUpdateDto giftUpdateDTO)
        {
            var existingGift = await _giftRepository.GetGiftByIdAsync(id);
            if (existingGift == null) return null;

            existingGift.GiftName = giftUpdateDTO.GiftName;
            existingGift.Price = giftUpdateDTO.Price;
            existingGift.Description = giftUpdateDTO.Description;
            existingGift.ImageUrl = giftUpdateDTO.ImageUrl;

            if (existingGift.CategoryName != null)
                existingGift.CategoryName = giftUpdateDTO.CategoryName;

            if(existingGift.WinnerId!=null)
            existingGift.WinnerId = giftUpdateDTO.WinnerId;

            var updatedGift = await _giftRepository.UpdateAsync(existingGift);
            if (updatedGift == null) return null;

            return new GiftReadDTO
            {
                Id = updatedGift.Id,
                GiftName = updatedGift.GiftName,
                Price = updatedGift.Price,
                ImageUrl = updatedGift.ImageUrl
            };
        }

        public async Task<bool> DeleteGiftAsync(int id)
        {
            return await _giftRepository.DeleteAsync(id);
        }

        public async Task<DonorReadDTO?> GetDonorByGiftIdAsync(int giftId)
        {
            var donor = await _giftRepository.GetDonorByGiftIdAsync(giftId);
            if (donor == null) return null;
            return new DonorReadDTO
            {
                Id = donor.Id,
                DonorName = donor.DonorName,
                Email = donor.Email
            };
        }

        public async Task<IEnumerable<GiftReadDTO?>> GetGiftByDonorNameAsync(string donorName)
        {
            var gifts = await _giftRepository.GetGiftByDonorName(donorName);
            return gifts.Select(g => new GiftReadDTO
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Price = g.Price,
                ImageUrl = g.ImageUrl,
                CategoryName=g.CategoryName,
                DonorId=g.DonorId
            }).ToList();
        }

        public async Task<IEnumerable<GiftReadDTO?>> GetGiftByNumOfPurchasesAsync(int numOfPurchases)
        {
            var gifts = await _giftRepository.GetGiftByNumOfPurchases(numOfPurchases);
            return gifts.Select(g => new GiftReadDTO
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Price = g.Price,
                ImageUrl = g.ImageUrl
            }).ToList();
        }

        public async Task<decimal> GetPriceByGiftIdAsync(int giftId)
        {
            return await _giftRepository.GetPriceByGiftName(giftId);
        }

        public async Task<GiftReadDTO?> GetGiftByNameAsync(string giftName)
        {
            var gift = await _giftRepository.GetGiftByName(giftName);
            if (gift == null) return null;
            return new GiftReadDTO
            {
                Id = gift.Id,
                GiftName = gift.GiftName,
                Price = gift.Price,
                ImageUrl = gift.ImageUrl,
                DonorId = gift.DonorId,
                CategoryName = gift.CategoryName
            };
        }
    }
}
