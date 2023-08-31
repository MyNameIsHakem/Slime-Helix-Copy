using System;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    ///Data That will be manipulated by other objects
    public static int HeartNum = 0, C4Num = 0, PoisonNum = 0;
    public static int Coins = 100; 
    public static int HighScore = 0; 

    ///The Data that will be used by the SaveSystem Class
    public int heartNum, c4Num, poisonNum;
    public int coins;
    public int highScore;

    public PlayerData()
    {
        heartNum = HeartNum;
        c4Num = C4Num;
        poisonNum = PoisonNum;
        coins = Coins;
        highScore = HighScore;
    }

    public void Load()
    {
        HeartNum = heartNum;
        C4Num = c4Num;
        PoisonNum = poisonNum;
        Coins = coins;
        HighScore = highScore;
    }
}
