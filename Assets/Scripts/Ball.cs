using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
	private Rigidbody2D ballBody;
	private Paddle paddle;

	[SerializeField]
	float ballSpeed;

	[SerializeField]
	private GameObject ballPrefab;


	public UnityEvent onDivision;

	public Vector3 Position
	{
		get
		{
			return transform.position;
		}
	}

	void Start()
	{
		paddle = GameObject.FindObjectOfType<Paddle>();
		paddle.onItemCollectedDivision.AddListener(() => { Division(); });

		ballBody = GetComponent<Rigidbody2D>();
	}

	public void AddForce(Vector2 force)
	{
		ballBody.AddForce(force, ForceMode2D.Impulse);
	}

	public void FirstServeBall(Vector3 directorPos)
	{
		Vector3 ballPosition = Camera.main.WorldToScreenPoint(transform.position);

		Vector3 direction = (directorPos - ballPosition);
		direction = direction.normalized;

		Vector2 force = new Vector2(direction.x * ballSpeed, direction.y * ballSpeed);

		AddForce(force);
	}

	public void CheckVelocity()
	{
		float minSpeed = 20.0f; // 最低速度

		// 速度ベクトルの方向を維持しつつ、最低速度を保証
		if(ballBody.velocity.magnitude < minSpeed)
		{
			ballBody.velocity = ballBody.velocity.normalized * minSpeed;
		}
	}

	void FixedUpdate()
	{
		CheckVelocity();
	}

	public void Division()
	{
		if(ballPrefab == null)
		{
			return; // プレハブが未設定なら処理しない
		}

		// 現在のボールの位置と進行方向
		Vector3 currentPosition = transform.position;
		Vector2 currentVelocity = ballBody.velocity.normalized; // 進行方向を取得（正規化）

		// 左右に分かれる角度
		float splitAngle = 10f;

		// 左に分身
		CreateSplitBall(currentPosition, currentVelocity, -splitAngle);

		// 右に分身
		CreateSplitBall(currentPosition, currentVelocity, splitAngle);
	}

	// 新しいボールを作成する関数
	private void CreateSplitBall(Vector3 position, Vector2 direction, float angle)
	{
		Quaternion rotation = Quaternion.Euler(0, 0, angle); // 指定した角度回転
		Vector2 newDirection = rotation * direction; // 進行方向を回転

		// もし newDirection の大きさがほぼ 0 ならランダム方向を設定
		if(newDirection.magnitude < 0.1f)
		{
			float randomAngle = Random.Range(-45f, 45f); // -45°～45° のランダム角度
			newDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.up; // 上向きを基準に回転
		}

		GameObject newBall = Instantiate(ballPrefab, transform.parent);

		Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
		if(rb != null)
		{
			rb.velocity = newDirection * 20.0f;
		}
	}
}
