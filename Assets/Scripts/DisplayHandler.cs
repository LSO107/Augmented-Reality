using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

internal sealed class DisplayHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject imagePrefab;
    [SerializeField]
    private Text wikipediaPrefab;
    [SerializeField]
    private int picturesPerRow = 5;
    [SerializeField]
    private float imageOffset = 0.2f;
    [SerializeField]
    private Slider loadingBar;

    private const int NumberOfImages = 10;
    private float m_TotalDownloadProgress;

    /// <summary>
    /// Instantiates image prefabs, sets positions and rotation relative to camera.
    /// Sets the texture from the byte array
    /// </summary>
    public void DisplayImages(List<byte[]> images)
    {
        var currentRow = 0;
        var currentColumn = 0;
        var cam = Camera.main;
        var center = cam.transform.position;
        var camRight = cam.transform.right;
        var camForward = cam.transform.forward;

        var start = center + camForward - Vector3.Scale(camRight, new Vector3(0.3f, 0, 0.3f));

        foreach (var image in images)
        {
            var texture = new Texture2D(1, 1);
            texture.LoadImage(image);

            if (currentColumn % picturesPerRow == 0)
            {
                currentColumn = 0;
                currentRow++;
            }

            var x = start.x + (imageOffset * camRight.x) * currentColumn;
            var y = start.y + imageOffset * currentRow;
            var z = start.z + (imageOffset * camRight.z) * currentColumn;

            var img = Instantiate(imagePrefab, new Vector3(x, y, z), Quaternion.identity, transform);
            img.transform.LookAt(center);
            img.transform.Rotate(Vector3.up, 180);

            img.GetComponent<Renderer>().material.mainTexture = texture;

            currentColumn++;
        }
    }

    /// <summary>
    /// Download progress of all the images converted to
    /// a value between 0 and 1 for the slider
    /// </summary>
    public void UpdateLoadingBar(UnityWebRequest request)
    {
        m_TotalDownloadProgress += request.downloadProgress;
        loadingBar.value = (m_TotalDownloadProgress / NumberOfImages) * 1;
    }

    /// <summary>
    /// Sets <see cref="Text.text"/> value to string
    /// </summary>
    public void DisplayWikipediaText(string text)
    {
        wikipediaPrefab.text = text;
    }

    /// <summary>
    /// Iterates over child objects and deletes them
    /// </summary>
    public void DeleteOldImages()
    {
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        m_TotalDownloadProgress = 0;
        loadingBar.value = 0;
    }
}