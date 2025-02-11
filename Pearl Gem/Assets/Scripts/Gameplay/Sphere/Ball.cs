using UnityEngine;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
    [SerializeField] protected Renderer _ballRenderer;
    public Color BallColor { get; protected set; }
    private SphereController _sphereController;
    protected const string _ballKey = "Ball";

    public void SetColor(Color newColor)
    {
        BallColor = newColor;
        _ballRenderer.material.color = BallColor;
    }

    public void SetSphereController(SphereController sphereController)
    {
        _sphereController = sphereController;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_ballKey))
        {
            var otherBall = collision.gameObject.GetComponent<ShooterBall>();
        
            if (otherBall != null && ColorUtility.ToHtmlStringRGB(otherBall.BallColor) == ColorUtility.ToHtmlStringRGB(this.BallColor))
            {
                _sphereController.DestroyPlateau(this);
            }
        }
    }
}