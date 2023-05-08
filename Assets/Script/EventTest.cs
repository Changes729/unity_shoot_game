using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;
public class EventTest : MonoBehaviour
{
    // Start is called before the first frame update
    public VisualEffect vfxExplosion;
    public VisualEffect vfxMiss;
    public PlayableDirector playableDirector;
    public PlayableDirector MissDirector;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] Transform explotion;
    [SerializeField] Transform missExplotion;
    [SerializeField] GameObject targetCube;
    [SerializeField] GameObject aimMesh;
    void Start()
    {

        if (vfxExplosion == null)
        {
            Debug.Log("VFX ЮЊПе!");
            return;
        }
        vfxExplosion.Stop();

        if (vfxMiss == null)
        {
            Debug.Log("VFX ЮЊПе!");
            return;
        }
        vfxMiss.Stop();

    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) | GameObject.Find("MainAffector").GetComponent<Curse>().isShoot) & GameObject.Find("MainAffector").GetComponent<Curse>().isGameContinue)
        {
            if (isShootedTarget())
            {
                //Debug.Log(1);
                FindObjectOfType<AudioManager>().Play("xiu");
                Vector3 point = GameObject.Find("Affector").transform.position;
                explotion.transform.localPosition = new Vector3(point.x, point.y, -2.46f);
                GameObject.Find("ShootLight").transform.localPosition = new Vector3(point.x, point.y, -2 - 2.46f);
                aimMesh.SetActive(false);
                vfxExplosion.SendEvent("OnPlay1");
                FindObjectOfType<AudioManager>().Play("explosion");
                playableDirector.Play();
                winUI.SetActive(true);

            }
            else
            {
                FindObjectOfType<AudioManager>().Play("xiu");
                //Debug.Log(2);
                Vector3 point = GameObject.Find("Affector").transform.position;
                missExplotion.transform.localPosition = new Vector3(point.x, point.y, -2.46f);
                GameObject.Find("ShootLight").transform.localPosition = new Vector3(point.x, point.y, -2 - 2.46f);
                vfxMiss.SendEvent("miss");
                //FindObjectOfType<AudioManager>().Play("explosion");
                aimMesh.SetActive(false);
                MissDirector.Play();
                loseUI.SetActive(true);
                targetCube.SetActive(true);

            }
            GameObject.Find("MainAffector").GetComponent<Curse>().isRadarPlay = false;
            GameObject.Find("MainAffector").GetComponent<Curse>().isGameContinue = false;
            StartCoroutine(toScense0());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            vfxExplosion.Stop();
        }
    }
    bool isShootedTarget()
    {
        float targetDistance = GameObject.Find("MainAffector").GetComponent<Curse>().targetDistance;
        if (targetDistance < 0.5f)
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    IEnumerator toScense0()
    {

        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(0);
    }

}
