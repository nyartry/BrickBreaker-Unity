using UnityEngine;
using UnityEngine.Events;

public class BallManager : MonoBehaviour
{
	[SerializeField] private Transform paddle;      // PaddleのTransform
	[SerializeField] private Ball ball;
	[SerializeField] private Rigidbody2D ballRb;    // BallのRigidbody2D
	[SerializeField] private Vector3 offset = new Vector3(0, 0.5f, 0);
	[SerializeField] private float serveForce = 20.0f;

	[SerializeField]
	private GameObject ballPrefab;

	Trajectory trajectory;
	private Vector3 mousePosition;
	float zDept;

	bool ballServed = false;

	void Start()
	{
		ballRb.isKinematic = true; // 最初はボールを固定

		trajectory = FindObjectOfType<Trajectory>();
		zDept = Camera.main.transform.position.z - transform.position.z;
		mousePosition = new Vector3(0f, 0f, zDept);

	}

	void Update()
	{
		if(!ballServed)
		{
			// Paddleの上にボールを固定
			ball.transform.position = paddle.position + offset;
			trajectory.Show();
			mousePosition.x = Input.mousePosition.x;
			mousePosition.y = Input.mousePosition.y;
			trajectory.UpdateDots(Camera.main.WorldToScreenPoint(ball.Position), mousePosition);
			if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				ServeBall();
			}
		}
	}

	void ServeBall()
	{
		ballServed = true;

		ballRb.isKinematic = false;
		ball.AddForce(new Vector2(1f, 1f).normalized * serveForce);
		trajectory.Hide();
		mousePosition.x = Input.mousePosition.x;
		mousePosition.y = Input.mousePosition.y;

		Cursor.visible = false;
	}

}
