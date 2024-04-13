﻿namespace RestaurantSystem.Models
{
    public class Receipt
    {
        public int Id { get; private set; }
        public DateTime Date { get; private set; }
        public Order Order { get; private set; }
        public string RestaurantName { get; private set; }
        private decimal VatRate = 0.21m;

        public Receipt(int id, DateTime date, Order order, string restaurantName)
        {
            Id = id;
            Date = date;
            Order = order;
            RestaurantName = restaurantName;
        }

        public decimal TotalAmountVatIncluded()
        {
            decimal totalAmount = 0;
            foreach (var item in Order.Items)
            {
                totalAmount += item.Quantity * item.OrderedItem.Price;
            }
            return totalAmount;
        }

        public decimal TotalAmountWithoutVat()
        {
            return TotalAmountVatIncluded() - VatAmount();
        }

        public decimal VatAmount()
        {
            return TotalAmountVatIncluded() * VatRate;
        }

        public override string ToString()
        {
            return $"Id: {Id}";
        }

    }
}