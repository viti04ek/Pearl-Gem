using System;
using UnityEngine;

public class ShooterBall : Ball
{
    [SerializeField] private Rigidbody _rigidbody;
    private float _speed = 15f;
    private bool _hasCollided = false;
    private const string _ballKey = "Ball";
    private const string _starKey = "Star";

    private void Start()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
    }

    public void Launch(Vector3 direction)
    {
        _rigidbody.useGravity = true;
        _rigidbody.velocity = direction.normalized * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided) return;

        _hasCollided = true;

        if (collision.gameObject.CompareTag(_starKey))
        {
            Services.GameManager.HitStar();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag(_ballKey))
        {
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