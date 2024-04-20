﻿using RestaurantSystem.Helpers;
using RestaurantSystem.Interfaces;
using RestaurantSystem.Models;

namespace RestaurantSystem.Windows
{
    public class ReviewOrdersWindow
    {
        private readonly IOrderService _orderService;
        private readonly IMenuService _menuService;

        public ReviewOrdersWindow(IOrderService orderService, IMenuService menuService)
        {
            _orderService = orderService;
            _menuService = menuService;
        }

        public void Load(Employee employee)
        {
            List<Order> activeOrders = _orderService.GetEmployeeOrders(employee.Id);

            Console.Clear();
            Console.WriteLine("Active Orders:");
            foreach (Order order in activeOrders)
            {
                Console.WriteLine($"Order ID: {order.OrderId} | Table: {order.TableId} | Guests: {order.NumberOfPeople} | Date: {order.Checkin}");
            }

            Console.Write("Enter the Order ID to review details (or '0' to exit): ");
            string input = Console.ReadLine();
            bool isValidNumber = int.TryParse(input, out int orderId);

            while (!isValidNumber || (orderId != 0 && !activeOrders.Any(o => o.OrderId == orderId)))
            {
                if (orderId == 0)
                {
                    return;
                }

                Console.Write("Invalid input. Enter a valid Order ID (or '0' to exit): ");
                input = Console.ReadLine();
                isValidNumber = int.TryParse(input, out orderId);
            }


            if (orderId != 0)
            {
                Order selectedOrder = activeOrders.FirstOrDefault(o => o.OrderId == orderId);
                Console.Clear();
                Console.WriteLine($"Order Details for Order ID: {selectedOrder.OrderId}");
                Console.WriteLine($"Table: {selectedOrder.TableId}");
                Console.WriteLine($"Number of Guests: {selectedOrder.NumberOfPeople}");
                Console.WriteLine("Ordered Items:");
                if ( selectedOrder.Items.Count == 0 )
                {
                    Console.WriteLine("No items added yet.");
                }
                else
                {
                    Console.WriteLine(selectedOrder);
                }

                Console.WriteLine("Options:");
                Console.WriteLine("1. Ordered dish or drink");
                Console.WriteLine("2. Exit");

                Console.Write("Select an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        foreach (MenuItem menuItem in _menuService.GetMenu().Items)
                        {
                            Console.WriteLine(menuItem);
                        }
                        OrderItem(selectedOrder);
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void OrderItem(Order selectedOrder)
        {
            Console.Write("Enter the Id of the menu item to add: ");

            if (int.TryParse(Console.ReadLine(), out int menuItemId))
            {
                MenuItem selectedMenuItem = _menuService.GetMenuItemById(menuItemId);
                if (selectedMenuItem != null)
                {
                    Console.Write("Enter the quantity to add: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                    {
                        _orderService.AddMenuItemToOrder(selectedOrder.OrderId, selectedMenuItem, quantity);
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity. Quantity must be a positive integer.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid menu item ID.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }

            Console.WriteLine("Do you want to add another item? (yes/no): ");
            bool addAnotherInput = ConsoleHelper.YesOrNoInput();
            if(addAnotherInput)
            {
                OrderItem(selectedOrder);
            }
        }
    }
}
