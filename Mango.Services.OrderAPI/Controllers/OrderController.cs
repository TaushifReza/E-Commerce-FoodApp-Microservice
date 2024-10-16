﻿using AutoMapper;
using Mango.MessageBus;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Service.IService;
using Mango.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        protected ResponseDto _response;

        public OrderController(AppDbContext context, IProductService productService, IMapper mapper, IMessageBus messageBus, IConfiguration configuration)
        {
            _context = context;
            _productService = productService;
            _mapper = mapper;
            _messageBus = messageBus;
            _configuration = configuration;
            this._response = new ResponseDto();
        }

        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.StatusPending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);

                OrderHeader orderCreated = _context.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _context.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDto;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto dto)
        {
            try
            {
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = dto.ApprovedUrl,
                    CancelUrl = dto.Cancelurl,
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                };

                var DiscountObj = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions
                    {
                        Coupon = dto.OrderHeader.CouponCode
                    }
                };

                foreach (var item in dto.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "NPR",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.Count
                    };

                    options.LineItems.Add(sessionLineItem);
                }

                if (dto.OrderHeader.Discount > 0)
                {
                    options.Discounts = DiscountObj;
                }
                var service = new Stripe.Checkout.SessionService();
                Session session = service.Create(options);
                dto.StripeSessionUrl = session.Url;
                OrderHeader orderHeader =
                    await _context.OrderHeaders.FirstAsync(u => u.OrderHeaderId == dto.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                await _context.SaveChangesAsync();

                _response.Result = dto;
            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = await _context.OrderHeaders.FirstAsync(u => u.OrderHeaderId == orderHeaderId);


                var service = new Stripe.Checkout.SessionService();
                Session session = await service.GetAsync(orderHeader.StripeSessionId);

                var paymentIntentService = new Stripe.PaymentIntentService();
                var paymentIntent = await paymentIntentService.GetAsync(session.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                     // then payment was successful
                     orderHeader.PaymentIntentId = paymentIntent.Id;
                     orderHeader.Status = SD.StatusApproved;
                     await _context.SaveChangesAsync();

                     RewardsDto rewardsDto = new RewardsDto()
                     {
                         OrderId = orderHeader.OrderHeaderId,
                         RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal),
                         UserId = orderHeader.UserId
                     };
                     string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
                     await _messageBus.PublishMessage(rewardsDto, topicName);

                     _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                }
            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
