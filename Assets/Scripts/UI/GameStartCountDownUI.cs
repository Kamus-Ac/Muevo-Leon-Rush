using UnityEditor.Rendering;
using UnityEngine;
using TMPro;
using System.Collections;

public class GameStartCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countDownText;
    private float time;
    private void Start()
    {
        
        Hide();
    }

    private void Awake()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    }

    /* private void OnDisable()
    {
        GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    } */

    private void Update()
    {
        _countDownText.text = Mathf.Ceil(GameManager.Instance.GetCountDownTimer()).ToString();
        FadeCountTime();
    }


    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountDownActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }


    private void Show()
    {
        gameObject.SetActive(true); //show
    }

    private void Hide()
    {
        gameObject.SetActive(false); //hide
    }

    private void FadeCountTime()
    {
        time = Mathf.Abs(GameManager.Instance.GetCountDownTimer()- Mathf.Ceil(GameManager.Instance.GetCountDownTimer()));
        _countDownText.alpha = Mathf.Lerp(1.0f,0.2f,time);
    }

}
