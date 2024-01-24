using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

  [SerializeField] private Button logoutButton; 
  [SerializeField] private Button playButton;
  [SerializeField] private TextMeshProUGUI menuErrorText;
  private string logoutEndpoint = "http://3.79.166.123:8080/auth/logout/";
  // private string logoutEndpoint = "http://localhost:8080/auth/logout/";
  private Transition_Loader transition_Loader;
  private TokenOrganizer tokenOrganizer;
  void Start() {
    tokenOrganizer = new TokenOrganizer();
  }

  public void OnLogoutClick() {
    ToggleButton(false);
    StartCoroutine(LogoutUser());
  }

  public void OnPlayButtonClick() {
    // transition_Loader.LoadNextScene();
    SceneManager.LoadScene("Level 3");
  }

  private IEnumerator LogoutUser() {
    tokenOrganizer.CheckTokens();
    String accessToken = PlayerPrefs.GetString(tokenOrganizer.GetAccessTokenTag(), string.Empty);
    String refreshToken = PlayerPrefs.GetString(tokenOrganizer.GetRefreshTokenTag(), string.Empty);

    if(accessToken == string.Empty && refreshToken == string.Empty) { // both tokens are empty
      menuErrorText.text = "Invalid logout request";
      ToggleButton(true);
      yield return null;
    }
    
    var request = new UnityWebRequest(logoutEndpoint, "POST");
    if(accessToken == string.Empty) {
      request.SetRequestHeader("Authorization", "Bearer " + refreshToken);
    } else {
      request.SetRequestHeader("Authorization", "Bearer " + accessToken);
    }

    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    var handler = request.SendWebRequest();

    float startTime = 0.0f;
    while(!handler.isDone) {
      startTime += Time.deltaTime;
      if(startTime > 15.0f) { // if it's longer than 15 seconds
        Debug.Log("Niepomyślne wylogowanie...");
        menuErrorText.text = "Invalid logout request";
        ToggleButton(true);
        break;
      }
      yield return null;
    }

    if(request.result == UnityWebRequest.Result.Success) {
      LogoutResponse response = JsonUtility.FromJson<LogoutResponse>(request.downloadHandler.text);
      int responseCode = (int)request.responseCode;
      Debug.Log("Kod odpowiedzi HTTP: " + responseCode);
      if(responseCode >= 200) { // logout success
        tokenOrganizer.EraseTokens(); // erase tokens
        SceneManager.LoadScene("LoginScene");
      }
    } else {
      Debug.LogError($"Błąd zapytania POST: {request.error}");
      LogoutResponse response = JsonUtility.FromJson<LogoutResponse>(request.downloadHandler.text);
      Debug.LogError($"Message from server: {response.error}");
      if(request.responseCode == 400 || request.responseCode >= 500) {
        menuErrorText.text = "Error " + request.responseCode.ToString() + " trying to logout...";
      }
      ToggleButton(true);
    }
    yield return null;
  }

  private void ToggleButton(bool toggle) {
    logoutButton.interactable = toggle;
  }

}
