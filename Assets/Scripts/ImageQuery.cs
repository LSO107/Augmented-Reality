using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using Bindings;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

internal sealed class ImageQuery : MonoBehaviour
{
    [SerializeField]
    private string query;
    [SerializeField]
    private Text inputFieldText;
    [SerializeField] 
    private DisplayHandler displayHandler;
    [SerializeField]
    private InputField searchInputField;
    [SerializeField]
    private Button searchButton;

    public List<byte[]> downloadedImages = new List<byte[]>();

    private const string API_KEY = "INSERT_API_KEY";
    private const string CX = "INSERT_API_CREDENTIALS";

    public void GetPictures()
    {
        StartCoroutine(DownloadImage());
    }

    /// <summary>
    /// Sends a GET request to Google Custom Search API
    /// and downloads the queried image and Wikipedia text
    /// </summary>
    private IEnumerator DownloadImage()
    {
        searchInputField.interactable = false;
        searchButton.interactable = false;

        query = inputFieldText.text;

        var wikipediaUrl = $"https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles={query}";

        var url = $"https://www.googleapis.com/customsearch/v1?key={API_KEY}&cx={CX}&q={query}&searchType=image";

        var wikiRequest = UnityWebRequest.Get(wikipediaUrl);
        yield return wikiRequest.SendWebRequest();

        var wikiJsonString = wikiRequest.downloadHandler.text;

        string text;

        try
        {
            var wikiResponse = JsonConvert.DeserializeObject<WikipediaResponseJsonBinding>(wikiJsonString);
            text = wikiResponse.Query.Pages.Select(s => s.Value.Extract).First();
        }
        catch (JsonSerializationException e)
        {
            text = "No Wikipedia entry was found.";
        }

        var www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.LogError($"Error while receiving: {www.error}");

            if (API_KEY == "INSERT_API_KEY" || CX == "INSERT_API_CREDENTIALS")
            {
                Debug.LogError("API key or credentials required in: ImageQuery.cs");
                Debug.LogError("Please see README.md for instructions");
            }
        }
        else
        {
            var jsonString = www.downloadHandler.text;
            var imageLinks = ExtractImageLinks(jsonString);

            foreach (var link in imageLinks)
            {
                var request = UnityWebRequest.Get(link);
                request.SendWebRequest();

                while (!request.isDone)
                {
                    displayHandler.UpdateLoadingBar(request);
                    yield return null;
                }

                downloadedImages.Add(request.downloadHandler.data);
            }

            displayHandler.DeleteOldImages();

            displayHandler.DisplayImages(downloadedImages);
            displayHandler.DisplayWikipediaText(text);

            downloadedImages.Clear();

            searchInputField.interactable = true;
            searchButton.interactable = true;
        }
    }

    /// <summary>
    /// Extracts image links from Json string
    /// and returns them as an IEnumerable
    /// </summary>
    private static IEnumerable<string> ExtractImageLinks(string jsonString)
    {
        Debug.Log(jsonString);

        var results = JsonConvert.DeserializeObject<ImageResultsJsonBinding>(jsonString);

        return results.Items.Select(s => s.Link);
    }
}
