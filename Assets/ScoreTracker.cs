using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
	public int Score;
	public int ScoreToAdd;
	public int ScoreTarget = 10;

	public GameObject PanelToActivate;
    // Start is called before the first frame update
    void Start()
    {
        ScoreToAdd = 1;
    }

	public void EnemyKilled()
	{
		Score += ScoreToAdd;
		if (Score >= ScoreTarget)
		{
			PanelToActivate.SetActive(true);
		}
		ScoreToAdd *= 2;
	}
}
