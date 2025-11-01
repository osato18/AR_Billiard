using UnityEngine;
using UniRx;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private ControllerSubject _controllerSub;
    [SerializeField]private BallNumber[] _ballNumbers;
    [SerializeField] private BilliardRule _billiardRule;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _seSource;

    [Header("BGM")]
    [SerializeField] private AudioClip _playClip;
    [SerializeField] private AudioClip _gameOverClip;
    [SerializeField] private AudioClip _gameClearClip;

    [Header("SE")]
    [SerializeField] private AudioClip _shotClip;
    [SerializeField] private AudioClip _successClip;
    [SerializeField] private AudioClip _foulClip;
    void Start()
    {
        _bgmSource.loop = true;
        _bgmSource.clip = _playClip;
        _bgmSource.Play();

        _billiardRule.GameOverBgmSub.Subscribe(_ =>
            {
                _bgmSource.loop = false;
                PlayBGM(_gameOverClip);
            }).AddTo(this);

        _billiardRule.GameClearBgmSub.Subscribe(_ =>
        {
            _bgmSource.loop = false;
            PlayBGM(_gameClearClip);
        }).AddTo(this);

        _controllerSub.IsShotSESub.Subscribe(_ =>
        {
            PlaySE(_shotClip);
        }).AddTo(this);

        Observable.Merge(_ballNumbers.Select(ball => ball.IsColliderSESub))
        .Subscribe(_ =>
        {
            PlaySE(_shotClip);
        }).AddTo(this);

        _billiardRule.SuccessSESub.Subscribe(_ =>
        {
            PlaySE(_successClip);
        }).AddTo(this);
        
        _billiardRule.FoulSESub.Subscribe(_ =>
        {
            PlaySE(_foulClip);
        }).AddTo(this);
    }

    private void PlayBGM(AudioClip clip)
    {
        _bgmSource.Stop();
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void PlaySE(AudioClip clip)
    {
        _seSource.PlayOneShot(clip);
    }
}
