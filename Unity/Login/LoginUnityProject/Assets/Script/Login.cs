using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class Login : MonoBehaviour {
  private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,24})";
  private const string EMAIL_REGEX = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}";
  [SerializeField] private string loginEndpoint = "http://127.0.0.1:8080/auth/login";

  [SerializeField] private TextMeshProUGUI alertText;
  [SerializeField] private Button loginButton;
  [SerializeField] private TMP_InputField emailInputField;
  [SerializeField] private TMP_InputField passwordInputField;
  public ClickToChangeScene clickToChangeScene;

  public void OnLoginClick() {
    string email = emailInputField.text;
    string password = passwordInputField.text;
    Debug.Log($"{email}:{password}");
    alertText.text = "Signing in...";
    ActivateButtons(false);
    StartCoroutine(TryLogin());
  }

  public void OnLinkClick() {
    clickToChangeScene.onSceneChange("RegisterScene");
  }

  private IEnumerator TryLogin() {
    string email = emailInputField.text;
    string password = passwordInputField.text;

    if(email.Length < 3 || email.Length > 24) {
      alertText.text = "Invalid email";
      ActivateButtons(true);
      yield break;
    }

    // if(!Regex.IsMatch(password, PASSWORD_REGEX) || !Regex.IsMatch(email, EMAIL_REGEX)) {
    //   alertText.text = "Invalid credentials";
    //   ActivateButtons(true);
    //   yield break;
    // }
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
      if(startTime > 10.0f) { // if it's longer than 10 seconds
        Debug.Log("Niepomyślne logowanie...");
        break;
      }
      yield return null;
    }
    Debug.Log($"{request.result}");
    

    if(request.result == UnityWebRequest.Result.Success) {
      Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
      Debug.Log($"{response}");
      int responseCode = (int)request.responseCode;
      Debug.Log("Kod odpowiedzi HTTP: " + responseCode);
      if(responseCode >= 200) { // login success
        ActivateButtons(false);
        alertText.text = "Welcome";
        SceneManager.LoadScene("MenuScene"); // zmiana sceny na scenę logowania
      }
    } else {
      Debug.LogError($"Błąd zapytania POST: {request.error}");
      Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
      Debug.LogError($"Message from server: {response.message}");
      if(request.responseCode == 401 || request.responseCode == 400) {
        alertText.text = "Invalid credentials";
      }
      if(request.responseCode >= 500) {
        alertText.text = "Server error";
      }
      ActivateButtons(true);
    }
    yield return null;
  }
  private void ActivateButtons(bool toggle) {
    loginButton.interactable = toggle;
  }
}
