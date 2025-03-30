using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BricksManager : MonoBehaviour
{
	[SerializeField]
	private int numOfBrickableBricks;

	public int NumOfBrickableBricks
	{
		get => numOfBrickableBricks;
		set => numOfBrickableBricks = value;
	}
	private AudioManager audioManager;
	private bool soundNotPlayed;

	[SerializeField]
	public UnityEvent onAllBricksDestroyed;


	public void DestroyBrick()
	{
		numOfBrickableBricks--;
	}

	public int RemainBricks()
	{
		return numOfBrickableBricks;
	}

	void Start()
	{
		audioManager = AudioManager.Instance.GetComponent<AudioManager>();
		soundNotPlayed = true;
	}

	void Update()
	{
		if(numOfBrickableBricks <= 0)
		{
			if(soundNotPlayed)
			{
				soundNotPlayed = false;
				audioManager.PlayClearingLevelAudio();

			}
			onAllBricksDestroyed?.Invoke();
		}
	}
}
