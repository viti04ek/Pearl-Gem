using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRainbowBall : ShooterBall
{
    protected override void OnCollisionEnter(Collision collision)
    {
        if (HasCollided) return;
        
        if (collision.gameObject.CompareTag(BallKey))
        {
            HasCollided = true;
            var otherBall = collision.gameObject.GetComponent<Ball>();
        
            if (otherBall != null)
            {
                Services.GameManager.DestroyPlateau(otherBall);
                Services.GameManager.RegisterSuccessfulHit();
            }
        }
    }
}
