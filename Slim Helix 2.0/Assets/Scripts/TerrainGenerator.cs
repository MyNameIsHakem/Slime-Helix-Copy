using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TerrainGenerator : MonoBehaviour
{
    public static Transform LastCyl;
    [Tooltip("The Last Cylander that i instantiated at the start")]
    public Transform LastCylander;
    public LayerMask EnemyLayer;
    public Vector3 EnemiesChecker, EnemiesCheckerSize;
    [Tooltip("How much time the player take falling from a platform to another in seconds")]
    public float FallTime;
    [Header("UI Interaction Stuff")]
    public ObjectsInteractions ObI;
    public GameObject BeforeObjects, AfterObjects;
    public bool Spawning { get; private set; }

    private AudioSource PlatformCrack;
    private List<Transform> Cylinders = new List<Transform>();
    private int CurrentCyl , LastCylIndex;
    [Tooltip("The Distance between the Two Adjacent Cylinders")]
    private float Distance;
    public static int CurScore { get; private set; } 

    void Awake()
    {
        CurrentCyl = 0;

        CurScore = 0;

        Spawning = false;

        LastCylIndex = -1;

        LastCyl = LastCylander;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tran = transform.GetChild(i);

            if (tran.tag == "Cylinder")
            {
                Cylinders.Add(tran);
            }
           
        }
        
        Distance = Cylinders[0].position.y - Cylinders[1].position.y;

        PlatformCrack = GetComponent<AudioSource>();
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(LastCylander.position + EnemiesChecker, EnemiesCheckerSize);
    //}

    public void CheckIfDestroy()
    {
        // Debug.Log("The Detcted Ones are = " + Physics.OverlapBox(Cylinders[CurrentCyl].position + EnemiesChecker, EnemiesCheckerSize, new Quaternion(), EnemyLayer).Length + " And the Current cylinder is Num : " + CurrentCyl);

        if (!Spawning)
        {
            if (Physics.OverlapBox(Cylinders[CurrentCyl].position + EnemiesChecker, EnemiesCheckerSize, new Quaternion(), EnemyLayer).Length <= 0)
            {
                Cylinders[CurrentCyl].GetChild(0).gameObject.SetActive(false);

                PlatformCrack.Play();

                IncreaseScore();

                StartCoroutine(Spawn());
            }
        }       
    }

    IEnumerator Spawn()
    {
        Spawning = true;

        if (LastCylIndex <= 0)
        {
            //This will only be called on the first platform destroy
            Destroy(BeforeObjects);
            AfterObjects.SetActive(true);
        }

        yield return new WaitForSeconds(FallTime);

        ChangeLevel();    

        Spawning = false;
    }

    private void ChangeLevel()
    {
        if (LastCylIndex >= 0)
        {
            //Debug.Log("change object position");

            Cylinders[LastCylIndex].position = LastCyl.position + Vector3.down * Distance;

            Cylinders[LastCylIndex].GetChild(0).gameObject.SetActive(true);

            LastCyl = Cylinders[LastCylIndex];
        }       

        LastCylIndex = CurrentCyl;

        //Debug.Log("I Will Increase CurrentCyl from " + Cur + " to " + (Cur + 1) % Cylinders.Count);

        CurrentCyl = (CurrentCyl + 1) % Cylinders.Count;       
    }

    public void C4Effect()
    {
        Collider[] cols = Physics.OverlapBox(Cylinders[CurrentCyl].position + EnemiesChecker, EnemiesCheckerSize, new Quaternion(), EnemyLayer);
        for(int i = 0; i < cols.Length; i++)
        {
            Destroy(cols[i].transform.parent.gameObject);
        }

        Cylinders[CurrentCyl].GetChild(0).gameObject.SetActive(false);

        PlatformCrack.Play();

        IncreaseScore();

        ChangeLevel();
    }

    void IncreaseScore()
    {
        CurScore++;
        ObI.Score.text = "Score  : " + CurScore;
        if (PlayerData.HighScore < CurScore)
        {
            PlayerData.HighScore = CurScore;
            ObI.HighScore.text = "High Score : " + PlayerData.HighScore.ToString();
        }
    }
}
