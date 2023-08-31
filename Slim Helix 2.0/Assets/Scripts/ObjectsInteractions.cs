using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class ObjectsInteractions : MonoBehaviour
{
    public AudioMixer AudMix;
    private AudioSource HeartUse;

    [Header("Objects Related Stuff")]
    public PlayerInteractions Pi;
    public TerrainGenerator Tg;
    public TextMeshProUGUI Coins;
    public int NewLifeCost;
    [Tooltip("How Much i can have of each item at the same time")]
    public int MaxNum;
    [Tooltip("How Much the Poison effect will last in seconds")]
    public float PoisonTime;
    public TextMeshProUGUI HeartPrice, C4Price, PoisonPrice;
    [Tooltip("The Num Text of each Object in the Before and after Menu")]
    public TextMeshProUGUI[] HeartNum, C4Num, PoisonNum;  

    [Header("Score Related")]
    public TextMeshProUGUI HighScore;
    public TextMeshProUGUI Score;

    void Awake()
    {
        SaveSystem.LoadData();
        UpdateHeartDisplay();
        UpdateC4Display();
        UpdatePoisonDisplay();
        Coins.text = PlayerData.Coins.ToString();
        HighScore.text = "High Score : " + PlayerData.HighScore.ToString();
        HeartUse = GetComponent<AudioSource>();
    }

    #region Buy Objects Event Handlers

    public void BuyHeart_EventHandler()
    {
        int price = int.Parse(HeartPrice.text);

        if (PlayerData.Coins - price >= 0 && PlayerData.HeartNum < MaxNum)
        {          
            PlayerData.HeartNum++;

            UpdateHeartDisplay();

            Buy(price);
        }
    }   

    public void BuyC4_EventHandler()
    {
        int price = int.Parse(C4Price.text);

        if (PlayerData.Coins - price >= 0 && PlayerData.C4Num < MaxNum)
        {
            PlayerData.C4Num++;

            UpdateC4Display();

            Buy(price);
        }
    }

    public void BuyPoison_EventHandler()
    {
        int price = int.Parse(PoisonPrice.text);

        if (PlayerData.Coins - price >= 0 && PlayerData.PoisonNum < MaxNum)
        {
            PlayerData.PoisonNum++;

            UpdatePoisonDisplay();

            Buy(price);
        }
    }

    void Buy(int price)
    {
        PlayerData.Coins -= price;

        Coins.text = PlayerData.Coins.ToString();

        SaveSystem.SaveData();
    }

    #endregion

    #region Use Objects Event Handlers

    public void UseHeart_EventHandler()
    {
        if(PlayerData.HeartNum > 0 && Pi.Lives < 3)
        {
            HeartUse.Play();

            Pi.AddLive();

            PlayerData.HeartNum--;

            UpdateHeartDisplay();

            SaveSystem.SaveData();
        }
    }

    public void UseC4_EventHandler()
    {
        if(PlayerData.C4Num > 0 && !Tg.Spawning)
        {
            Tg.C4Effect();

            PlayerData.C4Num--;

            UpdateC4Display();

            SaveSystem.SaveData();
        }
    }

    public void UsePoison_EventHandler()
    {
        if (PlayerData.PoisonNum > 0 && !Pi.Poisened)
        {
            StartCoroutine(Pi.PoisenedEffect(PoisonTime));

            PlayerData.PoisonNum--;

            UpdatePoisonDisplay();

            SaveSystem.SaveData();
        }
    }

    #endregion

    #region Update Heart, C4, Poison Num Display

    void UpdateHeartDisplay()
    {
        HeartNum[0].text = "x" + PlayerData.HeartNum.ToString();
        HeartNum[1].text = "x" + PlayerData.HeartNum.ToString();
    }
    
    void UpdateC4Display()
    {
        C4Num[0].text = "x" + PlayerData.C4Num.ToString();
        C4Num[1].text = "x" + PlayerData.C4Num.ToString();
    }

    void UpdatePoisonDisplay()
    {
        PoisonNum[0].text = "x" + PlayerData.PoisonNum.ToString();
        PoisonNum[1].text = "x" + PlayerData.PoisonNum.ToString();
    }

    #endregion

    #region Retry Screen Events

    public void Retry_EventHandler()
    {
        SceneManager.LoadScene(0);
    }
    
    public void Continue_EventHandler()
    {
        if(PlayerData.Coins >= NewLifeCost)
        {
            PlayerData.Coins -= NewLifeCost;
            Pi.gameObject.SetActive(true);
            Pi.RetryScreen.SetActive(false);
            Pi.AddLive();
            SaveSystem.SaveData();
        }
    }

    #endregion

    #region Sound Event Handler

    public void Sound_EventHandler(GameObject Disabled)
    {
        if(Disabled.activeSelf == false)
        {
            Disabled.SetActive(true);
            AudMix.SetFloat("Volume", -80);
        }
        else
        {
            Disabled.SetActive(false);
            AudMix.SetFloat("Volume", 12);
        }
    }

    #endregion

    public void AddCoin()
    {
        PlayerData.Coins++;
        Coins.text = PlayerData.Coins.ToString();
    }
}
