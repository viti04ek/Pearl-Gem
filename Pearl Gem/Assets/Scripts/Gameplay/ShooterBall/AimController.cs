using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _shooterBallPrefab;
    [SerializeField] private LineRenderer _aimLine;
    private float _maxAimAngle = 60f;
    private int _trajectorySteps = 30;
    private float _timeStep = 0.05f;
    private Camera _mainCamera;
    private bool _isAiming = false;
    private Vector3 _aimDirection;
    private Color _ballColor;
    private GameObject _currentBall;
    private Rigidbody _ballRigidbody;

    private void Start()
    {
        _mainCamera = Camera.main;
        _aimLine.positionCount = _trajectorySteps;
        _aimLine.enabled = false;
        GenerateNextBall();
    }

    private void Update()
    {
        HandleAiming();
    }

    private void HandleAiming()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isAiming = true;
            _aimLine.enabled = true;
        }

        if (_isAiming)
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 10f;

            var worldPos = _mainCamera.ScreenToWorldPoint(mousePos);
            _aimDirection = (worldPos - _shootPoint.position).normalized;

            _aimDirection.y = Mathf.Clamp(_aimDirection.y, -Mathf.Sin(_maxAimAngle * Mathf.Deg2Rad), 
                Mathf.Sin(_maxAimAngle * Mathf.Deg2Rad));

            DrawTrajectory(_aimDirection);
        }

        if (Input.GetMouseButtonUp(0) && _isAiming)
        {
            Shoot();
            _isAiming = false;
            _aimLine.enabled = false;
        }
    }

    private void Shoot()
    {
        if (_currentBall == null) return;

        Services.GameManager.Shoot();
        
        var shooterBall = _currentBall.GetComponent<ShooterBall>();
        shooterBall.Launch(_aimDirection);

        _currentBall = null;
        Invoke(nameof(GenerateNextBall), 1f);
    }

    private void GenerateNextBall()
    {
        if (_currentBall != null) return;

        _ballColor = Services.GameManager.BallColors[Random.Range(0, Services.GameManager.BallColors.Count)];
        _currentBall = Instantiate(_shooterBallPrefab, _shootPoint.position, Quaternion.identity);
        _currentBall.GetComponent<ShooterBall>().SetColor(_ballColor);

        _ballRigidbody = _currentBall.GetComponent<Rigidbody>();
    }

    private void DrawTrajectory(Vector3 direction)
    {
        _aimLine.material.color = _ballColor;
        var startPosition = _shootPoint.position;
        var velocity = direction * (_ballRigidbody.mass * 15f);

        _aimLine.positionCount = _trajectorySteps;

        for (var i = 0; i < _trajectorySteps; i++)
        {
            var time = i * _timeStep;
            var point = startPosition + velocity * time + Physics.gravity * (0.5f * time * time);
            _aimLine.SetPosition(i, point);
        }
    }
}
