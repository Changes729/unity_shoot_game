using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class SceneChange : MonoBehaviour
{
    public InfosCollection json = new InfosCollection();

    void Start()
    {
        StartCoroutine(GetText());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || json.totalIndex > 0)
        {
            StartCoroutine(toScense1());
        }
    }

    IEnumerator toScense1()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    [System.Obsolete]
    IEnumerator GetText()
    {
        while (true)
        {
            string url = "http://127.0.0.1:5000/aim_info";
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                json.totalIndex = 0;
            }
            else
            {
                string text = www.downloadHandler.text;
                json = JsonUtility.FromJson<InfosCollection>(text);
            }
        }
    }
}
