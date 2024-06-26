﻿using RestaurantSystem.Helpers;
using RestaurantSystem.Interfaces;
using RestaurantSystem.Models;

namespace RestaurantSystem.Windows
{
    public class AddMenuItemWindow
    {
        private readonly IMenuService _menuService;
        public AddMenuItemWindow(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public void Load(Employee employee)
        {
            ConsoleHelper.ShowLoggedInAndClear(employee);
            Console.WriteLine("Menu:");
            foreach (MenuItem menuItem in _menuService.GetMenu().Items)
            {
                Console.WriteLine(menuItem);
            }
            while (true)
            {
                Console.WriteLine("Add new menu item:");
                Console.Write("Type '1' for Dish or '2' for Drink (or '0' to cancel): ");

                bool isChoiceCorrect = int.TryParse(Console.ReadLine(), out int choice);
                while (!isChoiceCorrect || choice < 0 || choice > 2)
                {
                    Console.Write("Invalid type selection. Please try again: ");
                    isChoiceCorrect = int.TryParse(Console.ReadLine(), out choice);
                }

                if (choice == 0)
                {
                    ConsoleHelper.GoBack();
                    return;
                }

                CreateNewItem(choice);

                Console.Write("Do you want to add another item? (yes/no): ");
                if (!ConsoleHelper.YesOrNoInput())
                {
                    break;
                }
            }

            ConsoleHelper.ShowLoggedInAndClear(employee);
            Console.WriteLine("Updated menu:");
            foreach (MenuItem menuItem in _menuService.GetMenu().Items)
            {
                Console.WriteLine(menuItem);
            }
            ConsoleHelper.GoBack();
        }

        private void CreateNewItem(int choise)
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Price: ");

            bool isPriceCorrect = decimal.TryParse(Console.ReadLine(), out decimal price);
            while (!isPriceCorrect || price <= 0)
            {
                Console.Write("Invalid Price format. Please try again: ");
                isPriceCorrect = decimal.TryParse(Console.ReadLine(), out price);
            }

            MenuItem menuItem;
            if (choise == 1)
            {
                menuItem = new Dish(1, name, price, DateTime.Now);
            }
            else
            {
                menuItem = new Drink(1, name, price, DateTime.Now);
            }

            _menuService.AddMenuItem(menuItem);
        }
    }
}
