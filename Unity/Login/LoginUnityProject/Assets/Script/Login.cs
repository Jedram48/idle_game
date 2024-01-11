using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class Login : MonoBehaviour {
  private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{5,20})";
  private const string EMAIL_REGEX = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
  private string loginEndpoint = "http://3.79.166.123:8080/auth/login/";
  // private string loginEndpoint = "http://localhost:8080/auth/login/";
  [SerializeField] private TextMeshProUGUI loginErrorText;
  [SerializeField] private TextMeshProUGUI alertText;
  [SerializeField] private Button loginButton;
  [SerializeField] private TMP_InputField emailInputField;
  [SerializeField] private TMP_InputField passwordInputField;
  public ClickToChangeScene clickToChangeScene;
  private string accessTokenTag = "ACCESS_TOKEN";
  private string accessTokenExpiration = "AccessTokenExpiration";
  private string refreshTokenTag = "REFRESH_TOKEN";
  private string refreshTokenExpiration = "RefreshTokenExpiration";
  private string usernameTag = "Username";
  private string userIdTag = "User_Id";

  public void OnLoginClick() {
    ActivateButtons(false);
    StartCoroutine(TryLogin());
  }

  public void OnLinkClick() {
    clickToChangeScene.onSceneChange("RegisterScene");
  }

  private IEnumerator TryLogin() {
    string email = emailInputField.text;
    string password = passwordInputField.text;

    if(email.Length < 6 || email.Length > 24 || !Regex.IsMatch(email, EMAIL_REGEX)) {
      SetErrorText();
      CleanInputs();
      ActivateButtons(true);
      yield break;
    }

    if(!Regex.IsMatch(password, PASSWORD_REGEX)) {
      SetErrorText();
      CleanInputs();
      ActivateButtons(true);
      yield break;
    }
    
    LoginData requestData = new LoginData();
    requestData.email = email;
    requestData.password = password;

    string jsonData = JsonUtility.ToJson(requestData);
    var request = new UnityWebRequest(loginEndpoint, "POST");
    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    var handler = request.SendWebRequest();

    float startTime = 0.0f;
    while (!handler.isDone) {
      startTime += Time.deltaTime;
      if(startTime > 15.0f) { // if it's longer than 15 seconds
        Debug.Log("Niepomyślne logowanie...");
        CleanInputs();
        ActivateButtons(true);
        break;
      }
      yield return null;
    }
    Debug.Log($"{request.result}");    

    if(request.result == UnityWebRequest.Result.Success) {
      LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
      SetPrefs(response.accessToken, response.refreshToken, response.data.username, response.data.userId);
      
      int responseCode = (int)request.responseCode;
      Debug.Log("Kod odpowiedzi HTTP: " + responseCode);
      if(responseCode >= 200) { // login success
        ActivateButtons(false);
        CleanInputs();
        SceneManager.LoadScene("MenuScene"); // zmiana sceny na scenę menu
      }
    } else {
      Debug.LogError($"Błąd zapytania POST: {request.error}");
      LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
      Debug.LogError($"Message from server: {response.error}");
      if(request.responseCode == 401 || request.responseCode == 400) {
        SetErrorText();
      }
      if(request.responseCode >= 500) {
        SetErrorText();
      }
      ActivateButtons(true);
      CleanInputs();
    }
    yield return null;
  }

  private void ActivateButtons(bool toggle) {
    loginButton.interactable = toggle;
  }

  private void CleanInputs() {
    emailInputField.text = "";
    passwordInputField.text = "";
  }

  private void SetErrorText() {
    loginErrorText.text = "Invalid login data";
  }

  private void SetPrefs(string accessToken, string refreshToken, string username, string userId) {
    PlayerPrefs.SetString(accessTokenTag, accessToken);
    PlayerPrefs.SetString(accessTokenExpiration, System.DateTime.Now.ToString());
    PlayerPrefs.SetString(refreshTokenTag, refreshToken);
    PlayerPrefs.SetString(refreshTokenExpiration, System.DateTime.Now.ToString());
    PlayerPrefs.SetString(usernameTag, username);
    PlayerPrefs.SetString(userIdTag, userId);
  }
}
