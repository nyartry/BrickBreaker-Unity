//using UnityEngine;

//public class BricksGenerator : MonoBehaviour
//{
//	[SerializeField] private Texture2D levelMap;
//	[SerializeField] private GameObject greenBlockPrefab;
//	[SerializeField] private GameObject blackBlockPrefab;
//	[SerializeField] private float blockSize = 100f;

//	private Vector2 startPosition;
//	private const float PIXEL_TO_WORLD = 0.01f; // 100px → 1 unit
//	private float halfBlockOffset;

//	void Start()
//	{
//		CalculateStartPosition();
//		halfBlockOffset = blockSize * 0.5f * PIXEL_TO_WORLD; // 事前計算
//		GenerateBricks();
//	}

//	void CalculateStartPosition()
//	{
//		Camera camera = Camera.main;
//		if(camera == null)
//		{
//			Debug.LogError("Main Camera not found!");
//			return;
//		}

//		float cameraHeight = camera.orthographicSize; // 縦のワールド範囲
//		float cameraWidth = cameraHeight * camera.aspect; // 横のワールド範囲

//		// 左上のワールド座標を計算
//		startPosition = new Vector2(-cameraWidth, -cameraHeight);
//	}


//	void GenerateBricks()
//	{
//		if(levelMap == null)
//		{
//			Debug.LogError("Level map image is not set.");
//			return;
//		}

//		float referenceSize = 10f;
//		float scaleFactor = referenceSize / Camera.main.orthographicSize;
//		float scaleFactor2 = Camera.main.orthographicSize / referenceSize;
//		float adjustedBlockSize = blockSize * scaleFactor;
//		float pixelStep = adjustedBlockSize;

//		// カメラ情報取得
//		float camHeight = Camera.main.orthographicSize * 2f;
//		float camWidth = camHeight * Camera.main.aspect;
//		float baseBlockSize = 100f; // px基準       
//		// ワールド座標左上を起点とする
//		Vector2 topLeft = new Vector2(
//			Camera.main.transform.position.x - camWidth / 2f,
//			Camera.main.transform.position.y + camHeight / 2f
//		);
//		// ピクセル -> ワールドのスケール係数
//		float pixelToWorld = camWidth / levelMap.width;

//		for(int x = 0; x < levelMap.width; x += (int)pixelStep)
//		{
//			for(int y = 0; y < levelMap.height; y += (int)pixelStep)
//			{
//				int px = (int)(x * scaleFactor2);
//				int py = (int)(y * scaleFactor2);
//				// 1ブロックが占めるピクセル幅
//				int stepPx = Mathf.RoundToInt(baseBlockSize);
//				Vector2 worldPos = GetWorldPosition(topLeft, x, y, pixelToWorld, stepPx);

//				if(IsColorPresent(levelMap, px, py, (int)pixelStep, Color.green))
//				{
//					Instantiate(greenBlockPrefab, worldPos, Quaternion.identity, transform);
//					continue;
//				}
//				if(IsColorPresent(levelMap, px, py, (int)pixelStep, Color.black))
//				{
//					Instantiate(blackBlockPrefab, worldPos, Quaternion.identity, transform);
//					continue;
//				}
//			}
//		}
//	}


//	Vector2 GetWorldPosition(Vector2 topLeft, int px, int py, float pixelToWorld, int stepPx)
//	{
//		float worldX = topLeft.x + (px + stepPx / 2f) * pixelToWorld;
//		float worldY = topLeft.y - (levelMap.height - (py + stepPx / 2f)) * pixelToWorld;
//		return new Vector2(worldX, worldY);
//	}

//	bool IsColorPresent(Texture2D texture, int startX, int startY, int size, Color targetColor, float tolerance = 0.01f)
//	{


//		float referenceSize = 10f;
//		float scaleFactor2 = Camera.main.orthographicSize / referenceSize;
//		int startX2 = (int)(startX / scaleFactor2);
//		int startY2 = (int)(startY / scaleFactor2);

//		int endX = startX2 + size;
//		int endY = startY2 + size;
//		for(int x = startX2; x < endX; x++)
//		{
//			for(int y = startY2; y < endY; y++)
//			{
//				if(ColorMatch(texture.GetPixel(x, y), targetColor, tolerance))
//				{
//					return true;
//				}
//			}
//		}
//		return false;
//	}

//	bool ColorMatch(Color a, Color b, float tolerance)
//	{
//		return Mathf.Abs(a.r - b.r) < tolerance &&
//			   Mathf.Abs(a.g - b.g) < tolerance &&
//			   Mathf.Abs(a.b - b.b) < tolerance;
//	}
//}


using UnityEngine;

public class BricksGenerator : MonoBehaviour
{
	[SerializeField] private Texture2D levelMap;
	[SerializeField] private GameObject greenBlockPrefab;
	[SerializeField] private GameObject blackBlockPrefab;
	[SerializeField] private float blockSize = 100f;

	private Vector2 startPosition;
	private float halfBlockOffset;

	private const float PIXEL_TO_WORLD = 0.01f;
	private const float REFERENCE_SIZE = 10f;
	private const float COLOR_TOLERANCE = 0.01f;

	private Camera mainCamera;

	void Start()
	{
		mainCamera = Camera.main;
		if(mainCamera == null)
		{
			Debug.LogError("Main Camera not found!");
			return;
		}

		halfBlockOffset = blockSize * 0.5f * PIXEL_TO_WORLD;
		CalculateStartPosition();
		GenerateBricks();
	}

	private void CalculateStartPosition()
	{
		float cameraHeight = mainCamera.orthographicSize;
		float cameraWidth = cameraHeight * mainCamera.aspect;
		startPosition = new Vector2(-cameraWidth, -cameraHeight);
	}

	private void GenerateBricks()
	{
		if(levelMap == null)
		{
			Debug.LogError("Level map image is not set.");
			return;
		}

		float scaleFactor = REFERENCE_SIZE / mainCamera.orthographicSize;
		float scaleFactorInverse = 1f / scaleFactor;
		float adjustedBlockSize = blockSize * scaleFactor;
		float pixelStep = adjustedBlockSize;

		float camHeight = mainCamera.orthographicSize * 2f;
		float camWidth = camHeight * mainCamera.aspect;
		Vector2 topLeft = new Vector2(
			mainCamera.transform.position.x - camWidth / 2f,
			mainCamera.transform.position.y + camHeight / 2f
		);
		float pixelToWorld = camWidth / levelMap.width;
		int baseBlockPxSize = Mathf.RoundToInt(blockSize);

		for(int x = 0; x < levelMap.width; x += (int)pixelStep)
		{
			for(int y = 0; y < levelMap.height; y += (int)pixelStep)
			{
				int px = Mathf.FloorToInt(x * scaleFactorInverse);
				int py = Mathf.FloorToInt(y * scaleFactorInverse);
				Vector2 worldPos = GetWorldPosition(topLeft, x, y, pixelToWorld, baseBlockPxSize);

				if(IsColorPresent(px, py, (int)pixelStep, Color.green))
				{
					Instantiate(greenBlockPrefab, worldPos, Quaternion.identity, transform);
				}
				else if(IsColorPresent(px, py, (int)pixelStep, Color.black))
				{
					Instantiate(blackBlockPrefab, worldPos, Quaternion.identity, transform);
				}
			}
		}
	}

	private Vector2 GetWorldPosition(Vector2 topLeft, int px, int py, float pixelToWorld, int stepPx)
	{
		float worldX = topLeft.x + (px + stepPx / 2f) * pixelToWorld;
		float worldY = topLeft.y - (levelMap.height - (py + stepPx / 2f)) * pixelToWorld;
		return new Vector2(worldX, worldY);
	}

	private bool IsColorPresent(int startX, int startY, int size, Color targetColor)
	{
		float scaleFactor = mainCamera.orthographicSize / REFERENCE_SIZE;
		int scaledStartX = Mathf.FloorToInt(startX / scaleFactor);
		int scaledStartY = Mathf.FloorToInt(startY / scaleFactor);
		int endX = scaledStartX + size;
		int endY = scaledStartY + size;

		for(int x = scaledStartX; x < endX; x++)
		{
			for(int y = scaledStartY; y < endY; y++)
			{
				if(x >= 0 && x < levelMap.width && y >= 0 && y < levelMap.height &&
					ColorMatch(levelMap.GetPixel(x, y), targetColor))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool ColorMatch(Color a, Color b)
	{
		return Mathf.Abs(a.r - b.r) < COLOR_TOLERANCE &&
			   Mathf.Abs(a.g - b.g) < COLOR_TOLERANCE &&
			   Mathf.Abs(a.b - b.b) < COLOR_TOLERANCE;
	}
}
