﻿using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;
using infrastructure.datamodels;

namespace service;

public class ComputerVisionService
{
    public async void MakeRequest(string imagePath)
    {
        var client = new HttpClient();
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        
        // Request headers
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "b918311834b84b71aba458cf5d43830d");
        //TODO: Create environment variable for the key ^^

        // Request parameters
        queryString["features"] = "read";
        queryString["model-name"] = "";
        queryString["language"] = "en";
        queryString["smartcrops-aspect-ratios"] = "";
        queryString["gender-neutral-caption"] = "False";
        var uri = "https://cvfoodallergenes.cognitiveservices.azure.com" +
                  "/computervision/imageanalysis:analyze?api-version=2023-02-01-preview&" +
                  queryString;

        HttpResponseMessage response;

        // Request body
        byte[] byteData = File.ReadAllBytes(imagePath);

        using (var content = new ByteArrayContent(byteData))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response = await client.PostAsync(uri, content);
        }

        // Handle response
        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response: " + responseContent);
            
            var obj = JsonSerializer.Deserialize<ComputerVisionResponseDto>(responseContent);
            string content = obj.readResult.content;
            Console.WriteLine("\nContent part: \n" + content);
            
            //TODO: Check 'content' for allergens against our DB
        }
        else
        {
            Console.WriteLine("Error: " + response.StatusCode);
            Console.WriteLine("Error: " + response);
        }
    }
}