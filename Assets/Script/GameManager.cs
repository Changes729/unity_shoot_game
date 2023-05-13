using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int GameStatue = 0;
    [SerializeField]
    public Transform targetPos;
    [SerializeField]
    public float targetPosZ = -2.46f;

    private bool key_pressed = false;
    // Start is called before the first frame update
    void Start()
    {
        SetTarget();
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
            SetTarget();
        }
    }

    void SetTarget()
    {
        float targetPosX = Random.Range(-3.8f, 4.25f);
        float targetPosY = Random.Range(1.5f, 7f);
        targetPos.localPosition = new Vector3(targetPosX, targetPosY, targetPosZ);
        Debug.Log(targetPos.localPosition);
    }
}
