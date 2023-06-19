using System;
using System.Collections.Generic;

// Интерфейс для наблюдателя
public interface IObserver
{
    void Update(string data);
}

// Интерфейс для субъекта
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

// Реализация субъекта
public class WeatherStation : ISubject
{
    private List<IObserver> _observers;
    private string _weatherData;

    public WeatherStation()
    {
        _observers = new List<IObserver>();
    }

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_weatherData);
        }
    }

    public void SetWeatherData(string weatherData)
    {
        _weatherData = weatherData;
        Notify();
    }
}

// Реализация наблюдател
public class Display : IObserver
{
    public void Update(string data)
    {
        Console.WriteLine(data);
    }
}

// Интерфейс для внешнего API сторонней библиотеки
public interface IExternalApi
{
    void SendRequest(string data);
}

public class ExternalApi
{
    public void SendExternalRequest(string data)
    {
        
    }
}


public class ExternalApiAdapter : IExternalApi
{
    private readonly ExternalApi _externalApi;

    public ExternalApiAdapter(ExternalApi externalApi)
    {
        _externalApi = externalApi;
    }

    public void SendRequest(string data)
    {
        _externalApi.SendExternalRequest(data);
    }
}

// Интерфейс продукта
public interface IProduct
{
    void Use();
}

// Реализация продукта
public class ConcreteProduct : IProduct
{
    public void Use()
    {
       
    }
}

// Фабричный метод для создания продукта
public abstract class Creator
{
    public abstract IProduct FactoryMethod();
}

// Конкретный создатель, реализующий фабричный метод
public class ConcreteCreator : Creator
{
    public override IProduct FactoryMethod()
    {
        return new ConcreteProduct();
    }
}

// Архитектурный шаблон: MVC
// Модель
public class Recipe
{
    public string Name { get; set; }
    public List<string> Ingredients { get; set; }
    public string Instructions { get; set; }
}

// Контроллер
public class RecipeController
{
    private List<Recipe> _recipes;

    public RecipeController()
    {
        _recipes = new List<Recipe>();
    }

    public void CreateRecipe(string name, List<string> ingredients, string instructions)
    {
        Recipe recipe = new Recipe
        {
            Name = name,
            Ingredients = ingredients,
            Instructions = instructions
        };

        _recipes.Add(recipe);
        Console.WriteLine("\nСоздать рецепт: " + recipe.Name);
    }

    public List<Recipe> GetRecipes()
    {
        return _recipes;
    }
}

// Представление
public class RecipeView
{
    private RecipeController _recipeController;

    public RecipeView(RecipeController recipeController)
    {
        _recipeController = recipeController;
    }

    public void DisplayRecipes(List<Recipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            Console.WriteLine("Название рецепта: " + recipe.Name);
            Console.WriteLine("Ингредиенты: " + string.Join(", ", recipe.Ingredients));
            Console.WriteLine("Инструкции: " + recipe.Instructions);
            Console.WriteLine();
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Использование поведенческого паттерна: Наблюдатель (Observer)
        WeatherStation weatherStation = new WeatherStation();
        Display display = new Display();
        weatherStation.Attach(display);
        weatherStation.SetWeatherData("Температура: 28°C");

        // Использование структурного паттерна: Адаптер (Adapter)
        ExternalApi externalApi = new ExternalApi();
        IExternalApi externalApiAdapter = new ExternalApiAdapter(externalApi);
        externalApiAdapter.SendRequest("Данные");

        // Использование порождающего паттерна: Фабричный метод (Factory Method)
        Creator creator = new ConcreteCreator();
        IProduct product = creator.FactoryMethod();
        product.Use();

        // Использование архитектурного шаблона: MVC
        RecipeController recipeController = new RecipeController();
        RecipeView recipeView = new RecipeView(recipeController);

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Создать рецепт");
            Console.WriteLine("2. Посмотреть рецепты");
            Console.WriteLine("3. Получить названия о популярных блюдах в России");
            Console.WriteLine("4. Выход");
            Console.Write("Выберите опцию: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Создание нового рецепта.");
                    Console.Write("Введите имя рецепта ");
                    string name = Console.ReadLine();
                    Console.Write("Введите ингредиенты (через запятую): ");
                    string[] ingredients = Console.ReadLine().Split(',');
                    Console.Write("Введите инструкции: ");
                    string instructions = Console.ReadLine();

                    List<string> ingredientList = new List<string>(ingredients.Length);
                    foreach (var ingredient in ingredients)
                    {
                        ingredientList.Add(ingredient.Trim());
                    }

                    recipeController.CreateRecipe(name, ingredientList, instructions);
                    Console.WriteLine("\nРецепт успешно создан.");
                    Console.WriteLine();
                    break;

                case "2":
                    Console.WriteLine("\nПросмотр рецептов...");
                    List<Recipe> recipes = recipeController.GetRecipes();
                    recipeView.DisplayRecipes(recipes);
                    Console.WriteLine();
                    break;

                case "3":
                    Console.WriteLine("Получение названий популярных блюд в России...");
                    weatherStation.SetWeatherData("1) Борщ");
                    weatherStation.SetWeatherData("2) Оливье");
                    weatherStation.SetWeatherData("3) Холодец");

                    Console.WriteLine();
                    break;

                case "4":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Неверный вариант. Пожалуйста, попробуйте еще раз.");
                    Console.WriteLine();
                    break;
            }
        }
    }
}
