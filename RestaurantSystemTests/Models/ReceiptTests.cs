using RestaurantSystem.Models;

namespace RestaurantSystemTests.Models
{
    public class ReceiptTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TotalAmountVatIncluded_CalculateTotalPrice()
        {
            //Arrange
            var order = new Order(2, DateTime.Now);
            var receipt = new Receipt(1, DateTime.Now, order, "Dominos" );
            var pizza = new Dish(1, "Pizza", 12.55m);
            var fanta = new Drink(2, "Fanta", 2.50m);
            decimal expectedPrice = 30.10m;

            //Act
            order.AddOrderItem(pizza, 2);
            order.AddOrderItem(fanta, 2);
            var totalPrice = receipt.TotalAmountVatIncluded();

            //Assert
            Assert.AreEqual(expectedPrice, totalPrice);
        }


        [Test]
        public void VatAmount_CalculateVatAmount()
        {
            //Arrange
            var order = new Order(2, DateTime.Now);
            var receipt = new Receipt(1, DateTime.Now, order, "Dominos");
            var pizza = new Dish(1, "Pizza", 12.55m);
            var fanta = new Drink(2, "Fanta", 2.50m);
            decimal expectedVat = 6.321m;

            //Act
            order.AddOrderItem(pizza, 2);
            order.AddOrderItem(fanta, 2);
            var vat = receipt.VatAmount();

            //Assert
            Assert.AreEqual(expectedVat, vat);
        }

        [Test]
        public void TotalAmountWithoutVat_CalculatePriceWithoutVat()
        {
            //Arrange
            var order = new Order(2, DateTime.Now);
            var receipt = new Receipt(1, DateTime.Now, order, "Dominos");
            var pizza = new Dish(1, "Pizza", 12.55m);
            var fanta = new Drink(2, "Fanta", 2.50m);
            decimal expectedPrice = 23.779m;

            //Act
            order.AddOrderItem(pizza, 2);
            order.AddOrderItem(fanta, 2);
            var totalPrice = receipt.TotalAmountWithoutVat();

            //Assert
            Assert.AreEqual(expectedPrice, totalPrice);
        }
    }
}