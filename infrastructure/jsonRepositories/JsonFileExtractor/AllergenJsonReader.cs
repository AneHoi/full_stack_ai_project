using Newtonsoft.Json;

namespace ConsoleApp1.JsonFileExtractor;


public class AllergenData
{
    public string Allergen { get; set; }
    public string Category { get; set; }
}

public static class AllergenJsonReader
{
    public static List<AllergenData> ReadAllergens(string filePath)
    {
        try
        {
            // Read the JSON file
            string jsonContent = File.ReadAllText(filePath);

            // Deserialize JSON to list of AllergenData objects
            var allergenDataList = JsonConvert.DeserializeObject<AllergenData[]>(jsonContent);

            // Output the allergen data
            foreach (var allergenData in allergenDataList)
            {
                //Console.WriteLine($"Allergen: {allergenData.Allergen}, Category: {allergenData.Category ?? "Unknown"}");
            }

            return allergenDataList.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
        }

        return null;
    }
    
    
    /**
     * takes the "Categories.json file path
     */
    public static List<string> ReadAllergenCategoryList(string filePath)
    {
        try
        {
            // Read the JSON file
            string jsonContent = File.ReadAllText(filePath);

            // Deserialize JSON to list of strings
            var allergenList = JsonConvert.DeserializeObject<List<string>>(jsonContent);

            return allergenList;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            return null;
        }
    }
}
