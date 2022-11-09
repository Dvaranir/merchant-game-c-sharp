using System.Configuration;
using System.Collections.Specialized;
using MerchantGame;
using MerchantGame.Entities;
using MerchantGame.Models;

Controller Controller = new();

MainMenu();

void MainMenu() 
{
    Console.WriteLine("1) Continue");
    Console.WriteLine("2) New Game");
    Console.WriteLine("3) Settings");
    Console.WriteLine("4) Exit");

    byte UserInput = Events.GetInputFromUser(4);

    switch (UserInput)
    {
        case 1:
            Console.WriteLine("1) Continue");
            break;
        case 2:
            Controller.StartGame();
            break;
        case 3:
            SettingsMenu();
            break;
        case 4:
            System.Environment.Exit(0);
            break;
    }
    
}

void SettingsMenu()
{
    Console.WriteLine("1) Add New Good");
    Console.WriteLine("2) Update Good");
    Console.WriteLine("3) Return To Main Menu");

    byte UserInput = Events.GetInputFromUser(3);

    switch (UserInput)
    {
        case 1:
            Console.WriteLine("1) Continue");
            break;
        case 2:
            Controller.StartGame();
            break;
        case 3:
            MainMenu();
            break;
    }
}




