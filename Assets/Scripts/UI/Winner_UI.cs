using UnityEngine;
using System.Collections;
using TMPro;

public class Winner_UI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public TextMeshProUGUI Score;
    public TextMeshProUGUI ScoreSombra;

    public TextMeshProUGUI Perfect;
    public TextMeshProUGUI Good;
    public TextMeshProUGUI Mistakes;

    public TextMeshProUGUI Retry;

    void OnEnable()
    {
        // Sobrescribir con datos del GameManager
        Score.text = "Score: " + GameManager.Instance.score;
        ScoreSombra.text = "Score: " + GameManager.Instance.score;

        Perfect.text = "Perfect: " + GameManager.Instance.perfect;
        Good.text = "Good: " + GameManager.Instance.good;
        Mistakes.text = "Mistakes: " + GameManager.Instance.mistakes;

        // Ocultar todo al inicio
        Score.gameObject.SetActive(false);
        ScoreSombra.gameObject.SetActive(false);
        Perfect.gameObject.SetActive(false);
        Good.gameObject.SetActive(false);
        Mistakes.gameObject.SetActive(false);
        Retry.gameObject.SetActive(false);

        StartCoroutine(RevealSequence());
    }

    IEnumerator RevealSequence()
    {
        yield return new WaitForSeconds(0.5f);

        Score.gameObject.SetActive(true);
        ScoreSombra.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.7f);
        Perfect.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.7f);
        Good.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.7f);
        Mistakes.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        Retry.gameObject.SetActive(true);

        StartCoroutine(BlinkRetry());
    }

    IEnumerator BlinkRetry()
    {
        while (true)
        {
            Retry.enabled = !Retry.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
        Retry.enabled = true;
    }
}
