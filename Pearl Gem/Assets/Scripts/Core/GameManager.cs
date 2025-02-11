using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<Color> BallColors;

    private int _shots;

    private const int _minLayers = 2;
    private const int _maxLayers = 5;
    
    private void Awake()
    {
        Services.Initialize();
        DontDestroyOnLoad(gameObject);
    }

    public int StartGame()
    {
        var layers = Random.Range(_minLayers, _maxLayers);
        _shots = layers * 5;
        Services.UIManager.UpdateShotsText(_shots);
        return layers;
    }

    public void IsGameFinished(int totalPearls, int knockedPearls)
    {
        if (_shots < 1)
        {
            Debug.Log("Lost");
        }
        
        if (totalPearls == knockedPearls)
        {
            Debug.Log("Game finished");
        }
    }

    public void Shoot()
    {
        _shots--;
        Services.UIManager.UpdateShotsText(_shots);
    }
}
