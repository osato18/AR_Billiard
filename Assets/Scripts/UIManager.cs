using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using TMPro;
public class UImanager : MonoBehaviour
{
    [SerializeField] private BilliardRule _billiardRule;
    [SerializeField] private TextMeshProUGUI _lifeText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void LifeTextChanger(TextMeshProUGUI lifeText, int lifeData)
    {
        lifeText.text = "Lives : " + lifeData.ToString();
    }
    private void ScoreTextChanger(TextMeshProUGUI scoreText, int scoreData)
    {
        scoreText.text = "Lives : " + scoreData.ToString();
    }
    void Start()
    {
        _lifeText.text = "Lives : " + _billiardRule.PlayerLife.ToString();
        _scoreText.text = "Score : " + _billiardRule.Score.ToString();

        _billiardRule.LifeSub.Subscribe(Life =>
        {
            LifeTextChanger(_lifeText, Life);
        }).AddTo(this);

        _billiardRule.ScoreSub.Subscribe(Score =>
        {
            ScoreTextChanger(_scoreText, Score);
        }).AddTo(this);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
