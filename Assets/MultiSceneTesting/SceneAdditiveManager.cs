using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
public class SceneAdditiveManager : MonoBehaviour
{
    [SerializeField] private List<string> scenes;

    [ContextMenu("Load all scenes")]
    void Load()
    {
        loadScenes();
    }
    [ContextMenu("Unload all scenes")]
    void Unload()
    {
        unloadScenes();
    }

    void loadScenes()
    {
        for(int i = 0; i < scenes.Count; i++)
        {
            SceneManager.LoadScene(scenes[i], LoadSceneMode.Additive);
        }
    }

    void unloadScenes()
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            SceneManager.UnloadSceneAsync(scenes[i]);
        }
    }
}
