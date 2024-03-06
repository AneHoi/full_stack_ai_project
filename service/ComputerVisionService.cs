using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using infrastructure.Models;

namespace service;

public static class ComputerVisionService
{
    public static async void MakeRequest()
    {
        var client = new HttpClient();
        var queryString = HttpUtility.ParseQueryString(string.Empty);

        // Request headers
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "b918311834b84b71aba458cf5d43830d");

        // Request parameters
        queryString["features"] = "read";
        queryString["model-name"] = "";
        queryString["language"] = "en";
        queryString["smartcrops-aspect-ratios"] = "";
        queryString["gender-neutral-caption"] = "False";
        var uri = "https://cvfoodallergenes.cognitiveservices.azure.com/computervision/imageanalysis:analyze?api-version=2023-02-01-preview&" + queryString;

        HttpResponseMessage response;

        /* Object from image stream
        
        // Request body
        byte[] byteData = File.ReadAllBytes("C:\\Users\\micha\\Documents\\CSe2022A\\4. semester\\Fullstack\\AI Project\\marcipan.jpg");

        using (var content = new ByteArrayContent(byteData))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response = await client.PostAsync(uri, content);
        }
        */
        
        // Create your object
        var requestBody = new
        {
            url = "https://assets.summerbird.dk/media/wdof5euo/5509-marcipan-200-g-vegansk.jpg"
        };

        // Serialize the object to JSON
        string jsonBody = JsonSerializer.Serialize(requestBody);

        // Request body
        using (var content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
        {
            response = await client.PostAsync(uri, content);
        }
        
        // Handle response
        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response: " + responseContent);
            var obj = JsonSerializer.Deserialize<Root>(responseContent);
            string content = obj.readResult.content;
            Console.WriteLine("\nContent part: \n" + content);
        }
        else
        {
            Console.WriteLine("Error: " + response.StatusCode);
        }
    }
}
