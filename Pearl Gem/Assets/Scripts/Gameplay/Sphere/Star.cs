using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Star : MonoBehaviour
{
    private const float SpinDuration = 3f;
    private readonly Vector3 FinalScale = Vector3.one;
    private const float SlowdownTime = 1.5f;
    private const string ShooterBallKey = "ShooterBall";
    private const string GoldColor = "#FFD700";
    private const string StandardShaderKey = "Standard";
    
    private void AnimateStar(System.Action onComplete)
    {
        gameObject.transform.parent = null;
        transform.rotation = Quaternion.identity;
        if (TryGetComponent<Renderer>(out var renderer))
        {
            renderer.material.shader = Shader.Find(StandardShaderKey);
            renderer.material.color =
                ColorUtility.TryParseHtmlString(GoldColor, out var newColor) ? newColor : Color.yellow;
        }

        transform.DORotate(new Vector3(0, 720, 0), SpinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                onComplete?.Invoke();

                transform.DOScale(FinalScale, SlowdownTime)
                    .SetEase(Ease.OutBack);
            }); 
    }

    private void OnStarAnimationComplete()
    {
        Services.GameManager.HitStar();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ShooterBallKey) && !collision.gameObject.GetComponent<ShooterBall>().HasCollided)
        {
            AnimateStar(OnStarAnimationComplete);
        }
    }
}
