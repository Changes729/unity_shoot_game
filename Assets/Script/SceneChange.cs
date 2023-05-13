using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(toScense1());
        }
    }

    IEnumerator toScense1()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}
