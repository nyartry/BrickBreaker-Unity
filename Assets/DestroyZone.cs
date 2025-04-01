using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 衝突してきたオブジェクト（例：Ball）を削除
		Destroy(collision.gameObject);
	}
}
