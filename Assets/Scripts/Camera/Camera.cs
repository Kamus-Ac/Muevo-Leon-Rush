using UnityEngine;
using Unity.Cinemachine;

public class WinnerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera winnerCam;


    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (winnerCam != null && GameManager.Instance.IsWinner())
        {
            winnerCam.Priority.Value = 20;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
        }
    }
}
