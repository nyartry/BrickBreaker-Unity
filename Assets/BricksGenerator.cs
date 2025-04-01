
using UnityEngine;

public class BricksGenerator : MonoBehaviour
{
	[SerializeField] private Texture2D levelMap;
	[SerializeField] private GameObject greenBlockPrefab;
	[SerializeField] private GameObject blackBlockPrefab;
	[SerializeField] private float blockSize = 100f;

	private const float REFERENCE_SIZE = 10f;
	private const float COLOR_TOLERANCE = 0.01f;

	private Camera mainCamera;

	private BricksManager BricksManager;    // Keep track of number of brickable bricks remain, move to

	void Start()
	{
		mainCamera = Camera.main;
		if(mainCamera == null)
		{
			Debug.LogError("Main Camera not found!");
			return;
		}
		BricksManager = GameObject.FindObjectOfType<BricksManager>();
		GenerateBricks();
	}

	private void GenerateBricks()
	{
		if(levelMap == null)
		{
			Debug.LogError("Level map image is not set.");
			return;
		}
		// 変換係数（1ピクセル = 0.01ワールド単位とする）
		float pixelToWorld = 0.01f;
		float blockWorldSize = blockSize * pixelToWorld;

		// カメラの表示範囲（ワールド単位）
		float camHeight = mainCamera.orthographicSize * 2f;
		float camWidth = camHeight * mainCamera.aspect;


		// ブロック数（ワールド上で何個入るか）
		int blocksPerRow = Mathf.FloorToInt(camWidth / blockWorldSize);
		int blocksPerColumn = Mathf.FloorToInt(camHeight / blockWorldSize);

		// 左下の開始位置（中央寄せ考慮）
		Vector2 bottomLeft = new Vector2(
			mainCamera.transform.position.x - camWidth / 2f ,
			mainCamera.transform.position.y - camHeight / 2f 
		);
		int scaledBlockSizeX = levelMap.width / blocksPerRow;
		int scaledBlockSizeY = levelMap.height / blocksPerColumn;

		

		// ブロック配置ループ
		for(int x = 0; x < blocksPerRow; x++)
		{
			for(int y = 0; y < blocksPerColumn; y++)
			{
				// テクスチャ上の対応ピクセル位置
				int texX = x * scaledBlockSizeX;
				int texY = y * scaledBlockSizeY;
				Vector2 worldPos = bottomLeft + new Vector2(x * blockWorldSize + 0.5f, y * blockWorldSize + 0.5f);

				if(IsColorPresent(texX, texY, scaledBlockSizeX, scaledBlockSizeY, Color.green))
				{
					Instantiate(greenBlockPrefab, worldPos, Quaternion.identity, transform);
				}
				else if(IsColorPresent(texX, texY, scaledBlockSizeX, scaledBlockSizeY, Color.black))
				{
					Instantiate(blackBlockPrefab, worldPos, Quaternion.identity, transform);
				}
			}
		}

		BricksManager.NumOfBrickableBricks = transform.childCount;
	}


	private Vector2 GetWorldPosition(Vector2 bottomLeft, float px, float py, float pixelToWorld, float stepPx)
	{
		float worldX = bottomLeft.x + (px + stepPx / 2f) * pixelToWorld;
		float worldY = bottomLeft.y + (py + stepPx / 2f) * pixelToWorld;
		return new Vector2(worldX, worldY);
	}

	private bool IsColorPresent(int startX, int startY, int sizeX, int sizeY, Color targetColor)
	{

		int scaledStartX = startX;
		int scaledStartY = startY;
		int endX = scaledStartX + sizeX - 1;
		int endY = scaledStartY + sizeY - 1;

		bool outOfBoundsWarned = false;

		for(int x = scaledStartX; x < endX; x++)
		{
			for(int y = scaledStartY; y < endY; y++)
			{
				if(x < 0 || x >= levelMap.width || y < 0 || y >= levelMap.height)
				{
					if(!outOfBoundsWarned)
					{
						Debug.LogWarning($"IsColorPresent: 範囲外アクセスが発生（scaledStartX: {scaledStartX}, scaledStartY: {scaledStartY}）");
						Debug.LogWarning($"IsColorPresent: 範囲外アクセスが発生（endX: {endX}, endY: {endY}）");
						Debug.LogWarning($"IsColorPresent: 範囲外アクセスが発生（x: {x}, y: {y}）levelMapサイズ: {levelMap.width}x{levelMap.height}");
						outOfBoundsWarned = true;
					}
					continue;
				}

				if(ColorMatch(levelMap.GetPixel(x, y), targetColor))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool ColorMatch(Color a, Color b)
	{
		if(Mathf.Abs(a.a - 0) < COLOR_TOLERANCE)
			return false;
		return Mathf.Abs(a.r - b.r) < COLOR_TOLERANCE &&
			   Mathf.Abs(a.g - b.g) < COLOR_TOLERANCE &&
			   Mathf.Abs(a.b - b.b) < COLOR_TOLERANCE;
	}
}
