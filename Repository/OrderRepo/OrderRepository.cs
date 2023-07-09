using Microsoft.EntityFrameworkCore;
using Shipping_System.Data;
using Shipping_System.Models;
using System;

namespace Shipping_System.Repository.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Order> GetAll()
        {
            return _context.Orders.
                Where(e => e.IsDeleted == false)
                .Include(o => o.Representative)
                .Include(o=>o.ClientGovernorate)
                .Include(o=>o.ClientCity)
                .Include(o=>o.OrderState)
                .Include(o=>o.DeliverType)
                .Include(o=>o.OrderType)
                .Include(o=>o.Branch)
                .Include(t => t.Trader)
                .ThenInclude(t=> t.AppUser)
                .ToList();
        }


        public Order GetById(int id)
        {
            return _context.Orders
                .Include(o => o.Representative)
                .Include(o => o.ClientGovernorate)
                .Include(o => o.ClientCity)
                .Include(o => o.OrderState)
                .Include(o => o.DeliverType)
                .Include(o => o.OrderType)
                .Include(o => o.Branch).FirstOrDefault(e => e.Id == id && e.IsDeleted == false)!;
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public void Edit(Order order)
        {
            _context.Orders.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void Delete(int id)
        {
            Order order = _context.Orders.Find(id)!;
            order.IsDeleted = true;
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public decimal CalculateTotalPrice(Order order)
        {

            decimal Price = 0;

            var CityId = order.ClientCityId;
            var DeliverTypeId = order.DeliveryTypeId;

            Price += CalculateCityPrice(CityId);     //City Price

            Price += CalculateOrderTypePrice(DeliverTypeId);  //Order Type Price

            Price += CalculatePriceIfShippingToVillage(order);  //Shipping To Village Price

            Price += CalculatePriceWeight(order); //Total Size Weight

            return Price;
        }


        public decimal CalculateCityPrice(int id)
        {
            City city = _context.Cities.Find(id);
            var cityPrice = city.ShippingCost;
            return cityPrice;
        }

        public decimal CalculateOrderTypePrice(int deliverTypeId)
        {
            DeliverType deliverType = _context.DeliverTypes.Find(deliverTypeId);
            var orderPrice = deliverType.Price;
            return orderPrice;
        }
        public decimal CalculatePriceIfShippingToVillage(Order order)
        {
            decimal shippingToVillagePrice;

            if (order.DeliverToVillage == true)
            {
                shippingToVillagePrice = 30;
            }
            else
            {
                shippingToVillagePrice = 0;
            }
            return shippingToVillagePrice;
        }

        public decimal CalculatePriceWeight(Order order)
        {
            var defaultWeight = _context.WeightSetting.Select(ws=>ws.DefaultSize).FirstOrDefault();
            var priceForExtraKilo = _context.WeightSetting.Select(ws => ws.PriceForEachExtraKilo).FirstOrDefault();
            decimal price = 0;

            if (defaultWeight < order.TotalWeight)
             price = (order.TotalWeight - defaultWeight) * priceForExtraKilo;
            else
            {
                price = 0;
            }
            return price;
        }
      

        public List<Order> GetByOrderState(int stateId)
        {
            return _context.Orders.Where(o => o.OrderStateId == stateId && o.IsDeleted == false)/*.Include(o=>o.ClientCity).Include(o=>o.ClientGovernorate)*/.ToList();
        }



        public List<Order> GetByRepresentativeId(string represntativeId)
        {
            return _context.Orders.Include(o => o.ClientCity)
               .Include(o => o.Representative)
                .Include(o => o.ClientGovernorate)
                .Include(o => o.ClientCity)
                .Include(o => o.OrderState)
                .Include(o => o.DeliverType)
                .Include(o => o.OrderType)
                .Include(o => o.Branch)
                .Where(o => o.RepresentativeId == represntativeId && o.OrderStateId != 4).ToList();
        }
    }
}
