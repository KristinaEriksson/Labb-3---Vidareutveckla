using System;
using System.Collections.Generic;

// Kristina Eriksson .NET22

namespace VarmDrinkStation
{
    // Interface for warm drinks
    public interface IWarmDrink
    {
        void Consume();
    }

    // Implementation of warm water
    internal class Water : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Warm water is served.");
        }
    }

    // Implementation of coffee
    internal class Coffee : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Coffee is served.");
        }
    }

    // Implementation of cappuccino
    internal class Cappuccino : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Cappuccino is served.");
        }
    }

    // Implementation of hot chocolate
    internal class HotChocolate : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Hot chocolate is served.");
        }
    }

    // Interface for warm drink factories
    public interface IWarmDrinkFactory
    {
        IWarmDrink Prepare(int total);
    }

    // Implementation of hot water factory
    internal class HotWaterFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot water in your cup");
            return new Water();
        }
    }

    // Implementation of coffee factory
    internal class CoffeeFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Brew {total} ml of coffee");
            return new Coffee();
        }
    }

    // Implementation of cappuccino factory
    internal class CappuccinoFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Brew {total} ml of cappuccino");
            return new Cappuccino();
        }
    }

    // Implementation of hot chocolate factory
    internal class HotChocolateFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Make {total} ml of hot chocolate");
            return new HotChocolate();
        }
    }

    // Warm drink machine class
    public class WarmDrinkMachine
    {
        // Enums available drinks
        public enum AvailableDrink // violates open-closed
        {
            Coffee,
            Cappuccino,
            Chocolate,
            Water
        }

        // Dictionary to store drink factories
        private Dictionary<AvailableDrink, IWarmDrinkFactory> factories =
          new Dictionary<AvailableDrink, IWarmDrinkFactory>();

        // List to store named factories
        private List<Tuple<string, IWarmDrinkFactory>> namedFactories =
          new List<Tuple<string, IWarmDrinkFactory>>();

        // Constructor to initialize the machine
        public WarmDrinkMachine()
        {
            // Loop through all types in the assembly to find warm drink factories
            foreach (var t in typeof(WarmDrinkMachine).Assembly.GetTypes())
            {
                // Check if the type is assignable to the IWarmDrinkFactory interface and not an interface itself
                if (typeof(IWarmDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    // Add the factory to the named factories list
                    namedFactories.Add(Tuple.Create(
                      t.Name.Replace("Factory", string.Empty), (IWarmDrinkFactory)Activator.CreateInstance(t)));
                }
            }
        }

        // Method to make a drink
        public IWarmDrink MakeDrink()
        {
            Console.WriteLine("This is what we serve today:");
            for (var index = 0; index < namedFactories.Count; index++)
            {
                var tuple = namedFactories[index];
                Console.WriteLine($"{index}: {tuple.Item1}");
            }
            Console.WriteLine("Select a number to continue:");
            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null
                    && int.TryParse(s, out int i) // c# 7
                    && i >= 0
                    && i < namedFactories.Count)
                {
                    Console.Write("How much: ");
                    s = Console.ReadLine();
                    if (s != null
                        && int.TryParse(s, out int total)
                        && total > 0)
                    {
                        return namedFactories[i].Item2.Prepare(total);
                    }
                }
                Console.WriteLine("Something went wrong with your input, try again.");
            }
        }
    }

    // Main program
    class Program
    {
        static void Main(string[] args)
        {
            // Creates a instance of WarmDrinkMachine and calls the MakeDrink method and then calls the Consume method
            var machine = new WarmDrinkMachine();
            IWarmDrink drink = machine.MakeDrink();
            drink.Consume();
        }
    }
}
