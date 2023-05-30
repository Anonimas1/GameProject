using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextScript : MonoBehaviour
{
    [SerializeField]
    private ScoreTracker _scoreTracker;

    [SerializeField]
    private TMP_Text _text;

    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private TMP_Text _multi;

    void Start()
    {
        _slider.maxValue = _scoreTracker.MultiplyerResetTime;
    }

    void Update()
    {
        _text.text = $"SCORE: {_scoreTracker.Score.ToString()}";
        _multi.text = $"X{_scoreTracker.Multiplyer}";
        _slider.value = _scoreTracker.TimeUntilMulitplyerReset;
    }
}