
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UI_Elements : MonoBehaviour
{
    public TextMeshProUGUI Accuracy;
    public TextMeshProUGUI Combo;

    public TextMeshProUGUI Note;

    public Image ColorNote;

    public RectTransform RotationNote;

    public Sprite[] OriginalLanes;
    public Sprite[] PointLanes;

    public Image[] ImagesLanes;

    public float DurationNote;
    
    private Coroutine fadeCoroutine;

    void OnEnable()
    {
        Referee.Madegood +=Update_good;
        Referee.Madeperfect +=Update_perfect;
        Referee.Mademistake +=Update_mistake;
    }

    void OnDisable()
    {
        Referee.Madegood -=Update_good;
        Referee.Madeperfect -=Update_perfect;
        Referee.Mademistake -=Update_mistake;
    }

    void Update_accuracy()
    {
        Accuracy.text="Accuracy: "+GameManager.Instance.Accuracy + "%";
    }

    void Update_combo()
    {
        Combo.text="Combo: "+GameManager.Instance.combo;
    }

    void Update_good(int button)
    {
        Update_accuracy();
        Update_combo();
        Note.text = "Good";
        StartCoroutine(Change_texture(DurationNote,button));
        SetNoteColor("good");
        
    }

    void Update_perfect(int button)
    {
        Update_accuracy();
        Update_combo();
        Note.text = "Perfect";
        StartCoroutine(Change_texture(DurationNote,button));
        SetNoteColor("perfect");
        
    }

    void Update_mistake(int button)
    {
        Update_accuracy();
        Update_combo();
        Note.text = "X";
        SetNoteColor("mistake");
    }

    void SetNoteColor(string noteType)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);


        Color noteColor = Color.white;
        switch (noteType.ToLower())
        {
            case "good":
                noteColor = Color.green;
                break;
            case "perfect":
                noteColor = new Color(1f, 0.84f, 0f, 1f); // Dorado
                break;
            case "mistake":
                noteColor = Color.red;
                break;
        }

        //Alpha=80/255
        noteColor.a = 0.31f;
        Note.alpha = 1f;
        ColorNote.color = noteColor;

        RotationNote.rotation = Quaternion.Euler(0, 0, Random.Range(-20f, 20f));//valores para cambiar la rotación del score


        // Iniciar desvanecimiento
        fadeCoroutine = StartCoroutine(FadeOutNote(1f));
    }

    IEnumerator FadeOutNote(float duration)
    {

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0.31f, 0f, elapsed / duration);
            float alpha2 = Mathf.Lerp(1f, 0f, elapsed / duration);
            
            Color currentColor = ColorNote.color;
            currentColor.a = alpha;
            Note.alpha = alpha2;
            ColorNote.color = currentColor;

            yield return null;
        }

        // Asegurar que llegue a alpha 0
        Color finalColor = ColorNote.color;
        finalColor.a = 0f;
        Note.alpha = 0f;
        ColorNote.color = finalColor;
    }

    IEnumerator Change_texture(float duration, int button)
    {
        //Debug.Log("Cambio de botón " + button);

        ImagesLanes[button].sprite = PointLanes[button];

        yield return new WaitForSeconds(duration);

        ImagesLanes[button].sprite = OriginalLanes[button];
    }
}
