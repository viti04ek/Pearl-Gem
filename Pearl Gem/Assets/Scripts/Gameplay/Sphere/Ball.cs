using System;
using UnityEngine;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
    [SerializeField] protected Renderer _ballRenderer;
    [SerializeField] protected Rigidbody _rigidbody;
    public Color BallColor { get; protected set; }
    private const string _deleteTriggerKey = "DeleteTrigger";

    public void SetColor(Color newColor)
    {
        BallColor = newColor;
        _ballRenderer.material.color = BallColor;
    }

    public void IsKinematic(bool value = false)
    {
        _rigidbody.isKinematic = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_deleteTriggerKey))
        {
            Destroy(gameObject);
        }
    }
}
