using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {
  [SerializeField] private Slider slider;
  private string accessTokenTag = "ACCESS_TOKEN";
  private string accessTokenExpiration = "AccessTokenExpiration";
  private string refreshTokenTag = "REFRESH_TOKEN";
  // private string authorizeEndpoint = "http://3.79.166.123:8080/auth/verify/";
  private string authorizeEndpoint = "http://3.79.105.64:8080/auth/verify/";
  private TokenOrganizer tokenOrganizer;

  void Start() {
    // PlayerPrefs.DeleteAll();
    tokenOrganizer = new TokenOrganizer();
    StartCoroutine(AuthorizeUser());
  }

  private IEnumerator AuthorizeUser() {
    tokenOrganizer.CheckTokens();
    String accessToken = PlayerPrefs.GetString(accessTokenTag, string.Empty);
    String refreshToken = PlayerPrefs.GetString(refreshTokenTag, string.Empty);
    
    if(accessToken == string.Empty && refreshToken == string.Empty) { // both tokens are empty
      float AnimDuration = 3.0f;
      float StartTime = 0.0f;

      while(StartTime < AnimDuration) {
        StartTime += Time.deltaTime;
        float progress = Mathf.Lerp(0.0f, 1.0f, StartTime / AnimDuration);
        slider.value = progress;

        yield return null; // Poczekanie na kolejną klatkę
      }
      slider.value = 1.0f;
      SceneManager.LoadScene("LoginScene");
    }

    var request = new UnityWebRequest(authorizeEndpoint, "POST");
    if(accessToken == string.Empty) {
      request.SetRequestHeader("Authorization", "Bearer " + refreshToken);
    } else {
      request.SetRequestHeader("Authorization", "Bearer " + accessToken);
    }

    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    var handler = request.SendWebRequest();

    float startTime = 0.0f;
    float animationDuration = 3.0f;
    while(!handler.isDone || startTime < animationDuration) {
      startTime += Time.deltaTime;
      float progress = Mathf.Lerp(0.0f, 1.0f, startTime / animationDuration);
      slider.value = progress;

      if(startTime > 15.0f) { // if it's longer than 15 seconds
        Debug.Log("Niepomyślna autoryzacja...");
        SceneManager.LoadScene("LoginScene");
        break;
      }
      yield return null;
    }

    if(request.result == UnityWebRequest.Result.Success) {
      AuthorizeResponse response = JsonUtility.FromJson<AuthorizeResponse>(request.downloadHandler.text);
      Debug.Log($"{response.message}");

      int responseCode = (int)request.responseCode;
      Debug.Log("Kod odpowiedzi HTTP: " + responseCode);
      slider.value = 1.0f;
      if(responseCode >= 200) { // authorize success
        if(accessToken == string.Empty) { // new accessToken based on refreshToken
          PlayerPrefs.SetString(accessTokenTag, response.accessToken);
          PlayerPrefs.SetString(accessTokenExpiration, DateTime.Now.ToString());
        }
        // PlayerPrefs.SetString(usernameTag, response.username);
        SceneManager.LoadScene("MenuScene"); // zmiana sceny na scenę menu
      }
    } else {
      slider.value = 1.0f;
      Debug.LogError($"Błąd zapytania POST: {request.error}");
      AuthorizeResponse response = JsonUtility.FromJson<AuthorizeResponse>(request.downloadHandler.text);
      Debug.Log(response.error);
      Debug.LogError($"Message from server: {response.error}");
      if(request.responseCode == 401 || request.responseCode == 403 || request.responseCode >= 500) {
        SceneManager.LoadScene("LoginScene");
      }
    }    
    yield return null;
  }
}
