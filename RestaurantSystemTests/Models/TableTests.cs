﻿using RestaurantSystem.Models;

namespace RestaurantSystemTests.Models
{
    public class TableTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsFree_WhenReservationAndOrderIsNull()
        {
            //Arrange
            var table = new Table() { NumberOfSeats = 5 };

            //Act
            bool isFree = table.IsFree();

            //Assert
            Assert.IsTrue(isFree);
        }

        [Test]
        public void IsFree_NotFreeWithReservation()
        {
            //Arrange
            var table = new Table { NumberOfSeats = 5 };
            var reservation = new Reservation(DateTime.Now, 4, "Birute", "+37060054640", 1);

            //Act
            bool isReserved = table.Reserve(reservation);
            bool isFree = table.IsFree();

            //Assert
            Assert.IsTrue(isReserved);
            Assert.IsFalse(isFree);
        }

        [Test]
        public void IsFree_NotFreeWithOrder()
        {
            //Arrange
            var table = new Table { NumberOfSeats = 5 };
            var order = new Order
            {
                OrderId = 1,
                NumberOfPeople = 5,
            };

            //Act
            bool isOccupied = table.Occupy(order);
            bool isFree = table.IsFree();

            //Assert
            Assert.IsTrue(isOccupied);
            Assert.IsFalse(isFree);
        }

        [Test]
        public void Reserve_NotReserveWithOrder()
        {
            //Arrange
            var table = new Table { NumberOfSeats = 5 };
            var order = new Order
            {
                OrderId = 1,
                NumberOfPeople = 5,
            };
            var reservation = new Reservation(DateTime.Now, 4, "Birute", "+37060054640", 1);

            //Act
            bool isOccupied = table.Occupy(order);
            bool isReserved = table.Reserve(reservation);

            //Assert
            Assert.IsTrue(isOccupied);
            Assert.IsFalse(isReserved);
        }

        [Test]
        public void Reserve_NotReserveWithReservation()
        {
            //Arrange
            var table = new Table { NumberOfSeats = 5 };
            var firstReservation = new Reservation(DateTime.Now, 4, "Birute", "+37060054640", 1);
            var secondReservation = new Reservation(DateTime.Now, 5, "Ana", "+37060054660", 1);

            //Act
            bool isReservedFirst = table.Reserve(firstReservation);
            bool isReservedSecond = table.Reserve(secondReservation);

            //Assert
            Assert.IsTrue(isReservedFirst);
            Assert.IsFalse(isReservedSecond);
        }

        [Test]
        public void Reserve_NotReservePeopleMoreThanSeats()
        {
            //Arrange
            var table = new Table { NumberOfSeats = 5 };
            var reservation = new Reservation(DateTime.Now, 6, "Birute", "+37060054640", 1);

            //Act
            bool isReserved = table.Reserve(reservation);

            //Assert
            Assert.IsFalse(isReserved);
        }

        [Test]
        public void Occupy_NotOccupyWithOrder()
        {
            //Arrange
            var table = new Table { NumberOfSeats = 5 };
            var orderFirst = new Order
            {
                OrderId = 1,
                NumberOfPeople = 5,
            };
            var orderSecond = new Order
            {
                OrderId = 2,
                NumberOfPeople = 4,
            };


            //Act
            bool isOccupiedFirst = table.Occupy(orderFirst);
            bool isOccupiedSecond = table.Occupy(orderSecond);

            //Assert
            Assert.IsTrue(isOccupiedFirst);
            Assert.IsFalse(isOccupiedSecond);
        }

        [Test]
        public void Occupy_NotOccupyWithReservation()
        {
            //Arrange
            var table = new Table { NumberOfSeats = 5 };
            var reservation = new Reservation(DateTime.Now, 4, "Birute", "+37060054640", 1);
            var order = new Order { OrderId = 1, NumberOfPeople = 5 };

            //Act
            bool isReserved = table.Reserve(reservation);
            bool isOccupied = table.Occupy(order);

            //Assert
            Assert.IsTrue(isReserved);
            Assert.IsFalse(isOccupied);
        }

        [Test]
        public void Occupy_NotOccupyPeopleMoreThanSeats()
        {
            //Arrange
            var table = new Table { NumberOfSeats = 5 };
            var order = new Order { OrderId = 1, NumberOfPeople = 7 };

            //Act
            bool isOccupied = table.Occupy(order);

            //Assert
            Assert.IsFalse(isOccupied);
        }

        [Test]
        public void FreeUp_FreesUp()
        {
            //Arrange
            var table = new Table { NumberOfSeats = 5 };
            var order = new Order { OrderId = 1, NumberOfPeople = 4 };

            //Act
            bool isOccupied = table.Occupy(order);
            table.FreeUp();
            bool isFree = table.IsFree();

            //Assert
            Assert.IsTrue(isOccupied);
            Assert.IsTrue(isFree);
        }
    }
}
