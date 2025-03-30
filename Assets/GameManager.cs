using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
	Trajectory trajectory;
	LevelManager levelManager;
	Ball ball;
	[SerializeField] float ballSpeed;

	private Vector3 mousePosition;
	float zDept;
	bool ballServed = false;

	[SerializeField] private LoseCondition loseCondition;
	[SerializeField] private BricksManager bricksManager;
	[SerializeField] UnityEvent onWin;
	[SerializeField] UnityEvent onLose;
	void Start()
	{
		ball = FindObjectOfType<Ball>();
		levelManager = FindObjectOfType<LevelManager>();
		trajectory = FindObjectOfType<Trajectory>();
		zDept = Camera.main.transform.position.z - transform.position.z;
		mousePosition = new Vector3(0f, 0f, zDept);
		// イベント登録
		onWin.AddListener(HandleWin);
		onLose.AddListener(HandleLose);

		loseCondition.onBallEnter.AddListener(InvokeLose);
		bricksManager.onAllBricksDestroyed.AddListener(InvokeWin);
	}

	public bool IsBallServed()
	{
		return ballServed;
	}

	// Update is called once per frame
	void Update()
	{
		if(!ballServed)
		{
			trajectory.Show();
			mousePosition.x = Input.mousePosition.x;
			mousePosition.y = Input.mousePosition.y;
			trajectory.UpdateDots(Camera.main.WorldToScreenPoint(ball.Position), mousePosition);
			if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				//ServeBall();
				Destroy(GameObject.Find("Instruction Text"));
				Cursor.visible = false;
				ballServed = true;
				trajectory.Hide();
			}

		}
	}

	//void ServeBall()
	//{
	//	ballServed = true;
	//	// Update mouse position at the launch time
	//	mousePosition.x = Input.mousePosition.x;
	//	mousePosition.y = Input.mousePosition.y;

	//	Destroy(GameObject.Find("Instruction Text"));
	//	Cursor.visible = false;

	//	ball.FirstServeBall(mousePosition);
	//}

	//public void OnLosing()
	//{
	//	levelManager.LoadLevel("Lose");
	//}

	//public void OnWinning()
	//{
	//	levelManager.LoadNextLevel();
	//}

	public void InvokeWin()
	{
		onWin?.Invoke();
	}

	public void InvokeLose()
	{
		onLose?.Invoke();
	}

	void HandleWin()
	{
		levelManager.LoadNextLevel();
	}

	void HandleLose()
	{
		levelManager.LoadLevel("Lose");
	}

}
