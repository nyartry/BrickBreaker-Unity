using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Paddle : MonoBehaviour
{
	private Ball Ball;

	[SerializeField]
	private GameObject ballPrefab;

	private AudioManager audioManager;
	private GameManager gameManager;
	private float mouseXPos;
	private float ballPos;
	[SerializeField]
	private bool autoPlay;
	private float zDistance, leftCorner, rightCorner;

	// アイテム取得時のイベント
	public UnityEvent onItemCollectedShotgunBurst;
	public UnityEvent onItemCollectedDivision;

	// 下からの距離（ワールド単位）
	public float bottomMargin = 0.5f;

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
		Ball = FindObjectOfType<Ball>();
		audioManager = AudioManager.Instance.GetComponent<AudioManager>();

		// Restrict paddle position
		zDistance = transform.position.z - Camera.main.transform.position.z;
		// 
		Sprite sprite = GetComponent<SpriteRenderer>().sprite;
		leftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zDistance)).x + sprite.bounds.size.x / 2;
		rightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, zDistance)).x - sprite.bounds.size.x / 2;

		onItemCollectedShotgunBurst.AddListener(() => { ShotgunBurst(); });
		PositionPaddle();
	}

	void PositionPaddle()
	{
		Camera cam = Camera.main;
		float zDist = Mathf.Abs(cam.transform.position.z); // カメラからの距離

		Vector3 bottomCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, zDist));
		bottomCenter.y += bottomMargin;
		bottomCenter.z = 0f;

		transform.position = bottomCenter;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Only play sound of collision between ball and paddle when the ball has already been served
		if(gameManager.IsBallServed())
			audioManager.PlayUnbreakableHitAudio();
	}
	// Update is called once per frame
	private void MoveWithMouse()
	{
		// Main gameplay
		// Move paddle with mouse
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = zDistance;
		mouseXPos = Camera.main.ScreenToWorldPoint(mousePos).x;
		Vector3 paddlePos = gameObject.transform.position;
		paddlePos.x = Mathf.Clamp(mouseXPos, leftCorner, rightCorner);
		gameObject.transform.position = paddlePos;
	}
	private void AutomatedPlay()
	{
		// Automated play testing

		ballPos = Ball.transform.position.x;
		Vector3 paddlePos = gameObject.transform.position;
		paddlePos.x = Mathf.Clamp(ballPos, leftCorner, rightCorner);
		gameObject.transform.position = paddlePos;
	}
	void Update()
	{
		if(autoPlay)
		{
			AutomatedPlay();
		}
		else
		{
			MoveWithMouse();
		}
	}

	public void ShotgunBurst()
	{
		for(int i = 0; i < 3; i++)
		{
			float angle = -15f + (15f * i); // -15度, 0度, +15度
			Quaternion rotation = Quaternion.Euler(0, 0, angle); // Z軸回転

			// すべてのボールを最初はバーの中央に配置
			Vector3 spawnPosition = transform.position;
			spawnPosition += new Vector3(0, 0.1f, 0);
			GameObject obj = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

			// Rigidbody2D で方向を設定
			Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
			if(rb != null)
			{
				Vector2 direction = rotation * Vector2.up;
				rb.velocity = direction * 5f; // 速度 5
			}
		}
	}

}
