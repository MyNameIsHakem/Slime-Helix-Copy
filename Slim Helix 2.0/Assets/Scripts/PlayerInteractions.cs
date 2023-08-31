using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractions : MonoBehaviour
{
    public static bool Dead;
    public ObjectsInteractions Obi;
    public GameObject PlayerDeath, PlayerPoisened;
    public int Lives;
    public bool Poisened { get; private set; }
    [HideInInspector] public int Damage;

    public GameObject RetryScreen;
    public TextMeshProUGUI FinalScore;
    public Material HealthMat;
    [Tooltip("Color Representation of current health : There should only be 3 (the overload will be ignored)")]
    public List<Color> HealthColors;    
    private int CurrentColor;

    [Header("Sounds")]
    public AudioSource PlayerHurt;
    public AudioSource SlimeHurt;

    private Animator anim;
    private LayerMask EnemyLayer;
    private bool OnIt;    


    void Awake()
    {
        anim = GetComponent<Animator>();

        Damage = 1;
        Poisened = false;
        Dead = false;
        OnIt = false;
        EnemyLayer = LayerMask.NameToLayer("Enemy");

        CurrentColor = 0;
        HealthMat.color = HealthColors[CurrentColor];
    }

    public void Hurt()
    {
        Lives--;        

        if (Lives <= 0)
        {
            SaveSystem.SaveData();

            RetryScreenStuff();

            Instantiate(PlayerDeath, transform.position, new Quaternion());

            Dead = true;

            gameObject.SetActive(false);
        }
        else
        {
            PlayerHurt.Play();
            anim.SetTrigger("Hurt");
            CurrentColor++;
            HealthMat.color = HealthColors[CurrentColor];
        }
    }

    private void RetryScreenStuff()
    {
        RetryScreen.SetActive(true);

        if(PlayerData.HighScore == TerrainGenerator.CurScore)
        {
            FinalScore.text = "New High Score :\n" + PlayerData.HighScore;
        }
        else
        {
            FinalScore.text = "Score :\n" + TerrainGenerator.CurScore;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //To Ensure that this isn't called 2 times
        if (!OnIt)
        {
            OnIt = true;

            if (other.gameObject.layer == EnemyLayer)
            {
                SlimeBehavior sb = other.gameObject.GetComponent<SlimeBehavior>();
                
                if (sb.Lives > 1)
                {
                    SlimeHurt.Play();
                }
                else
                {
                    Obi.AddCoin();
                }

                sb.Hurt(Damage);
            }

            OnIt = false;
        }
      
    }

    public void AddLive()
    {
        Lives++;
        CurrentColor--;
        HealthMat.color = HealthColors[CurrentColor];
    }


    public IEnumerator PoisenedEffect(float time)
    {
        Damage = 1000;
        Poisened = true;
        PlayerPoisened.SetActive(true);

        yield return new WaitForSeconds(time);

        Poisened = false;
        Damage = 1;
        PlayerPoisened.SetActive(false);
    }

    
  
}
