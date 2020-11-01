using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace MenuSystem
{
    
    public class Menu
    {
        
        private readonly int? _menuLevel;
        public static int CurrentPos;
        public static int CurrentLenght;
        private string _menuCommandExit;
        private string _menuCommandReturnToPrevious;
        private string _menuCommandReturnToMain;
        
        private Dictionary<string, MenuItem> _menuItemsDictionary = new Dictionary<string, MenuItem>();

        public string Title { get; set; }

        public Menu(int menuLevel = 0)
        {
            _menuLevel = menuLevel;
        }

        public Dictionary<string, MenuItem> MenuItemsDictionary
        {
            get => _menuItemsDictionary;
            set
            {

                _menuItemsDictionary = value;
                if (_menuLevel >= 2)
                {
                    _menuCommandReturnToPrevious = (_menuItemsDictionary.Count + 1).ToString();
                    _menuItemsDictionary.Add((_menuItemsDictionary.Count + 1).ToString(), 
                        new MenuItem(){Title = "Return to Previous Menu"});
                }
                if (_menuLevel >= 1)
                {
                    _menuCommandReturnToMain = (_menuItemsDictionary.Count + 1).ToString();
                    _menuItemsDictionary.Add((_menuItemsDictionary.Count + 1).ToString(), 
                        new MenuItem(){Title = "Return to Main Menu"});
                }
                else
                {
                    _menuCommandExit = (_menuItemsDictionary.Count + 1).ToString();
                    _menuItemsDictionary.Add((_menuItemsDictionary.Count + 1).ToString(), 
                        new MenuItem(){ Title = "Exit"}); 
                }
            }
        }
        public string Run()
        {
            short curItem = 1;
            ConsoleKeyInfo key;
            ConsoleAnimatedCursor spin = new ConsoleAnimatedCursor();
            var command = "";
            bool enter = false;

            do
            {
                Console.Clear();
        
                Console.WriteLine(Title);
                Console.WriteLine("========================");
                foreach (var menuItem in MenuItemsDictionary)
                {
                    if (curItem.ToString() == menuItem.Key)
                    {
                        CurrentLenght = menuItem.Value.Title.Length;
                        CurrentPos = Int32.Parse(menuItem.Key) + 1;
                        Console.WriteLine(menuItem.Value);
                    }
                    else
                    {
                        Console.WriteLine(menuItem.Value);
                    }
                }
        
                Console.WriteLine("----------");
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        command = "";
                        key = Console.ReadKey(true);
                        
                        if (key.Key.ToString() == "DownArrow")
                        {
                            curItem++;
                            if (curItem > MenuItemsDictionary.Count) curItem = 1;
                            break;
                        }
        
                        if (key.Key.ToString() == "UpArrow")
                        {
                            curItem--;
                            if (curItem < 1) curItem = Convert.ToInt16(MenuItemsDictionary.Count);
                            break;
                        }
        
                        if (key.Key.ToString() == "Enter")
                        {
                            command = curItem.ToString();
                            break;
                        }
                    }
                    spin.Turn();
                }
                if (MenuItemsDictionary.ContainsKey(command))
                {
                    var menuItem = MenuItemsDictionary[command];
                    menuItem.CommandToExecute?.Invoke();
                }
            } while (command != _menuCommandExit &&
                     command != _menuCommandReturnToMain &&
                     command != _menuCommandReturnToPrevious);
            return command;
        }
       public class ConsoleAnimatedCursor
            {
                
                int counter;
                string cursor = "<-";
                public ConsoleAnimatedCursor()
                {
                    counter = 0;
                    Console.CursorVisible = false;
                }
    
                public void Turn()
                {
                    counter++;
                    if (counter == 7)
                    {
                        counter = 0;
                    }
                    Thread.Sleep(100);
                    switch (counter % 6)
                    {
                        case 1:
                            Console.Write("{0} ", cursor);
                            Console.SetCursorPosition(CurrentLenght+( 0), CurrentPos);
                            break;
                        case 2:
                            Console.Write(" {0}", cursor);
                            Console.SetCursorPosition(CurrentLenght+( 0), CurrentPos);
                            break;
                        case 3:
                            Console.Write("  {0}", cursor);
                            Console.SetCursorPosition(CurrentLenght+( 1), CurrentPos);
                            break;
                        case 4:
                            Console.Write("   {0}", cursor);
                            Console.SetCursorPosition(CurrentLenght+( 2), CurrentPos);
                            break;
                        case 5:
                            Console.Write("  {0} ", cursor);
                            Console.SetCursorPosition(CurrentLenght+( 2), CurrentPos);
                            break;
                        case 0:
                            Console.Write(" {0} ", cursor);
                            Console.SetCursorPosition(CurrentLenght+( 1), CurrentPos);
                            break;
                    }
                }
            } 
    }
    
    
}