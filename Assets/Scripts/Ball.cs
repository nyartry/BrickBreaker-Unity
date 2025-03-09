using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	// Start is called before the first frame update
	private Rigidbody2D ballBody;
	private Paddle paddle;
	private Vector3 ballToPaddle;
	private GameManager gameManager;

	[SerializeField] float ballSpeed;

	public Vector3 Position
	{
		get
		{
			return transform.position;
		}
	}

	void Start()
	{
		gameManager = GameObject.FindObjectOfType<GameManager>();
		paddle = GameObject.FindObjectOfType<Paddle>();
		ballBody = GetComponent<Rigidbody2D>();
	}

	public void AddForce(Vector2 force)
	{
		ballBody.AddForce(force, ForceMode2D.Impulse);
	}

	public void FirstServeBall(Vector3 directorPos)
	{
		Vector3 ballPosition = Camera.main.WorldToScreenPoint(transform.position); // ball position in Screenpoint

		// Caculate the direction
		Vector3 direction = (directorPos - ballPosition);
		direction = direction.normalized;

		Vector2 force = new Vector2(direction.x * ballSpeed, direction.y * ballSpeed);

		AddForce(force);
	}

	public void CheckVelocity()
	{
		float minSpeed = 3f; // 最低速度

		// 速度ベクトルの方向を維持しつつ、最低速度を保証
		if(ballBody.velocity.magnitude < minSpeed)
		{
			ballBody.velocity = ballBody.velocity.normalized * minSpeed;
		}
	}

	void FixedUpdate()
	{
		if(gameManager.IsBallServed())
		{
			CheckVelocity();
		}
	}

}
