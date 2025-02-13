using System;
using UnityEngine;

public class ShooterBall : Ball
{
    public bool HasCollided { get; protected set; } = false;
    
    protected const float Speed = 15f;
    protected const string BallKey = "Ball";
    protected const string StarKey = "Star";

    private void Start()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
    }

    public void Launch(Vector3 direction)
    {
        _rigidbody.useGravity = true;
        _rigidbody.velocity = direction.normalized * Speed;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (HasCollided) return;
        
        if (collision.gameObject.CompareTag(BallKey))
        {
            HasCollided = true;
            var otherBall = collision.gameObject.GetComponent<Ball>();
        
            if (otherBall != null)
            {
                if (ColorUtility.ToHtmlStringRGB(otherBall.BallColor) == ColorUtility.ToHtmlStringRGB(this.BallColor))
                {
                    Services.GameManager.DestroyPlateau(otherBall);
                    Services.GameManager.RegisterSuccessfulHit();
                }
                else
                {
                    Services.GameManager.ResetHitStreak();
                }
            }
        }
    }
}