// Get the latest webcam shot from outside "Friday's" in Times Square
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WWWTest : MonoBehaviour
{
    public string url = "https://www.colorcombos.com/images/colors/FFCC00.png";

    public Material fallbackMat;

    void Start()
    {
        StartCoroutine(GetText());
    }


    IEnumerator GetText()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            Renderer renderer = GetComponent<Renderer>();

            if(uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
                this.GetComponent<Renderer>().material = fallbackMat;
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);
                this.GetComponent<Renderer>().material.mainTexture = texture;
            }
            
        }
    }
}