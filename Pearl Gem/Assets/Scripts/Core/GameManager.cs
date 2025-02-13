using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<Color> BallColors;

    [SerializeField] private GameObject _starPrefab;
    
    public int Level { get; private set; }
    public int Coins { get; private set; }
    public int RainbowBallsAmount { get; private set; }

    private int _shots;
    private SphereController _sphere;
    private int _successfulHits = 0;
    private GameObject _starObject;
    private int _pearlsScored;
    
    private const int HitsForStar = 4;
    private const int MinLayers = 2;
    private const int MaxLayers = 5;
    private const int MoneyForWin = 15;
    private const int MoneyForBall = 3;
    private const int AddBalls = 5;
    private const int PriceToContinue = 50;
    
    private void Awake()
    {
        Services.Register(this);
    }

    public void StartGame()
    {
        _sphere = null;
        _sphere = FindObjectOfType<SphereController>();
        var layers = Random.Range(MinLayers, MaxLayers);
        _shots = layers * 5;
        _sphere.SetLayers(layers);
        Services.UIManager.UpdateShotsText(_shots);
        Level = Services.DataManager.LoadField<int>(DataManager.LevelKey);
        Coins = Services.DataManager.LoadField<int>(DataManager.CoinKey);
        RainbowBallsAmount = Services.DataManager.LoadField<int>(DataManager.RainbowBallsKey);
        RainbowBallsAmount = 9;
        Services.UIManager.UpdateRainbowBallsAmountText();
    }

    public void IsGameFinished(int totalPearls = 1, int knockedPearls = 0)
    {
        if (totalPearls == knockedPearls)
        {
            _pearlsScored = totalPearls;
            Win();
        }
        else if (_shots < 1)
        {
            Lose();
        }
    }

    public void DestroyPlateau(Ball ball)
    {
        _sphere.DestroyPlateau(ball);
    }

    public void Shoot()
    {
        _shots--;
        Services.UIManager.UpdateShotsText(_shots);
    }

    public void RegisterSuccessfulHit()
    {
        if (_starObject != null) return;
        
        _successfulHits++;
        Services.UIManager.AddStarBar(_successfulHits - 1);

        if (_successfulHits >= HitsForStar)
        {
            SpawnStar();
        }
    }

    public void ResetHitStreak()
    {
        if (_starObject != null) return;
        
        _successfulHits = 0;
        Services.UIManager.ResetStarBars();
    }

    private void SpawnStar()
    {
        if (_starObject == null)
        {
            Services.UIManager.AddFullStar();
            _starObject = Instantiate(_starPrefab, _sphere.gameObject.transform.position,
                Quaternion.identity, _sphere.gameObject.transform);
        }
    }

    public void HitStar()
    {
        _sphere.DestroyAllBalls();
    }

    private void Win()
    {
        Coins += MoneyForWin + MoneyForBall * _shots;
        Services.UIManager.ShowWinPanel();
        Services.DataManager.SaveField(DataManager.CoinKey, Coins);
        Services.DataManager.SaveField(DataManager.LevelKey, Level + 1);
        Services.DataManager.SaveField(DataManager.LevelPearlScoreKey, _pearlsScored);
    }

    private void Lose()
    {
        Services.UIManager.ShowLosePanel1();
    }

    public bool TryToSelectRainbowBall()
    {
        if (RainbowBallsAmount < 1) 
            return false;

        RainbowBallsAmount--;
        Services.DataManager.SaveField(DataManager.RainbowBallsKey, RainbowBallsAmount);
        return true;
    }

    public bool TryToContinueForMoney()
    {
        if (PriceToContinue > Coins)
            return false;

        Coins -= PriceToContinue;
        ContinueGame();
        return true;
    }

    private void ContinueGame()
    {
        _shots += AddBalls;
        Services.UIManager.ContinueGame();
        Services.UIManager.UpdateShotsText(_shots);
    }
}
