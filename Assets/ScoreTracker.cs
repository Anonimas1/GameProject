using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
	public int Score;
	public int ScoreToAdd;
	
	public GameObject PanelToActivate;
    // Start is called before the first frame update
    void Start()
    {
        ScoreToAdd = 1;
    }

	public void EnemyKilled(string gameObjectTag)
	{
		if (gameObjectTag == "NonPlayerPlaced")
		{
			Score += ScoreToAdd;
		}
	}
}
