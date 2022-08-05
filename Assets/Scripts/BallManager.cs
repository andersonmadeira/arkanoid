using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    #region Singleton

    private static BallManager _instance;

    public static BallManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    [SerializeField]
    private Ball ballPrefab;

    private Ball initialBall;

    private Rigidbody2D initialBallRigidBody;

    public float initialBallSpeed = 250;

    public List<Ball> Balls { get; set; }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (!GameManager.Instance.hasGameStarted)
        {
            Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);

            initialBall.gameObject.transform.position = ballPosition;

            if (Input.GetMouseButtonDown(0))
            {
                initialBallRigidBody.isKinematic = false;
                initialBallRigidBody.AddForce(new Vector2(0, initialBallSpeed));
                GameManager.Instance.hasGameStarted = true;
            }
        }
    }

    private void Initialize()
    {
        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);

        initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        initialBallRigidBody = initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<Ball>
        {
            initialBall
        };
    }
}
