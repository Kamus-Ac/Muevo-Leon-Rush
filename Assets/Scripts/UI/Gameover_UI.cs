using TMPro;
using UnityEngine;
using System.Collections;

public class Gameover_UI : MonoBehaviour
{
    public TextMeshProUGUI Retry;

    public TextMeshProUGUI Score;
    [SerializeField] private GameObject _hud;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }


    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
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
        _hud.SetActive(false); //hide hud
        gameObject.SetActive(true); //show
        // Cambiar el score
        Score.text = GameManager.Instance.score.ToString();

        // Iniciar parpadeo
        StartCoroutine(BlinkRetry());
    }

    private void Hide()
    {
        gameObject.SetActive(false); //hide
    }

IEnumerator BlinkRetry()
    {
        while (true)
        {
            Retry.enabled = !Retry.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
