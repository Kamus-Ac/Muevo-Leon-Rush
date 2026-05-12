using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        FinalGame1,
        FinalGame
    }

    private static Scene _targetScene;

    public static void Load(Scene targetScene)
    {
        Loader._targetScene = targetScene;
        SceneManager.LoadScene(targetScene.ToString());
    }
}
