using System.Net.Mail;
using System.Net;
using api_project.Interfaces;
using api_project.Models;
using api_project.Repositories;

using static api_project.DTO.UserDTOs;
using api_project.DTO;
using System.Collections.Generic;

namespace api_project.Services
{
    public class LotteryService : ILotteryService
    {
        private readonly IGiftRepository _giftRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;
        public LotteryService(IGiftRepository giftRepository, IOrderRepository orderRepository, IConfiguration configuration)
        {
            _giftRepository = giftRepository;
            _orderRepository = orderRepository;
            _configuration = configuration;
        }
        public async Task<IEnumerable<GiftReadDTO>> GiftsWithoutWinners()
        {
             var gifts = await _giftRepository.GetGiftsWhithOutWhiners();
            return gifts.Select(g => new GiftReadDTO
            {
                Id = g.Id,
                GiftName = g.GiftName,
                Price = g.Price,
                ImageUrl = g.ImageUrl,
            }).ToList();
                }
           
      


        public async Task<UserReadDTO?> LotteryForGiftAsync(int giftId)
        {
            var gift = await _giftRepository.GetGiftByIdAsync(giftId);
            if (gift == null) return null;

            var orders = await _orderRepository.GetAllOrdersByGiftAsync(giftId);
            var confirmedOrders = orders.Where(o => o.IsConfirmed).ToList();
            if (!confirmedOrders.Any()) return null;

            var random = new Random();
            var winnerOrder = confirmedOrders[random.Next(confirmedOrders.Count)];
            var winner = winnerOrder.User;
            if (winner == null) return null;
            gift.WinnerId = winner.Id;
            await _giftRepository.SaveChangesAsync();
            //await SendWinnerEmailAsync( Winner.Email, Winner.Name, gift.GiftName); // Added 'await' here to fix CS4014
            return new UserReadDTO
            {
                Id = winner.Id,
                Name = winner.Name,
                Email = winner.Email
            };
        }
        public async Task<decimal> GetTotalIncome()
        {
            var orders = await _orderRepository. GetAllOrdersWithGiftsAsync();
            return orders
                .Sum(o => o.Gift != null ? o.Gift.Price : 0);
        }


        //public async Task SendWinnerEmailAsync(string toEmail, string userName, string giftName)
        //{
        //    var smtpServer = _configuration["EmailSettings:SmtpServer"];
        //    var port = _configuration["EmailSettings:Port"];
        //    var emailUserName = _configuration["EmailSettings:UserName"];
        //    var password = _configuration["EmailSettings:Password"];
        //    var fromEmail = _configuration["EmailSettings:From"];

        //    if (smtpServer == null || port == null || emailUserName == null || password == null || fromEmail == null)
        //    {
        //        throw new InvalidOperationException("Email settings are not properly configured.");
        //    }

        //    var smtpClient = new SmtpClient
        //    {
        //        Host = smtpServer,
        //        Port = int.Parse(port),
        //        EnableSsl = true,
        //        Credentials = new NetworkCredential(emailUserName, password)
        //    };

        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress(fromEmail),
        //        Subject = "זכית בהגרלה!",
        //        Body = $@"
        //שלום {userName},

        //שמחים לבשר לך שזכית בהגרלה!
        //המתנה שלך: {giftName}

        //מזל טוב,
        //צוות ההגרלה
        //",
        //        IsBodyHtml = false
        //    };
        //    mailMessage.To.Add(toEmail);
        //    try
        //    {
        //        await smtpClient.SendMailAsync(mailMessage);
        //    }
        //    catch (SmtpException ex)
        //    {
        //        // לוג / טיפול
        //        throw new Exception("שגיאה בשליחת מייל לזוכה", ex);
        //    }




        //}

    }
}
