using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGenerator : MonoBehaviour
{
    public TerrainGenerator Tg;
    public GameObject Slime;
    public Slimes GreenData;
    public List<Slimes> SlimesData;
    [Tooltip("How Much The Green Slime Chances of spawning will decrease with each platform")] [Range(0f,1f)]
    public float GreenDecrease;

    [Tooltip("The Offset from the Cylender Position to where the Slime should spawn")]
    public Vector3 Offset;
    private Transform LastOne;
    private float SlimesSum;
    private double GreenChances;
    private System.Random RandGen;

    void Start()
    {
        LastOne = null;
        SlimesSum = 1;
        GreenChances = .5;
        RandGen = new System.Random();
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(Tg.LastCylander.position + Offset, .7f);
    //}

    void Update()
    {
        if (LastOne != TerrainGenerator.LastCyl)
        {           
            Vector3 last = TerrainGenerator.LastCyl.position;

            float FirstDegree = UnityEngine.Random.Range(0f, 360f);

            Transform first = Instantiate(Slime, last, new Quaternion(), transform).transform;
           
            first.eulerAngles = new Vector3(-90, 0 , FirstDegree);

            //Debug.Log("first.forward * Offset.z : " + first.forward * Offset.z + "first.up * Offset.y : " + first.up * Offset.y);
           // first.position += first.up * Offset.z + first.forward * Offset.y;
            first.position += first.up * Offset.z + first.forward * Offset.y;

            ChooseSlimeType(first.GetChild(0).GetComponent<SlimeBehavior>());

            Transform second = Instantiate(Slime, first.position, new Quaternion(), transform).transform;

            second.eulerAngles = new Vector3(-90, 0, FirstDegree + 180f);

            second.position += second.up * Offset.z * 2;

            ChooseSlimeType(second.GetChild(0).GetComponent<SlimeBehavior>());

            Transform third = Instantiate(Slime, last, new Quaternion() , transform).transform;

            third.eulerAngles = new Vector3(-90, 0, FirstDegree + 90f);

            third.position += third.up * Offset.z + third.forward * Offset.y;

            ChooseSlimeType(third.GetChild(0).GetComponent<SlimeBehavior>());

            //Manipulating Red Slime Appearance chances
            SlimesSum += GreenDecrease;

            GreenChances = (double)1 / SlimesSum;

            if (SlimesSum == 4)
            {
                SlimesData.Add(SlimesData.Find(slime => slime.name.Contains("Blue")));
            }
        }

        LastOne = TerrainGenerator.LastCyl;
    }

    void ChooseSlimeType(SlimeBehavior slime)
    {
        slime.Tg = Tg;

        double chance = RandGen.NextDouble();
        //Debug.Log("The Chances are : " + chance + " (See if it chances each time)");
      
        if (GreenChances < chance)
        {
            //Spawn red Slime
            int index = UnityEngine.Random.Range(0, SlimesData.Count);
            slime.ChangeType(SlimesData[index]);
        }
        else
        {
            slime.ChangeType(GreenData);
        }
    }
}
