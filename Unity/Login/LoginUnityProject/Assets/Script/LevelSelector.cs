using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] string next;
    [SerializeField] string prev;

    public void nextLevel()
    {
        SceneManager.LoadScene(next);
    }

    public void prevLevel()
    {
        SceneManager.LoadScene(prev);
    }
}
