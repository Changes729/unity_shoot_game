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
    public GameObject LivingParticles05;
    public Camera viewCamera0;
    public Camera viewCamera1;
    public Camera viewCamera2;
    public Transform planePos;

    [SerializeField]
    Transform targetPos;
    InfosCollection json = new InfosCollection();
    public GameObject shootPoint;
    public List<GameObject> shootPointsList = new List<GameObject>();

    [HideInInspector]
    public float targetDistance;
    public bool isRadarPlay = true;
    float radarTimeDelay = 3f;
    public bool isShoot = false;
    public bool isGameContinue = true;

    const int TOTAL_POINTS = 2;

    void Start()
    {
        isRadarPlay = true;
        for(int i = 0; i < TOTAL_POINTS; ++i){
            GameObject newPoint = GameObject.Instantiate(shootPoint) as GameObject;
            newPoint.transform.localPosition = new Vector3(0, 0, 0);
            newPoint.SetActive(true);
            shootPointsList.Add(newPoint);

            LivingParticles05.GetComponent<LivingParticleController>().affector.Add(newPoint.transform);
        }

        StartCoroutine(RadarSound());
        StartCoroutine(GetText());
        // StartCoroutine(GetShootInfo());
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        updatePoints();
    }

    [System.Obsolete]
    IEnumerator GetText()
    {
        while (true)
        {
            string url = "http://127.0.0.1:6000";
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
                // string text = "{\"totalIndex\": 2, \"infos\": [{\"index\": 0, \"position\": [0.5645833333333333, 0.6645833333333333], \"camera\": 0, \"shoot\": false}, {\"index\": 1, \"position\": [0.5697916666666667, 0.4875], \"camera\": 0, \"shoot\": false}]}";
                json = JsonUtility.FromJson<InfosCollection>(text);
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
            string url = "http://127.0.0.1:6000/test";
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string text = www.downloadHandler.text;
                json = JsonUtility.FromJson<InfosCollection>(text);
            }
        }
    }

    void updateLocalPosition(int camera_index, int p_index, Vector3 pos) {
        Plane groundPlane = new Plane(Vector3.back, planePos.position);
        Camera[] cams = {viewCamera0, viewCamera1, viewCamera2};

        if(camera_index > 2) {
            Debug.Log("[Error] error camera index.");
            return;
        }
        Debug.Log("update camera " + camera_index + ", point index " + p_index + ", in pos " + pos);

        Ray ray = cams[camera_index].ScreenPointToRay(pos);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            targetDistance = Vector3.Distance(targetPos.position, point);
            Debug.Log(targetDistance);
            if(camera_index == 0) {
                radarTimeDelay = targetDistance / 4f;
            } else /* 1 or 2 */ {
                radarTimeDelay = targetDistance / 3f;
            }

            radarTimeDelay = Mathf.Max(radarTimeDelay, 0.1f);
            Debug.DrawLine(ray.origin, point, Color.red);
            point.z -= 0.4f;

            shootPointsList[p_index].transform.localPosition = isGameContinue ? point : new Vector3(999f, 999f, 999f);
            shootPointsList[p_index].SetActive(true);
        }
    }

    void updatePoints()
    {
        ScreenPointInfo[] infos = new ScreenPointInfo[TOTAL_POINTS];
        for( int i = 0; i < TOTAL_POINTS; ++i) {
            infos[i] = new ScreenPointInfo();
            infos[i].index = i;
            infos[i].position = new Vector3(0, 0, 0);
            infos[i].camera = 0;
            infos[i].active = false;
            infos[i].shoot = false;
        }

        /* init & debug */ {
            if(Input.mousePresent) {
                infos[0].position[0] = Input.mousePosition.x;
                infos[0].position[1] = Input.mousePosition.y;
                infos[0].camera = (int)(Input.mousePosition.x / Screen.width * 3);
            }
        }

        for(int i = 0; i < json.totalIndex && i < TOTAL_POINTS; i++) {
            Info info = json.infos[i];
            infos[i].position[0] = info.position[0] * UnityEngine.Screen.width;
            infos[i].position[1] = (1 - info.position[1]) * UnityEngine.Screen.height;
            infos[i].camera = info.camera;
            infos[i].active = true;
        }

        for( int i = 0; i < TOTAL_POINTS; ++i) {
            updateLocalPosition(infos[i].camera, infos[i].index, infos[i].position);
        }
    }
}
