using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRainbowBall : ShooterBall
{
    protected override void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided) return;

        _hasCollided = true;

        if (collision.gameObject.CompareTag(StarKey))
        {
            Services.GameManager.HitStar();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag(BallKey))
        {
            var otherBall = collision.gameObject.GetComponent<Ball>();
        
            if (otherBall != null)
            {
                Services.GameManager.DestroyPlateau(otherBall);
                Services.GameManager.RegisterSuccessfulHit();
            }
        }
    }
}
