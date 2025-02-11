using System;
using UnityEngine;

public class ShooterBall : Ball
{
    [SerializeField] private Rigidbody _rigidbody;
    public float _speed = 15f;

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

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_ballKey))
        {
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}