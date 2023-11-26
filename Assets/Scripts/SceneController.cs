using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string _nextScene;

    public void LoadNextScene()
    {
        SceneManager.LoadScene(_nextScene);
    }
}
