//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    int levelIndex;
    int totalLevels;

    void OnEnable()
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 20, Screen.width - 40, Screen.height - 40));
        {
            if (GUILayout.Button("Load Scene")) {
                SceneManager.LoadScene((levelIndex + 1) % SceneManager.sceneCountInBuildSettings);
            }
            if (GUILayout.Button("Load Scene Async")) {
                SceneManager.LoadSceneAsync((levelIndex + 1) % SceneManager.sceneCountInBuildSettings);
            }
            if (GUILayout.Button("Load Scene Additive")) {
                SceneManager.LoadScene((levelIndex + 1) % SceneManager.sceneCountInBuildSettings, LoadSceneMode.Additive);
            }
        }
        GUILayout.EndArea();
    }
}
