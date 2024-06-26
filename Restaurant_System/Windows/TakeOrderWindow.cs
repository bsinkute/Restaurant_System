﻿using RestaurantSystem.Helpers;
using RestaurantSystem.Interfaces;
using RestaurantSystem.Models;

namespace RestaurantSystem.Windows
{
    public class TakeOrderWindow
    {
        private readonly ITableService _tableService;
        private readonly IOrderService _orderService;
        private readonly IEmployeeService _employeeService;

        public TakeOrderWindow(ITableService tableService, IOrderService orderService, IEmployeeService employeeService)
        {
            _tableService = tableService;
            _orderService = orderService;
            _employeeService = employeeService;
        }
        public void Load(Employee employee)
        {
            List<Table> tables = _tableService.GetTables();
            ConsoleHelper.ShowLoggedInAndClear(employee);
            Console.Write("How many guests should be seated? (or '0' to exit): ");
            bool isCorectNumber = int.TryParse(Console.ReadLine(), out int numberOfPeople);
            bool haveFittingTables = tables.Any(table => table.NumberOfSeats >= numberOfPeople && table.IsFree());

            while (!isCorectNumber || numberOfPeople < 0 || !haveFittingTables)
            {
                if (!haveFittingTables)
                {
                    Console.Write("Sorry, we don't have a table that would suit you. if you want, you can split up at different tables.");
                    Console.ReadLine();
                    return;
                }
                Console.Write("Please enter a positive number: ");
                isCorectNumber = int.TryParse(Console.ReadLine(), out numberOfPeople);
            }

            if (numberOfPeople == 0)
            {
                ConsoleHelper.GoBack();
                return;
            }

            PrintTables(tables);

            Console.Write("Enter the Id of the table at which you want customers to sit (or '0' to exit): ");
            string input = Console.ReadLine();
            bool isValidNumber = int.TryParse(input, out int tableId);
            bool tableExists = tables.Any(table => table.Id == tableId);
            bool enoughSpace = tables.Any(table => table.Id == tableId && table.NumberOfSeats >= numberOfPeople);
            bool isTableFree = tables.Any(table => table.Id == tableId && table.IsFree());

            while (!isValidNumber || !tableExists || !enoughSpace || !isTableFree)
            {
                if (!isValidNumber)
                {
                    Console.Write("Invalid input. Please enter a valid table Id: ");
                }
                else if (tableId == 0)
                {
                    ConsoleHelper.GoBack();
                    return;
                }
                else if (!tableExists)
                {
                    Console.Write($"Table with Id {tableId} does not exist. Enter a valid table Id: ");
                }
                else if (!enoughSpace)
                {
                    Console.Write($"Table with Id {tableId} does not have enough seats. Choose a different table Id: ");
                }
                else if (!isTableFree)
                {
                    Console.Write($"Table with Id {tableId} is occupied. Choose a different table Id: ");
                }
                input = Console.ReadLine();
                isValidNumber = int.TryParse(input, out tableId);
                tableExists = tables.Any(table => table.Id == tableId);
                enoughSpace = tables.Any(table => table.Id == tableId && table.NumberOfSeats >= numberOfPeople);
                isTableFree = tables.Any(table => table.Id == tableId && table.IsFree());
            }

            Table selectedTable = tables.FirstOrDefault(t => t.Id == tableId);
            SaveTableOrder(employee, tables, numberOfPeople, tableId, selectedTable);

            ConsoleHelper.ShowLoggedInAndClear(employee);
            PrintTables(tables);

            ConsoleHelper.GoBack();
        }

        private void PrintTables(List<Table> tables)
        {
            List<Order> orders = _orderService.GetOrders();
            Console.WriteLine("Tables:");
            foreach (Table table in tables)
            {
                Order tableOrder = orders.FirstOrDefault(x => x.OrderId == table.ActiveOrderId);
                if (tableOrder == null)
                {
                    Console.WriteLine(table);
                }
                else
                {
                    string employeeName = _employeeService.GetName(tableOrder.EmployeeId);
                    Console.WriteLine($"{table}, Employee: {employeeName}");
                }
            }
        }

        private void SaveTableOrder(Employee employee, List<Table> tables, int numberOfPeople, int tableId, Table selectedTable)
        {
            Console.WriteLine($"Table {tableId} selected. Number of seats: {selectedTable.NumberOfSeats}");

            Order order = new Order
            {
                NumberOfPeople = numberOfPeople,
                Checkin = DateTime.Now,
                TableId = selectedTable.Id,
                TableNumberOfSeats = selectedTable.NumberOfSeats,
                EmployeeId = employee.Id,
            };

            _orderService.AddOrder(order);
            selectedTable.Occupy(order);
            _tableService.SaveTables(tables);
        }
    }
}
