using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class ClickToChangeScene : MonoBehaviour, IPointerClickHandler {
  [SerializeField] private string scene = "";

  public void onSceneChange(string sceneName) {
    Debug.Log(sceneName);
    SceneManager.LoadScene(sceneName);
  }

  public void OnPointerClick(PointerEventData eventData) {
    SceneManager.LoadScene(scene);
  }
}
