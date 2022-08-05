using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton

    private static Paddle _instance;

    public static Paddle Instance => _instance;

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

    private float initialYPosition;
    private Camera mainCamera;
    private float defaultPaddleWidth = 200;
    private float paddleMinClampFactor = 135;
    private float paddleMaxClampFactor = 410;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = FindObjectOfType<Camera>();
        initialYPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        MovePaddle();
    }

    private void MovePaddle()
    {
        float paddleShift = (defaultPaddleWidth - ((defaultPaddleWidth / 2) * spriteRenderer.size.x)) / 2;
        float leftClamp = paddleMinClampFactor - paddleShift;
        float rightClamp = paddleMaxClampFactor - paddleShift;
        float mouseXPositionInPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mouseXPositionInWorld = mainCamera.ScreenToWorldPoint(new Vector3(mouseXPositionInPixels, 0, 0)).x;
        transform.position = new Vector3(mouseXPositionInWorld, initialYPosition, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRigidBody = other.gameObject.GetComponent<Rigidbody2D>();
            Vector3 contactPoint = other.contacts[0].point;
            Vector3 paddleCenterPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y);

            ballRigidBody.velocity = Vector2.zero;

            float difference = paddleCenterPos.x - contactPoint.x;

            if (contactPoint.x < paddleCenterPos.x)
            {
                ballRigidBody.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRigidBody.AddForce(new Vector2(Mathf.Abs(difference * 200), BallManager.Instance.initialBallSpeed));
            }

        }
    }
}
