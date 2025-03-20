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
				Vector2 worldPos = new Vector2(startPosition.x + x * 0.01f, startPosition.y + y * 0.01f);

				//Color pixelColor = GetAverageColor(levelMap, x, y, (int)blockSize);

				//if(pixelColor == Color.green)
				//{
				//	Instantiate(greenBlockPrefab, worldPos, Quaternion.identity, transform);
				//}
				//else if(pixelColor == Color.black)
				//{
				//	Instantiate(blackBlockPrefab, worldPos, Quaternion.identity, transform);
				//}


				if(ContainsColor(levelMap, x, y, (int)blockSize, Color.green))
				{
					Instantiate(greenBlockPrefab, worldPos, Quaternion.identity, transform);
				}
				else if(ContainsColor(levelMap, x, y, (int)blockSize, Color.black))
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
	bool ContainsColor(Texture2D texture, int startX, int startY, int size, Color targetColor)
	{
		for(int x = startX; x < startX + size; x++)
		{
			if(x >= texture.width)
				break;

			for(int y = startY; y < startY + size; y++)
			{
				if(y >= texture.height)
					break;

				if(texture.GetPixel(x, y) == targetColor)
				{
					return true; // 1ピクセルでも該当色があれば即座にtrueを返す
				}
			}
		}
		return false; // 1つも該当しなかった場合はfalse
	}

}
