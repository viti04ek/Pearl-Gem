using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimController : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _shooterBallPrefab;
    [SerializeField] private GameObject _shooterRainbowBallPrefab;
    [SerializeField] private LineRenderer _aimLine;
    
    public Color BallColor { get; private set; }
    public Color NextBallColor { get; private set; }
    
    private Camera _mainCamera;
    private bool _isAiming = false;
    private Vector3 _aimDirection;
    private GameObject _currentBall;
    private Rigidbody _ballRigidbody;
    
    private const float MaxAimAngle = 60f;
    private const int TrajectorySteps = 30;
    private const float TimeStep = 0.05f;

    private void Start()
    {
        _mainCamera = Camera.main;
        _aimLine.positionCount = TrajectorySteps;
        _aimLine.enabled = false;
        GenerateBallsColor();
        GenerateNextBall();
    }

    private void Update()
    {
        HandleAiming();
    }

    private void HandleAiming()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
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

            _aimDirection.y = Mathf.Clamp(_aimDirection.y, -Mathf.Sin(MaxAimAngle * Mathf.Deg2Rad), 
                Mathf.Sin(MaxAimAngle * Mathf.Deg2Rad));

            DrawTrajectory(_aimDirection);
        }

        if (Input.GetMouseButtonUp(0) && _isAiming && !IsPointerOverUI())
        {
            Shoot();
            _isAiming = false;
            _aimLine.enabled = false;
        }
    }
    
    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;
        
        if (Application.isEditor)
            return EventSystem.current.IsPointerOverGameObject();

        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    private void Shoot()
    {
        if (_currentBall == null) 
            return;
        
        if (_currentBall.GetComponent<ShooterRainbowBall>())
            Services.UIManager.ActivateChangeBallBtn();
        else
            Services.GameManager.Shoot();
        
        var shooterBall = _currentBall.GetComponent<ShooterBall>();
        shooterBall.Launch(_aimDirection);

        _currentBall = null;
        GenerateBallsColor(NextBallColor);
        Invoke(nameof(GenerateNextBall), 1f);
        
        Services.GameManager.IsGameFinished();
    }

    private void GenerateBallsColor(Color? currentColor = null)
    {
        BallColor = currentColor ?? Services.GameManager.BallColors[Random.Range(0, Services.GameManager.BallColors.Count)];
        do
        {
            NextBallColor = Services.GameManager.BallColors[Random.Range(0, Services.GameManager.BallColors.Count)];
        } while (ColorUtility.ToHtmlStringRGB(BallColor) == ColorUtility.ToHtmlStringRGB(NextBallColor));
        
        Services.UIManager.UpdateBallsColor();
    }

    private void GenerateNextBall()
    {
        if (_currentBall != null)
            Destroy(_currentBall);

        _currentBall = Instantiate(_shooterBallPrefab, _shootPoint.position, Quaternion.identity);
        _currentBall.GetComponent<ShooterBall>().SetColor(BallColor);

        _ballRigidbody = _currentBall.GetComponent<Rigidbody>();
    }

    public void ChangeColor()
    {
        (BallColor, NextBallColor) = (NextBallColor, BallColor);
        GenerateNextBall();
    }

    private void DrawTrajectory(Vector3 direction)
    {
        _aimLine.material.color = BallColor;
        var startPosition = _shootPoint.position;
        var velocity = direction * (_ballRigidbody.mass * 15f);

        _aimLine.positionCount = TrajectorySteps;

        for (var i = 0; i < TrajectorySteps; i++)
        {
            var time = i * TimeStep;
            var point = startPosition + velocity * time + Physics.gravity * (0.5f * time * time);
            _aimLine.SetPosition(i, point);
        }
    }

    public void GenerateRainbowBall()
    {
        if (_currentBall != null)
            Destroy(_currentBall);

        _currentBall = Instantiate(_shooterRainbowBallPrefab, _shootPoint.position, Quaternion.identity);
        BallColor = Color.white;
        _ballRigidbody = _currentBall.GetComponent<Rigidbody>();
        Services.UIManager.SetRainbowBallImage();
    }
}
