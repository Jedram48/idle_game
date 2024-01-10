using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {
  [SerializeField] private Slider slider;
  // [SerializeField] private TextMeshProUGUI sliderText;
  private string accessTokenTag = "ACCESS_TOKEN";
  private string usernameTag = "Username";
  private string accessTokenExpiration = "AccessTokenExpiration";
  private string refreshTokenTag = "REFRESH_TOKEN";
  private string refreshTokenExpiration = "RefreshTokenExpiration";
  private string authorizeEndpoint = "http://3.79.166.123:8080/auth/verify/";
  void Start() {
    // slider.onValueChanged.AddListener((v) => {
    //   sliderText.text = v.ToString("0.00");
    // });
    StartCoroutine(AuthorizeUser());
  }

  private IEnumerator AuthorizeUser() {
    CheckTokensValidity();
    String accessToken = PlayerPrefs.GetString(accessTokenTag, string.Empty);
    String refreshToken = PlayerPrefs.GetString(refreshTokenTag, string.Empty);
    
    if(accessToken == string.Empty && refreshToken == string.Empty) { // both tokens are empty
      SceneManager.LoadScene("LoginScene");
      yield return null;
    }

    var request = new UnityWebRequest(authorizeEndpoint, "POST");
    if(accessToken == string.Empty) {
      request.SetRequestHeader("Authorization", "Bearer " + refreshToken);
    } else {
      request.SetRequestHeader("Authorization", "Bearer " + accessToken);
    }
    request.SetRequestHeader("Content-Type", "application/json");
    var handler = request.SendWebRequest();

    float startTime = 0.0f;
    while (!handler.isDone) {
      startTime += Time.deltaTime;
      slider.value = handler.progress; // handle slider progress

      if(startTime > 15.0f) { // if it's longer than 15 seconds
        Debug.Log("Niepomyślna autoryzacja...");
        SceneManager.LoadScene("LoginScene");
        break;
      }
      yield return null;
    }
    Debug.Log($"{request.result}");

    if(request.result == UnityWebRequest.Result.Success) {
      AuthorizeResponse response = JsonUtility.FromJson<AuthorizeResponse>(request.downloadHandler.text);
      Debug.Log($"{response.message}");
      Debug.Log($"{response.username}");
      Debug.Log($"{response.accessToken}");

      int responseCode = (int)request.responseCode;
      Debug.Log("Kod odpowiedzi HTTP: " + responseCode);
      slider.value = 1.0f;
      if(responseCode >= 200) { // authorize success
        if(accessToken == string.Empty) { // new accessToken based on refreshToken
          PlayerPrefs.SetString(accessTokenTag, response.accessToken);
          PlayerPrefs.SetString(accessTokenExpiration, DateTime.Now.ToString());
        }
        PlayerPrefs.SetString(usernameTag, response.username);
        // PlayerPrefs.Save(); // saves all changes
        SceneManager.LoadScene("MenuScene"); // zmiana sceny na scenę menu
      }
    } else {
      slider.value = 1.0f;
      Debug.LogError($"Błąd zapytania POST: {request.error}");
      AuthorizeResponse response = JsonUtility.FromJson<AuthorizeResponse>(request.downloadHandler.text);
      Debug.LogError($"Message from server: {response.error}");
      if(request.responseCode == 401 || request.responseCode == 403 || request.responseCode >= 500) {
        SceneManager.LoadScene("LoginScene");
      }
    }    
    yield return null;
  }

  private void CheckTokensValidity() {
    string accessTokenTimestamp = PlayerPrefs.GetString(accessTokenExpiration, string.Empty);
    string refreshTokenTimestamp = PlayerPrefs.GetString(refreshTokenExpiration, string.Empty);

    if(!string.IsNullOrEmpty(accessTokenTimestamp)) {
      DateTime accesstimestamp = DateTime.Parse(accessTokenTimestamp);
      TimeSpan accessTokenElapsedTime = DateTime.Now - accesstimestamp;

      TimeSpan accessTokenExpirationTime = new TimeSpan(1, 0, 0); // 1hour for accessToken

      if(accessTokenElapsedTime > accessTokenExpirationTime) { // accessToken is not valid
        PlayerPrefs.DeleteKey(accessTokenExpiration);
        PlayerPrefs.DeleteKey(accessTokenTag);
      }
    }

    if(!string.IsNullOrEmpty(refreshTokenTimestamp)) {
      DateTime refreshtimestamp = DateTime.Parse(refreshTokenTimestamp);
      TimeSpan refreshTokenElapsedTime = DateTime.Now - refreshtimestamp;
      TimeSpan refreshTokenExpirationTime = new TimeSpan(3, 0, 0, 0); // 3 days for refreshToken

      if(refreshTokenElapsedTime > refreshTokenExpirationTime) { // refreshToken is not valid
        PlayerPrefs.DeleteKey(refreshTokenExpiration);
        PlayerPrefs.DeleteKey(refreshTokenTag);
      }
    }
  }

}
