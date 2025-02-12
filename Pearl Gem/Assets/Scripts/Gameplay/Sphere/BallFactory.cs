using UnityEngine;

public class BallFactory : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    private string _ballKey = "Ball";

    public GameObject CreateBall(Vector3 position, Color color, Transform parent = null)
    {
        var ball = Instantiate(_ballPrefab, position, Quaternion.identity, parent);

        var ballScript = ball.GetComponent<Ball>();
        ballScript.SetColor(color);

        return ball;
    }
}