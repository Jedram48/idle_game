using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_Loader : MonoBehaviour
{
  public Animator transition;
  void Update()
  {

  }

  public void LoadNextScene() {
    // StartCoroutine(LoadLoginScene("LoginScene"));
    // LoadLoginScene(SceneManager.GetActiveScene().buildIndex + 1);
    StartCoroutine(LoadLoginScene(SceneManager.GetActiveScene().buildIndex + 1));
  }

  private IEnumerator LoadLoginScene(int levelIndex) {
    transition.SetTrigger("Start");
    yield return new WaitForSeconds(1f);
    SceneManager.LoadScene(levelIndex);
  }
}
