using UnityEngine;

public class BricksGenerator : MonoBehaviour
{
	[SerializeField] private Texture2D levelMap;
	[SerializeField] private GameObject greenBlockPrefab;
	[SerializeField] private GameObject blackBlockPrefab;
	[SerializeField] private float blockSize = 100f;

	private Vector2 startPosition;
	private const float PIXEL_TO_WORLD = 0.01f; // 100px → 1 unit
	private float halfBlockOffset;

	void Start()
	{
		CalculateStartPosition();
		halfBlockOffset = blockSize * 0.5f * PIXEL_TO_WORLD; // 事前計算
		GenerateBricks();
	}

	void CalculateStartPosition()
	{
		Camera camera = Camera.main;
		if(camera == null)
		{
			Debug.LogError("Main Camera not found!");
			return;
		}

		float cameraHeight = camera.orthographicSize; // 縦のワールド範囲
		float cameraWidth = cameraHeight * camera.aspect; // 横のワールド範囲

		// 左上のワールド座標を計算
		startPosition = new Vector2(-cameraWidth, -cameraHeight);
	}

	void GenerateBricks()
	{
		if(levelMap == null)
		{
			Debug.LogError("Level map image is not set.");
			return;
		}
		//ブロックサイズ分づつ、画像に色があるか確認する
		for(int x = 0; x < levelMap.width; x += (int)blockSize)
		{
			for(int y = 0; y < levelMap.height; y += (int)blockSize)
			{
				Vector2 worldPos = GetWorldPosition(x, y);

				if(IsColorPresent(levelMap, x, y, (int)blockSize, Color.green))
				{
					Instantiate(greenBlockPrefab, worldPos, Quaternion.identity, transform);
					continue;
				}
				if(IsColorPresent(levelMap, x, y, (int)blockSize, Color.black))
				{
					Instantiate(blackBlockPrefab, worldPos, Quaternion.identity, transform);
					continue;
				}
			}
		}
	}

	Vector2 GetWorldPosition(int x, int y)
	{
		return startPosition + new Vector2(x, y) * PIXEL_TO_WORLD + Vector2.one * halfBlockOffset;
	}

	bool IsColorPresent(Texture2D texture, int startX, int startY, int size, Color targetColor, float tolerance = 0.01f)
	{
		int endX = startX + size;
		int endY = startY + size;
		for(int x = startX; x < endX; x++)
		{
			for(int y = startY; y < endY; y++)
			{
				if(ColorMatch(texture.GetPixel(x, y), targetColor, tolerance))
				{
					return true;
				}
			}
		}
		return false;
	}

	bool ColorMatch(Color a, Color b, float tolerance)
	{
		return Mathf.Abs(a.r - b.r) < tolerance &&
			   Mathf.Abs(a.g - b.g) < tolerance &&
			   Mathf.Abs(a.b - b.b) < tolerance;
	}
}
