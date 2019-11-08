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

    private string m_WikipediaText;

    public List<byte[]> downloadedImages = new List<byte[]>();

    private const string API_KEY = "";
    private const string CX = "";

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
        ToggleSelectable(searchInputField, searchButton);

        query = inputFieldText.text;

        var wikipediaUrl = $"https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles={query}";

        var googleUrl = $"https://www.googleapis.com/customsearch/v1?key={API_KEY}&cx={CX}&q={query}&searchType=image";

        StartCoroutine(ExtractWikipediaText(wikipediaUrl));

        var www = UnityWebRequest.Get(googleUrl);
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.LogError($"Error while receiving: {www.error}");

            if (string.IsNullOrEmpty(API_KEY) || string.IsNullOrEmpty(CX))
                Debug.Log("Insert API_KEY and API_CREDENTIALS. See README.md for details.");
        }
        else
        {
            var jsonString = www.downloadHandler.text;
            var imageLinks = ExtractImageLinks(jsonString);

            foreach (var link in imageLinks)
            {
                var request = UnityWebRequest.Get(link);
                yield return request.SendWebRequest();

                downloadedImages.Add(request.downloadHandler.data);
                displayHandler.UpdateLoadingBar(request);
            }

            displayHandler.DeleteOldImages();

            displayHandler.DisplayImages(downloadedImages);
            displayHandler.DisplayWikipediaText(m_WikipediaText);

            downloadedImages.Clear();

            ToggleSelectable(searchInputField, searchButton);
        }
    }

    /// <summary>
    /// Makes a request to Wikipedia API then sets response to <see cref="m_WikipediaText"/>
    /// </summary>
    private IEnumerator ExtractWikipediaText(string wikipediaUrl)
    {
        var wikiRequest = UnityWebRequest.Get(wikipediaUrl);
        yield return wikiRequest.SendWebRequest();

        var wikiJsonString = wikiRequest.downloadHandler.text;

        try
        {
            var wikiResponse = JsonConvert.DeserializeObject<WikipediaResponseJsonBinding>(wikiJsonString);
            m_WikipediaText = wikiResponse.Query.Pages.Select(s => s.Value.Extract).First();
        }
        catch (JsonSerializationException e)
        {
            m_WikipediaText = "No Wikipedia entry was found.";
        }
    }

    /// <summary>
    /// Extracts image links from Json string
    /// and returns them as an IEnumerable
    /// </summary>
    private IEnumerable<string> ExtractImageLinks(string jsonString)
    {
        try
        {
            var results = JsonConvert.DeserializeObject<ImageResultsJsonBinding>(jsonString);
            return results.Items.Select(s => s.Link);

        }
        catch (JsonSerializationException e)
        {
            ToggleSelectable(searchInputField, searchButton);
            throw new JsonSerializationException("Word must be entered");
        }
    }

    /// <summary>
    /// Toggle interactable feature on input field and button
    /// </summary>
    private static void ToggleSelectable(Selectable inputField, Selectable button)
    {
        inputField.interactable = !inputField.interactable;
        button.interactable = !button.interactable;
    }
}
