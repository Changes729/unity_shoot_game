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

        ResetTarget();
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
        /* total counts is 2 */
        float targetPosX = Random.Range(-2f, 3.7f);
        float targetPosY = Random.Range(0f, 8.5f * (targetPosX + 2f) / (3.7f + 2f));
        targetPosList[0].transform.localPosition = new Vector3(targetPosX, targetPosY, targetPosZ);
        targetPosList[0].SetActive(false);

        targetPosX = Random.Range(-2f, 3.7f);
        targetPosY = Random.Range(8.5f - 8.5f * (targetPosX + 2f) / (3.7f + 2f), 8.5f);
        targetPosList[1].transform.localPosition = new Vector3(targetPosX, targetPosY, targetPosZ);
        targetPosList[1].SetActive(false);
    }
}
