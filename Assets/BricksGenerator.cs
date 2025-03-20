using UnityEngine;

public class BricksGenerator : MonoBehaviour
{
	[SerializeField] private Texture2D levelMap; // 画像をアタッチ
	[SerializeField] private GameObject greenBlockPrefab;
	[SerializeField] private GameObject blackBlockPrefab;
	[SerializeField] private float blockSize = 100f; // ブロックの間隔
	[SerializeField] private Vector2 startPosition = new Vector2(-540, -960); // 左上を基準とする

	void Start()
	{
		GenerateBricks();
	}

	void GenerateBricks()
	{
		if(levelMap == null)
		{
			Debug.LogError("Level map image is not set.");
			return;
		}
		for(int x = 0; x < levelMap.width; x += (int)blockSize)
		{
			for(int y = 0; y < levelMap.height; y += (int)blockSize)
			{
				Color pixelColor = GetAverageColor(levelMap, x, y, (int)blockSize);
				Vector2 worldPos = new Vector2(startPosition.x + x * 0.01f, startPosition.y + y * 0.01f);

				if(pixelColor == Color.green)
				{
					Instantiate(greenBlockPrefab, worldPos, Quaternion.identity, transform);
				}
				else if(pixelColor == Color.black)
				{
					Instantiate(blackBlockPrefab, worldPos, Quaternion.identity, transform);
				}
			}
		}
	}

	Color GetAverageColor(Texture2D texture, int startX, int startY, int size)
	{
		Color sumColor = Color.clear;
		int count = 0;

		for(int x = startX; x < startX + size; x++)
		{
			if(x >= texture.width)
			{
				break; // 画像の幅を超えたら終了
			}

			for(int y = startY; y < startY + size; y++)
			{
				if(y >= texture.height)
				{
					break; // 画像の高さを超えたら終了
				}
				sumColor += texture.GetPixel(x, y);
				count++;
			}
		}

		return count > 0 ? sumColor / count : Color.clear;
	}
}
