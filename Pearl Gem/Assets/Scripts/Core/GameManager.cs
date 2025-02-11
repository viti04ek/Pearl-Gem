using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Color> BallColors; 
    
    private void Awake()
    {
        Sevices.Initialize();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }
}
