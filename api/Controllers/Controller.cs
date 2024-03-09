using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class Controller : ControllerBase
{
    [HttpGet]
    [Route("/allergeneApp/GetAllergenes/")]
    public async Task<List<string>> GetAllergenes()
    {
        string apiKey = "ec4bd432e78be9f8563fbd017af9f321";
        string appId = "bb0ac8c8";

        string ingredients = "CHOCOLATE";

        try
        {
            var allergens = await GetAllergensAsync(apiKey, appId, ingredients);
            Console.WriteLine("Allergens in the product: " + string.Join(", ", allergens));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        return null;
    }
    static async Task<string[]> GetAllergensAsync(string apiKey, string appId, string ingredients)
    {
        using (HttpClient client = new HttpClient())
        {
            string endpoint = "https://api.edamam.com/api/food-database/v2/parser";

            var queryParams = $"?ingr={Uri.EscapeDataString(ingredients)}&app_id={appId}&app_key={apiKey}";

            var response = await client.GetAsync(endpoint + queryParams);
            Console.WriteLine(endpoint + queryParams);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<EdamamResponse>();

                // Extract allergens from the response
                var allergens = data.Parsed?[0]?.Food?.Allergens ?? new string[0];
                return allergens;
            }
            else
            {
                throw new HttpRequestException($"API request failed with status code {response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
            }
        }
    }

}

public class EdamamResponse
{
    public ParsedItem[] Parsed { get; set; }
}

public class ParsedItem
{
    public FoodItem Food { get; set; }
}

public class FoodItem
{
    public string[] Allergens { get; set; }
}