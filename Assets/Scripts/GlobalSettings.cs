using Pixelplacement;
using UnityEngine;

public class GlobalSettings : Singleton<GlobalSettings>
{
    [Header("Root")]
    public Vector2 BeaversSpawnRangeSeconds = new Vector2(5f, 10f);
    public Vector2 GrowRangeSeconds = new Vector2(5f, 10f);

    public float RootInitialHealth = 50;
    public float LevelUpRootHealing = 50;

    public int HouseDamage = 5;

    public float DamageByHands = 10;
    public float DamageByShovel = 10;
    public float DamageByAxe = 10;

    [Header("Beaver")] 
    public float BeaverSpeed = 2;

    [Header("Character")] 
    public float CharacterSpeed = 0.1f;
    public int JumpForce = 200;
    public float FreezeDurationSeconds = 1f;

    [Header("Bobrolet")]
    public Vector2 RootsSpawnRangeSecondsStart = new Vector2(3f, 5f);
    public Vector2 RootsSpawnRangeSecondsEnd = new Vector2(1f, 2f);
    public Vector2 BobroletSpeedRange = new Vector2(3f, 5f);
    public float BobroletDefaultSpeed = 3f;
    public int BobroletInitialHP = 10;
        
    [Header("General")] 
    public int InitialCoins = 100;
    public int CoinsForBeaver = 20;
    public int CoinsForRoot = 10;
    public int ScoreForBeaver = 20;
    public int ScoreForRoot = 10;
    public int ScoreForSecond = 10;
    
    [Header("Game Tempo")]
    public int NeededScore = 500;
    public AnimationCurve TempoCurve;
    public int MaxBeaverCount = 5;
}
