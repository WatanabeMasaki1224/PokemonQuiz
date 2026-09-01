using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    private int _score;

    private void Start()
    {
        _score = 0;
        UpdateScoreText();
    }

    public void AddScore()
    {
        _score++;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        _score = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        _scoreText.text = "スコア：" + _score;
    }
}
