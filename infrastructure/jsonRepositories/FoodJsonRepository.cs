using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1.JsonFileExtractor;

public class FoodJsonRepository
{
    public static void ExtractNamesAndItems(string jsonFilePath)
    {
        int numberOfResults = 0;
        List<string> _allegens = new List<string>(); 

        List < ProductInfo > products = new List<ProductInfo>();
        try
        {
            using (StreamReader file = File.OpenText(jsonFilePath))
            {

                string line;
                while ((line = file.ReadLine()) != null)
                {
                    numberOfResults++;
                    JObject obj = JObject.Parse(line);

                   var product =  new ProductInfo()
                    {
                        name = (string)obj["product_name_en"],
                        language = (string)obj["lang"],
                        productName = obj["product_name"] != null ? (string)obj["product_name"] : string.Empty,
                        barCode = (string)obj["code"],
                        declaration = (string)obj["ingredients_text"],
                        
                        allergens = obj["allergens_hierarchy"],
                        allergensTraces = obj["traces_hierarchy"],
                    };
                   
                   /**
                   products.Add(product);
                   
                   if(numberOfResults % 1000 == 0)
                   {
                       
                       Console.WriteLine("loaded: " + numberOfResults + " produckts");
                       Console.WriteLine("starting saving content");
                       SaveSpecificProductsToJson(products, "test.json" );
                       Console.WriteLine("finished");
                       products.Clear();
                       
                   }
                   */
                   
                   foreach (var allergen in product.allergens)
                   {
                       if (_allegens.Contains(allergen.ToString()))
                       {
                           return;
                       }
                       _allegens.Add(allergen.ToString());
                       Console.WriteLine(allergen);
                   }
                }
            }
            Console.WriteLine(_allegens);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private void PrintProductInConsole(ProductInfo product)
    {
                           
        Console.WriteLine("name: "+  product.name);
        Console.WriteLine("Product_name " + product.productName);
        Console.WriteLine("barCode " + product.barCode);
        Console.WriteLine("declaration " + product.declaration);
                   
                   
        Console.WriteLine("allergens " + product.allergens);
        Console.WriteLine("allergens Traces " + product.allergensTraces);
        Console.WriteLine("");
    }
    
    
    public static void SaveSpecificProductsToJson(List<ProductInfo> newProducts, string outputFilePath)
    {
        try
        {
            // Serialize the new products to JSON format
            string json = JsonConvert.SerializeObject(newProducts, Formatting.Indented);
            
            if (json.Length > 2) // Ensure json string contains at least an array with elements
            {
                json = json.Remove(0, 1); // Remove the first character
                json = json.Remove(json.Length - 1); // Remove the last character (which is ']')
                json += ','; // Append a comma to the end
            }

            // Append the JSON data to the file
            File.AppendAllText(outputFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Der opstod en fejl under gemning af produkterne: {ex.Message}");
        }
    }
}


