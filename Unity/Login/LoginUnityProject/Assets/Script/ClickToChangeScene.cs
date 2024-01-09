using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
