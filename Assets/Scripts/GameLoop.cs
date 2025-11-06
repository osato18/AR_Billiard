using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using TMPro;
public class GameLoop : MonoBehaviour
{
    [Header("BilliardRule")]
    [SerializeField] private BilliardRule _billiardRuleObj;

    [Header("各Canvasオブジェクト")]
    [SerializeField] private GameObject _prepareCanvasObj;
    [SerializeField] private GameObject _gamePlayCanvasObj;
    [SerializeField] private GameObject _gameClearCanvasObj;
    [SerializeField] private GameObject _gameOverCanvasObj;

    [Header("各テキスト")]
    [SerializeField] private TextMeshProUGUI _pocketPointText;
    [SerializeField] private TextMeshProUGUI _lifeBonusText;
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    private void GameStart()
    {
        _prepareCanvasObj.SetActive(false);
        _gamePlayCanvasObj.SetActive(true);
    }
    private void GameClear(ClearData clearData)
    {
        _gamePlayCanvasObj.SetActive(false);
        _gameClearCanvasObj.SetActive(true);
        _pocketPointText.text = "ポケットスコア : " + clearData.PocketPoint.ToString();
        _lifeBonusText.text = "残機ボーナス : 100 × " + clearData.Life.ToString();
        _totalScoreText.text = "合計スコア : " + clearData.TotalScore.ToString();
    }

    private void GameOver()
    {
        _gamePlayCanvasObj.SetActive(false);
        _gameOverCanvasObj.SetActive(true);
    }
    void Start()
    {
        //BilliardRuleを購読
        _billiardRuleObj.GameStartSub.Subscribe(_ =>
        {
            GameStart();
        }).AddTo(this);
        
        _billiardRuleObj.GameClearSub.Subscribe(clearData =>
        {
            GameClear(clearData);
        }).AddTo(this);

        _billiardRuleObj.GameOverSub.Subscribe(_=>
        {
            GameOver();
        }).AddTo(this);

        _prepareCanvasObj.SetActive(true);
        _gamePlayCanvasObj.SetActive(false);
        _gameClearCanvasObj.SetActive(false);
        _gameOverCanvasObj.SetActive(false);
    }
}
