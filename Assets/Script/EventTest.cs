using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;
public class EventTest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GameManager;
    public VisualEffect vfxExplosion;
    public VisualEffect vfxMiss;
    public PlayableDirector playableDirector;
    public PlayableDirector MissDirector;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] Transform explotion;
    [SerializeField] Transform missExplotion;
    [SerializeField] GameObject aimMesh;

    int total_counts;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.GetComponent<GameManager>();
        total_counts = gameManager.total_counts;
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
        bool key_pressed = Input.GetKeyDown(KeyCode.Space);
        shootBool shoot_state = GameObject.Find("ShootPoint").GetComponent<Curse>().shoot_state;
        bool is_shoot = key_pressed;
        List<GameObject> shootPointsList = GameObject.Find("ShootPoint").GetComponent<Curse>().shootPointsList;
        bool isGameContinue = GameObject.Find("ShootPoint").GetComponent<Curse>().isGameContinue;

        for(int i = 0; i < total_counts && i < shoot_state.shoot.Length; ++i)
        {
            is_shoot |= shoot_state.shoot[i];
        }

        for(int i = 0; is_shoot && isGameContinue && i < total_counts; ++i)
        {
            GameObject targetPos = gameManager.targetPosList[i];
            Vector3 point = shootPointsList[i].transform.position;
            targetPos.SetActive(true);

            if(i == 0 && key_pressed)
            {
                /* go on. */
            }
            else if(i > shoot_state.shoot.Length || !shoot_state.shoot[i])
            {
                continue;
            }

            GameObject.Find("ShootLight").transform.localPosition = new Vector3(point.x, point.y, -2 -4.35f);
            if (isShootedTarget(i))
            {
                explotion.transform.localPosition = new Vector3(point.x, point.y, -4.35f);
                vfxExplosion.SendEvent("OnPlay1");
                FindObjectOfType<AudioManager>().Play("explosion");
                playableDirector.Play();
                targetPos.SetActive(false);
                winUI.SetActive(true);
            }
            else
            {
                missExplotion.transform.localPosition = new Vector3(point.x, point.y, -4.35f);
                vfxMiss.SendEvent("miss");
                MissDirector.Play();
                loseUI.SetActive(true);
            }
        }

        if(is_shoot && isGameContinue)
        {
            aimMesh.SetActive(false);
            GameObject.Find("ShootPoint").GetComponent<Curse>().isRadarPlay = false;
            GameObject.Find("ShootPoint").GetComponent<Curse>().isGameContinue = false;
            FindObjectOfType<AudioManager>().Play("xiu");
            StartCoroutine(toScense0());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            vfxExplosion.Stop();
        }
    }
    bool isShootedTarget(int index = 0)
    {
        float[] targetDistance = GameObject.Find("ShootPoint").GetComponent<Curse>().targetDistance;
        return targetDistance[index] < 0.5f;
    }

    IEnumerator toScense0()
    {

        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(0);
    }

}
