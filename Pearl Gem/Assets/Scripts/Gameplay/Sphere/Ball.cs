using UnityEngine;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
    [SerializeField] protected Renderer _ballRenderer;
    public Color BallColor { get; protected set; }

    public void SetColor(Color newColor)
    {
        BallColor = newColor;
        _ballRenderer.material.color = BallColor;
    }
}
