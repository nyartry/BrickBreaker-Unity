using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

	[SerializeField]
	private float Speed;

	private Rigidbody2D body;

	// Start is called before the first frame update
	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		Drop(new Vector3(0, -1, 0));
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void AddForce(Vector2 force)
	{
		body.AddForce(force, ForceMode2D.Impulse);
	}


	public void Drop(Vector3 director)
	{
		Vector3 ballPosition = Camera.main.WorldToScreenPoint(transform.position); // ball position in Screenpoint

		Vector3 direction = director;
		direction = direction.normalized;
		Vector2 force = new Vector2(direction.x * Speed, direction.y * Speed);

		AddForce(force);
	}


	void FixedUpdate()
	{

	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		//バーに当たったら
		var tes = collision.transform.gameObject.GetComponent<Paddle>();
		if(tes)
		{
			Destroy(this.gameObject);
		}
	}
}