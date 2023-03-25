using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class BaganTimeAPI : MonoBehaviour
{
    public GameObject timeTextObject;
       string url = "http://worldtimeapi.org/api/timezone/Asia/Yangon";

   
    void Start()
    {
       InvokeRepeating("GetDataFromWeb", 0f, 10f);
   }

   void GetDataFromWeb()
   {
       StartCoroutine(GetRequest(url));
   }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            if (webRequest.result ==  UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                // print out the time data to make sure it makes sense
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);

            	// grab the current time and simplify it if needed
            	int startTime = webRequest.downloadHandler.text.IndexOf("datetime",0);
            	int endTime = webRequest.downloadHandler.text.IndexOf("+",startTime);
				string timeF = webRequest.downloadHandler.text.Substring(startTime+22, (endTime-startTime-22));
                int hr = int.Parse(timeF.Substring(0, 2));
                int min = int.Parse(timeF.Substring(3, 2));
                // int sec = int.Parse(timeF.Substring(6, 2));
                string ampm = "";

                if (hr == 0) {
                    hr = 12;
                    ampm = "a.m.";
                } else if (hr < 12) {
                    ampm = "a.m.";
                } else if (hr == 12) {
                    ampm = "p.m.";
                } else {
                    hr -= 12;
                    ampm = "p.m.";
                }

                timeTextObject.GetComponent<TextMeshPro>().text = "" + hr.ToString("D2") + ":" + min.ToString("D2") + " " + ampm;
            }
        }
    }
}

