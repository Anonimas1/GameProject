using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreTextScript : MonoBehaviour
{
    [SerializeField]
    private ScoreTracker _scoreTracker;

    private TMP_Text _text; 
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        _text.text = $"score: {_scoreTracker.Score.ToString()}";
    }
}
