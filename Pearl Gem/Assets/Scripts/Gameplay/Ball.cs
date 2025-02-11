using UnityEngine;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
    [SerializeField] private Renderer _ballRenderer;
    public Color BallColor { get; private set; }
    private SphereController _sphereController;
    private string _ballKey = "Ball";

    public void SetColor(Color newColor)
    {
        BallColor = newColor;
        _ballRenderer.material.color = BallColor;
    }

    public void SetSphereController(SphereController sphereController)
    {
        _sphereController = sphereController;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_ballKey))
        {
            var otherBall = collision.gameObject.GetComponent<Ball>();

            if (otherBall != null && otherBall.BallColor == this.BallColor)
            {
                _sphereController.DestroyPlateau(this);
            }
        }
    }
}