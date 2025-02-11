using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    [SerializeField] private BallFactory _ballFactory;
    
    private int _layers = 3;
    private const float LayerSpacing = 0.5f;
    private const float BallSize = 0.2f;
    private const float RotationSpeed = 20f;
    private const float MinRadius = 1f;
    private int _knockedBalls = 0;
    private int _totalBalls = 0;
    private readonly List<Vector3> _plateauCenters = new();
    private readonly Dictionary<Vector3, Color> _plateauMap = new();
    private readonly List<List<GameObject>> _sphereLayers = new();

    private void Start()
    {
        _layers = Services.GameManager.StartGame();
        GenerateSphere();
    }

    private void Update()
    {
        RotateSphere();
    }

    private void GenerateSphere()
    {
        _totalBalls = 0;
        var maxRadius = _layers * LayerSpacing + MinRadius;
        
        for (var layer = 0; layer < _layers; layer++)
        {
            var plateauCount = Random.Range(Services.GameManager.BallColors.Count - 1, Services.GameManager.BallColors.Count + 3);
            
            for (var i = 0; i < plateauCount; i++)
            {
                var randomPoint = Random.onUnitSphere * maxRadius;
                _plateauCenters.Add(randomPoint);
                _plateauMap[randomPoint] = Services.GameManager.BallColors[Random.Range(0, Services.GameManager.BallColors.Count)];
            }
            
            var layerBalls = new List<GameObject>();

            var radius = maxRadius - (layer * LayerSpacing);
            var ballsInLayer = Mathf.RoundToInt(4 * Mathf.PI * Mathf.Pow(radius / BallSize, 2) / 2);

            for (var i = 0; i < ballsInLayer; i++)
            {
                var theta = Mathf.Acos(1 - 2 * (i + 0.5f) / ballsInLayer) * Mathf.Rad2Deg;
                var phi = (i * 137.5f) % 360;

                var position = SphericalToCartesian(radius, theta, phi);

                var closestPlateau = FindClosestPlateau(position);
                var selectedColor = _plateauMap[closestPlateau];

                var ball = _ballFactory.CreateBall(transform.position + position, selectedColor, transform, this);

                layerBalls.Add(ball);
                _totalBalls++;
            }

            _sphereLayers.Add(layerBalls);
        }
    }

    private Vector3 FindClosestPlateau(Vector3 position)
    {
        var closest = _plateauCenters[0];
        var minDistance = Vector3.Distance(position, closest);

        foreach (var center in _plateauCenters)
        {
            var distance = Vector3.Distance(position, center);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = center;
            }
        }

        return closest;
    }

    private Vector3 SphericalToCartesian(float r, float theta, float phi)
    {
        var thetaRad = Mathf.Deg2Rad * theta;
        var phiRad = Mathf.Deg2Rad * phi;

        var x = r * Mathf.Sin(thetaRad) * Mathf.Cos(phiRad);
        var y = r * Mathf.Cos(thetaRad);
        var z = r * Mathf.Sin(thetaRad) * Mathf.Sin(phiRad);

        return new Vector3(x, y, z);
    }
    
    private void RotateSphere()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime, Space.World);
    }
    
    public void DestroyPlateau(Ball hitBall)
    {
        var ballsToDestroy = FindConnectedPlateau(hitBall);
        _knockedBalls += ballsToDestroy.Count;
        Services.UIManager.UpdatePearlsText(_knockedBalls);
        Services.GameManager.IsGameFinished(_totalBalls, _knockedBalls);

        foreach (var ballObj in ballsToDestroy)
        {
            foreach (var layer in _sphereLayers.Where(layer => layer.Contains(ballObj)))
            {
                layer.Remove(ballObj);
                break;
            }

            Destroy(ballObj);
        }
    }

    private List<GameObject> FindConnectedPlateau(Ball startBall)
    {
        var connectedBalls = new List<GameObject>();
        var queue = new Queue<Ball>();
        var visited = new HashSet<Ball>();

        queue.Enqueue(startBall);
        visited.Add(startBall);

        while (queue.Count > 0)
        {
            var currentBall = queue.Dequeue();
            connectedBalls.Add(currentBall.gameObject);

            foreach (var neighborBall in GetNeighbors(currentBall).Select(neighborObj => 
                         neighborObj.GetComponent<Ball>()).Where(neighborBall => neighborBall != null && 
                         neighborBall.BallColor == startBall.BallColor && !visited.Contains(neighborBall)))
            {
                queue.Enqueue(neighborBall);
                visited.Add(neighborBall);
            }
        }

        return connectedBalls;
    }

    private List<GameObject> GetNeighbors(Ball ball)
    {
        var neighbors = new List<GameObject>();

        foreach (var layer in _sphereLayers.Where(layer => layer.Contains(ball.gameObject)))
        {
            neighbors.AddRange(layer.Where(otherBall => otherBall != ball.gameObject && 
                               Vector3.Distance(ball.transform.position, otherBall.transform.position) < 0.35f));
            break;
        }

        return neighbors;
    }
}

