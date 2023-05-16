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
            Debug.Log("VFX Ϊ��!");
            return;
        }
        vfxExplosion.Stop();

        if (vfxMiss == null)
        {
            Debug.Log("VFX Ϊ��!");
            return;
        }
        vfxMiss.Stop();

    }

    void Update()
    {
        shootBool shoot_state = GameObject.Find("ShootPoint").GetComponent<Curse>().shoot_state;
        int total_counts = GameObject.Find("ShootPoint").GetComponent<Curse>().total_counts;
        bool is_shoot = false;

        for(int i = 0; i < total_counts && i < shoot_state.shoot.Length; ++i)
        {
            is_shoot |= shoot_state.shoot[i];
        }
        if ((Input.GetKeyDown(KeyCode.Space) | is_shoot) & GameObject.Find("ShootPoint").GetComponent<Curse>().isGameContinue)
        {
            List<GameObject> shootPointsList = GameObject.Find("ShootPoint").GetComponent<Curse>().shootPointsList;
            if (isShootedTarget())
            {
                Debug.Log(1);
                FindObjectOfType<AudioManager>().Play("xiu");
                Vector3 point = shootPointsList[0].transform.position;
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
                Debug.Log(2);
                Vector3 point = shootPointsList[0].transform.position;
                missExplotion.transform.localPosition = new Vector3(point.x, point.y, -2.46f);
                GameObject.Find("ShootLight").transform.localPosition = new Vector3(point.x, point.y, -2 - 2.46f);
                vfxMiss.SendEvent("miss");
                //FindObjectOfType<AudioManager>().Play("explosion");
                aimMesh.SetActive(false);
                MissDirector.Play();
                loseUI.SetActive(true);
                targetCube.SetActive(true);

            }
            GameObject.Find("ShootPoint").GetComponent<Curse>().isRadarPlay = false;
            GameObject.Find("ShootPoint").GetComponent<Curse>().isGameContinue = false;
            StartCoroutine(toScense0());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            vfxExplosion.Stop();
        }
    }
    bool isShootedTarget()
    {
        float[] targetDistance = GameObject.Find("ShootPoint").GetComponent<Curse>().targetDistance;
        int total_counts = GameObject.Find("ShootPoint").GetComponent<Curse>().total_counts;
        bool success = false;
        for(int i = 0; i < total_counts; ++i){
            success |= (targetDistance[i] < 0.5f);
        }
        return success;
    }

    IEnumerator toScense0()
    {

        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(0);
    }

}
