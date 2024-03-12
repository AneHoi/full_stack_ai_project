using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1.JsonFileExtractor;

public class FoodJsonExtractorRepository
{
    public static void ExtractNamesAndItems(string jsonFilePath)
    {
        int numberOfResults = 0;
        int numberOfAllergens = 0;
        int dansihResult = 0;
        List<string> _allegens = new List<string>(); 

        List < ProductInfo > products = new List<ProductInfo>();
        try
        {
            using (StreamReader file = File.OpenText(jsonFilePath))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (numberOfResults % 10000 == 0)
                    {
                        Console.WriteLine(numberOfResults + " results scanned");
                    }
                    numberOfResults++;

                    JObject obj = JObject.Parse(line);
                    
                   var product =  new ProductInfo()
                    {
                        name = (string)obj["product_name_en"],
                        language = (string)obj["lang"],
                        productName = obj["product_name"] != null ? (string)obj["product_name"] : string.Empty,
                        barCode = (string)obj["code"],
                        declaration = (string)obj["ingredients_text"],
                        
                        allergens = obj["allergens_hierarchy"]  != null ? obj["allergens_hierarchy"]: null
                        //allergensTraces = obj["traces_hierarchy"],
                    };
                   
                   products.Add(product);
                   
                   //used for saving products for each 1000 so we dont run out of ram...
                   if(numberOfResults % 1000 == 0)
                   {
                       Console.WriteLine("loaded: " + numberOfResults + " produckts");
                       Console.WriteLine("starting saving content");
                       SaveSpecificProductsToJson(products, "test.json" );
                       Console.WriteLine("finished");
                       products.Clear();
                       
                   }
                   
                   /**
                   string name = (string)obj["allergens_from_ingredients"];
                   List<string> itemList = name.Split(',').ToList();
                   */

                   /**
                   var allergens = obj["allergens_tags"] != null ? obj["allergens_tags"] : null;
                   if (allergens != null && !allergens.Equals(null))
                   {
                       foreach (var allergen in allergens)
                       {
                           string aller = allergen.ToString();
                           
                           if (aller.StartsWith("en:") ||
                               aller.StartsWith("da:"))
                           {
                               dansihResult++;
                              // Console.WriteLine(dansihResult);
                               
                               if (_allegens.Contains(allergen.ToString())) continue;
                               numberOfAllergens++;
                               _allegens.Add(allergen.ToString());   
                               Console.WriteLine(allergen);
                           }
                       }
                   }
                   */
                   
                }
            }
            Console.WriteLine("allergens done ");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        
        // Serialize the new products to JSON format
        string json = JsonConvert.SerializeObject(_allegens, Formatting.Indented);
        File.AppendAllText(@"C:\Users\kenni\RiderProjects\ConsoleApp1\ConsoleApp1\test1.json", json);

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


public class ProductInfo
{
    public string language { get; set; }
    public string name { get; set; }
    public string productName { get; set; }
    public string barCode { get; set; }
    public string declaration { get; set; }
    
    public JToken? allergens { get; set; }
    public JToken? allergensTraces { get; set; }

}