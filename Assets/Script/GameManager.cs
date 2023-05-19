using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int GameStatue = 0;
    public float targetPosZ = -4.35f;
    [SerializeField]
    public GameObject targetPos;
    public List<GameObject> targetPosList = new List<GameObject>();
    public int total_counts = 2;

    private bool key_pressed = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < total_counts; ++i){
            GameObject newPos = GameObject.Instantiate(targetPos) as GameObject;
            float targetPosX = Random.Range(-2f, 3.7f);
            float targetPosY = Random.Range(0f, 8.5f);
            newPos.transform.localPosition = new Vector3(targetPosX, targetPosY, targetPosZ);
            targetPosList.Add(newPos);
            newPos.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            key_pressed = true;
        }
        else if( key_pressed )
        {
            key_pressed = false;
            ResetTarget();
        }
    }

    void ResetTarget()
    {
        for(int i = 0; i < total_counts; ++i){
            float targetPosX = Random.Range(-2f, 3.7f);
            float targetPosY = Random.Range(0f, 8.5f);
            targetPosList[i].transform.localPosition = new Vector3(targetPosX, targetPosY, targetPosZ);
            targetPosList[i].SetActive(false);
        }
    }
}
