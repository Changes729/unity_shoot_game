using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
public class SceneChange : MonoBehaviour
{

    public PointData c1;
    public PointData c2;
    public PointData c3;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetInfo());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("Space"))
        {
            StartCoroutine(toScense1());
        }
    }
    IEnumerator toScense1()
    {

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    //    [System.Obsolete]
    //    IEnumerator GetInfo()
    //    {
    //        while (true)
    //        {
    //            string url = "http://127.0.0.1:5000";
    //            UnityWebRequest www = UnityWebRequest.Get(url);
    //            yield return www.SendWebRequest();

    //            if (www.isNetworkError || www.isHttpError)
    //            {
    //                Debug.Log(www.error);
    //          
    //            {
    //                // 将结果显示为文本
    //                string text = www.downloadHandler.text;
    //                //Debug.Log(text);
    //            }
    //            else      JArray jArry = (JArray)JsonConvert.DeserializeObject(text);
    //                //PointsData aa = JsonConvert.DeserializeObject<PointsData>(text);
    //                c1 = JsonConvert.DeserializeObject<PointData>(jArry[0].ToString());
    //                c2 = JsonConvert.DeserializeObject<PointData>(jArry[1].ToString());
    //                c3 = JsonConvert.DeserializeObject<PointData>(jArry[2].ToString());
    //                if ((bool)c1.active || (bool)c2.active || (bool)c3.active)
    //                {
    //                    yield return StartCoroutine(toScense1());
    //                }

    //            }
    //        }
}

