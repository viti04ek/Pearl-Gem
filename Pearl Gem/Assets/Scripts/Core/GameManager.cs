using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<Color> BallColors;

    [SerializeField] private GameObject _starPrefab;

    private int _shots;
    private SphereController _sphere;
    private int _successfulHits = 0;
    private int _hitsForStar = 4;
    private GameObject _starObject;

    private const int _minLayers = 2;
    private const int _maxLayers = 5;
    
    private void Awake()
    {
        Services.Initialize();
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        _sphere = null;
        _sphere = FindObjectOfType<SphereController>();
        var layers = Random.Range(_minLayers, _maxLayers);
        _shots = layers * 5;
        _sphere.SetLayers(layers);
        Services.UIManager.UpdateShotsText(_shots);
        _starObject = null;
        _successfulHits = 0;
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

        if (_successfulHits >= _hitsForStar)
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
}
