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
    public GameObject GameManager;
    public Camera viewCamera0;
    public Camera viewCamera1;
    public Camera viewCamera2;
    public Transform planePos;

    [SerializeField]
    public InfosCollection json = new InfosCollection();
    public GameObject shootPoint;
    public List<GameObject> shootPointsList = new List<GameObject>();

    [HideInInspector]
    public shootBool shoot_state = new shootBool();
    public float[] targetDistance;
    public bool isRadarPlay = true;
    float[] radarTimeDelay;
    public bool isShoot = false;
    public bool isGameContinue = true;

    int total_counts;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.GetComponent<GameManager>();
        total_counts = gameManager.total_counts;
        isRadarPlay = true;
        targetDistance = new float[total_counts];
        radarTimeDelay = new float[total_counts];
        for(int i = 0; i < total_counts; ++i){
            GameObject newPoint = GameObject.Instantiate(shootPoint) as GameObject;
            newPoint.transform.localPosition = new Vector3(0, 0, 0);
            newPoint.SetActive(true);
            shootPointsList.Add(newPoint);

            LivingParticles05.GetComponent<LivingParticleController>().affector.Add(newPoint.transform);
            targetDistance[i] = 0f;
            radarTimeDelay[i] = 3f;
        }


        for(int i = 0; i < total_counts; ++i)
        {
            StartCoroutine(RadarSound(i));
        }
        StartCoroutine(GetText());
        StartCoroutine(GetShootInfo());
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
                // string text = "{\"totalIndex\": 2, \"infos\": [{\"index\": 0, \"position\": [0.5645833333333333, 0.6645833333333333], \"camera\": 0, \"shoot\": false}, {\"index\": 1, \"position\": [0.5697916666666667, 0.4875], \"camera\": 0, \"shoot\": false}]}";
                json = JsonUtility.FromJson<InfosCollection>(text);
            }
        }
    }

    IEnumerator RadarSound(int index)
    {
        while (isRadarPlay)
        {
            string label = "radar" + (index + 1).ToString();
            FindObjectOfType<AudioManager>().Play(label);
            yield return new WaitForSeconds(radarTimeDelay[index]);
        }

    }

    [Obsolete]
    IEnumerator GetShootInfo()
    {
        while (true)
        {
            string url = "http://127.0.0.1:5000/shoot_info";
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            Debug.Log("get shoot info.");
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string text = www.downloadHandler.text;
                shoot_state = JsonUtility.FromJson<shootBool>(text);
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
            GameObject targetPos = gameManager.targetPosList[p_index];
            targetDistance[p_index] = Vector3.Distance(targetPos.transform.position, point);
            Debug.Log(targetDistance[p_index]);
            if(camera_index == 0) {
                radarTimeDelay[p_index] = targetDistance[p_index] / 4f;
            } else /* 1 or 2 */ {
                radarTimeDelay[p_index] = targetDistance[p_index] / 3f;
            }

            radarTimeDelay[p_index] = Mathf.Max(radarTimeDelay[p_index], 0.1f);
            Debug.DrawLine(ray.origin, point, Color.red);
            point.z -= 0.4f;

            shootPointsList[p_index].transform.localPosition = isGameContinue ? point : new Vector3(999f, 999f, 999f);
            shootPointsList[p_index].SetActive(true);
        }
    }

    void updatePoints()
    {
        ScreenPointInfo[] infos = new ScreenPointInfo[total_counts];
        for( int i = 0; i < total_counts; ++i) {
            infos[i] = new ScreenPointInfo();
            infos[i].index = i;
            infos[i].position = new Vector3(0, 0, 0);
            infos[i].camera = 0;
            infos[i].active = false;
        }

        /* init & debug */ {
            if(Input.mousePresent) {
                infos[0].position[0] = Input.mousePosition.x;
                infos[0].position[1] = Input.mousePosition.y;
                infos[0].camera = (int)(Input.mousePosition.x / Screen.width * 3);
            }
        }

        for(int i = 0; i < json.totalIndex && i < total_counts; i++) {
            Info info = json.infos[i];
            infos[i].position[0] = info.position[0] * UnityEngine.Screen.width;
            infos[i].position[1] = (1 - info.position[1]) * UnityEngine.Screen.height;
            infos[i].camera = info.camera;
            infos[i].active = true;
        }

        for( int i = 0; i < total_counts; ++i) {
            /* curr camera is always be 1 */
            updateLocalPosition(1, infos[i].index, infos[i].position);
        }
    }
}
