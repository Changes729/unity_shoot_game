using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


public class Curse : MonoBehaviour
{

    public Camera viewCamera0;
    public Camera viewCamera1;
    public Camera viewCamera2;
    public Transform planePos;
    [SerializeField]
    Transform targetPos;
    Vector3 cursorPos = new Vector3(0, 0, 0);
    [HideInInspector]
    public float targetDistance;
    public bool isRadarPlay = true;
    float radarTimeDelay = 3f;
    public bool isShoot = false;
    public bool isGameContinue = true;
    public PointData c1;
    public PointData c2;
    public PointData c3;
    public int currentScreen = 0;
    void Start()
    {
        isRadarPlay = true;
        StartCoroutine(RadarSound());
        //StartCoroutine(GetText());
        //StartCoroutine(GetShootInfo());

    }

    // Update is called once per frame

    [System.Obsolete]
    void Update()
    {

        detectMousePos();
        //calPosOnScreen();
        Plane groundPlane = new Plane(Vector3.back, planePos.position);
        //print(cursorPos);
        if (currentScreen == 0)
        {
            Ray ray = viewCamera0.ScreenPointToRay(cursorPos);
            //Ray ray = viewCamera0.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float rayDistance))
            {

                Vector3 point = ray.GetPoint(rayDistance);
                targetDistance = Vector3.Distance(targetPos.position, point);
                print(targetDistance);
                radarTimeDelay = targetDistance / 4f;
                if (radarTimeDelay < 0.1f) radarTimeDelay = 0.1f;
                Debug.DrawLine(ray.origin, point, Color.red);
                point.z -= 0.4f;

                if (isGameContinue)
                {

                    transform.localPosition = point;
                }
                else
                {
                    transform.localPosition = new Vector3(999f, 999f, 999f);
                }
            }
        }
        else if (currentScreen == 1)
        {
            Ray ray = viewCamera1.ScreenPointToRay(cursorPos);
            //Ray ray = viewCamera1.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float rayDistance))
            {

                Vector3 point = ray.GetPoint(rayDistance);
                targetDistance = Vector3.Distance(targetPos.position, point);
                print(targetDistance);
                radarTimeDelay = targetDistance / 3f;
                if (radarTimeDelay < 0.1f) radarTimeDelay = 0.1f;
                Debug.DrawLine(ray.origin, point, Color.red);
                point.z -= 0.4f;

                if (isGameContinue)
                {

                    transform.localPosition = point;
                }
                else
                {
                    transform.localPosition = new Vector3(999f, 999f, 999f);
                }
            }
        }
        else if (currentScreen == 2)
        {
            Ray ray = viewCamera2.ScreenPointToRay(cursorPos);
            //Ray ray = viewCamera2.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float rayDistance))
            {

                Vector3 point = ray.GetPoint(rayDistance);
                targetDistance = Vector3.Distance(targetPos.position, point);
                print(targetDistance);
                radarTimeDelay = targetDistance / 3f;
                if (radarTimeDelay < 0.1f) radarTimeDelay = 0.1f;
                Debug.DrawLine(ray.origin, point, Color.red);
                point.z -= 0.4f;

                if (isGameContinue)
                {

                    transform.localPosition = point;
                }
                else
                {
                    transform.localPosition = new Vector3(999f, 999f, 999f);
                }
            }
        }
    }

    [System.Obsolete]
    IEnumerator GetText()
    {
        while (true)
        {
            string url = "http://127.0.0.1:5000";
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // 将结果显示为文本
                string text = www.downloadHandler.text;
                //JArray jlist = JArray.Parse(text);
                JArray jArry = (JArray)JsonConvert.DeserializeObject(text);
                //PointsData aa = JsonConvert.DeserializeObject<PointsData>(text);
                c1 = JsonConvert.DeserializeObject<PointData>(jArry[0].ToString());
                c2 = JsonConvert.DeserializeObject<PointData>(jArry[1].ToString());
                c3 = JsonConvert.DeserializeObject<PointData>(jArry[2].ToString());
                //cursorPos.x = (float)obj["x"] * UnityEngine.Screen.width;
                //cursorPos.y = UnityEngine.Screen.height - (float)obj["y"] * UnityEngine.Screen.height;
                //isShoot = (bool)obj["shoot"];
            }
        }
    }
    IEnumerator RadarSound()
    {
        while (isRadarPlay)
        {
            //Debug.Log("playradar");
            FindObjectOfType<AudioManager>().Play("radar");
            yield return new WaitForSeconds(radarTimeDelay);
        }

    }

    [Obsolete]
    IEnumerator GetShootInfo()
    {
        while (true)
        {
            string url = "http://127.0.0.1:5000/isShoot";
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string text = www.downloadHandler.text;
                isShoot = Convert.ToBoolean(text);
            }
        }
    }

    void mousePos() {
        cursorPos.x = Input.mousePosition.x;
        cursorPos.y = Input.mousePosition.y;
    } 
     
    void calPosOnScreen()
    {
        if (c1.active)
        {
            cursorPos.x = c1.x * UnityEngine.Screen.width / 3;
            cursorPos.y = (1 - c1.y) * UnityEngine.Screen.height;
            currentScreen = 0;
        }
        else if (c2.active)
        {
            cursorPos.x = c2.x * UnityEngine.Screen.width / 3 + UnityEngine.Screen.width / 3;
            cursorPos.y = (1 - c2.y) * UnityEngine.Screen.height;
            currentScreen = 1;
        }
        else if (c3.active)
        {
            cursorPos.x = c3.x * UnityEngine.Screen.width / 3 + UnityEngine.Screen.width / 3 * 2;
            cursorPos.y = (1 - c3.y) * UnityEngine.Screen.height;
            currentScreen = 2;
        }
        else
        {
            cursorPos.x = 0;
            cursorPos.y = 0;
        }
    }
    void detectMousePos()
    {
        if (Input.mousePosition.x < Screen.width / 3)
        {
            currentScreen = 0;
        }
        else if (Input.mousePosition.x > Screen.width / 3 && Input.mousePosition.x < Screen.width / 3 * 2)
        {
            currentScreen = 1;
        }
        else if (Input.mousePosition.x > Screen.width / 3 * 2 && Input.mousePosition.x < Screen.width)
        {
            currentScreen = 2;
        }
    }
}
