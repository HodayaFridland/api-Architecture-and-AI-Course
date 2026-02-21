using System.ComponentModel.DataAnnotations;
using api_project.DTO;
using api_project.Interfaces;
using api_project.Models;
using api_project.Repositories;



namespace api_project.Services
{
    public class DonorService : IDonorService

    {
        private readonly IDonorRepository _donorRepository;


        public DonorService(IDonorRepository donorRepository)
        {
            _donorRepository = donorRepository;
        }
        
        public async Task<List<DonorReadDTO>> GetAllDonorsAsync(string? name, string? email, string? giftName)
        {

            return (await _donorRepository.GetDonorsAsync( name,  email,  giftName))
                .Select(d => new DonorReadDTO
                {
                    Id = d.Id,
                    DonorName = d.DonorName,
                    Email = d.Email
                }).ToList();
        }
        public async Task<DonorGiftsReadDTO?> GetDonorByIdAsync(int id)
        {
            var donor = await _donorRepository.GetDonorByIdAsync(id);
            if (donor == null) return null;

            return new DonorGiftsReadDTO
            {
                Id = donor.Id,
                DonorName = donor.DonorName,
                Email = donor.Email,
                Gifts = donor.Gifts.Select(g => new GiftReadDTO
                {
                    Id = g.Id,
                    GiftName = g.GiftName,
                    Price = g.Price
                }).ToList()
            };
        }
        //הוספת תורם חדש
        public async Task<DonorReadDTO> CreateDonorAsync(DonorCreateDTO DonorCreateDTO)
        {
            if (await _donorRepository.EmailExistsAsync(DonorCreateDTO.Email))
            {
                throw new ArgumentException($"Email {DonorCreateDTO.Email} is already registered.");
            }
            var donor = new Donor
            {
                DonorName = DonorCreateDTO.DonorName,
                Email = DonorCreateDTO.Email
            };

            var createdDonor = await _donorRepository.CreateDonorAsync(donor);


            return new DonorReadDTO
            {
                Id = createdDonor.Id,
                DonorName = createdDonor.DonorName,
                Email = createdDonor.Email
            };
        }

        public async Task<DonorReadDTO?> UpdateDonorAsync(int id, DonorUpdateDTO updateDonor)
        {
            var existingDonor = await _donorRepository.GetDonorByIdAsync(id);
            if (existingDonor == null) return null;

            if (updateDonor.Email != null && updateDonor.Email != existingDonor.Email)
            {
                if (await _donorRepository.EmailExistsAsync(updateDonor.Email))
                {
                    throw new ArgumentException($"Email {updateDonor.Email} is already registered.");
                }
                existingDonor.Email = updateDonor.Email;
            }

            if (updateDonor.DonorName != null) existingDonor.DonorName = updateDonor.DonorName;

            var updatedDonor = await _donorRepository.UpdateAsync(existingDonor);
            if (updatedDonor == null) return null;
            return new DonorReadDTO
            {
                Id = updatedDonor.Id,
                DonorName = updatedDonor.DonorName,
                Email = updatedDonor.Email
            };
        }

        public async Task<bool> DeleteDonorAsync(int id)
        {
            return await _donorRepository.DeleteAsync(id);
        }



    }
}
