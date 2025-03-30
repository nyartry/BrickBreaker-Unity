using UnityEngine;

public class WallAutoAligner : MonoBehaviour
{
	public Transform topWall;
	public Transform bottomWall;
	public Transform leftWall;
	public Transform rightWall;

	public float wallThickness = 1f;

	void Start()
	{
		AlignWallsToCamera();
	}

	void AlignWallsToCamera()
	{
		Camera cam = Camera.main;

		Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
		Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

		float width = topRight.x - bottomLeft.x;
		float height = topRight.y - bottomLeft.y;

		// 上
		topWall.position = new Vector3(0, topRight.y + wallThickness / 2f, 0);
		topWall.localScale = new Vector3(width + wallThickness * 2, wallThickness, 1);

		// 下
		bottomWall.position = new Vector3(0, bottomLeft.y - wallThickness / 2f, 0);
		bottomWall.localScale = new Vector3(width + wallThickness * 2, wallThickness, 1);

		// 左
		leftWall.position = new Vector3(bottomLeft.x - wallThickness / 2f, 0, 0);
		leftWall.localScale = new Vector3(wallThickness, height + wallThickness * 2, 1);

		// 右
		rightWall.position = new Vector3(topRight.x + wallThickness / 2f, 0, 0);
		rightWall.localScale = new Vector3(wallThickness, height + wallThickness * 2, 1);
	}

}
