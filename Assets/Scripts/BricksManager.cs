using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BricksManager : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField]
	private int numOfBrickableBricks;

	public int NumOfBrickableBricks
	{
		get => numOfBrickableBricks;
		set => numOfBrickableBricks = value;
	}
	//private GameManager gameManager;
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
        //gameManager = GameObject.FindObjectOfType<GameManager>();
        audioManager = AudioManager.Instance.GetComponent<AudioManager>();
        soundNotPlayed = true;
       
    }
    
    // Update is called once per frame
    void Update()
    {
        if (numOfBrickableBricks <= 0)
        {
            if (soundNotPlayed)
            {
                soundNotPlayed = false;
                audioManager.PlayClearingLevelAudio();

			}
			onAllBricksDestroyed?.Invoke();
			//gameManager.OnWinning();
            
        }
    }
    //void CreateNewBrick()
}
