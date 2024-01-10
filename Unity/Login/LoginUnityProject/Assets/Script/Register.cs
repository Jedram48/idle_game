using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class Register : MonoBehaviour {

  private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{5,20})";
  private const string EMAIL_REGEX = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}";
  [SerializeField] private string createEndpoint = "http://127.0.0.1:8080/auth/";
  [SerializeField] private TextMeshProUGUI alertText;
  [SerializeField] private TMP_InputField emailInputField;
  [SerializeField] private TMP_InputField passwordInputField;
  [SerializeField] private TMP_InputField nameInputField;
  [SerializeField] private Button registerButton;
  [SerializeField] private TMP_Dropdown countryDropdown;
  public ClickToChangeScene clickToChangeScene;

  public void OnCreateClick() {
    alertText.text = "Creating account...";
    ActivateButtons(false);
    StartCoroutine(TryCreate());
  }

  public void OnLinkClick() {
    clickToChangeScene.onSceneChange("LoginScene");
  }

  private IEnumerator TryCreate() {
    string name = nameInputField.text;
    string email = emailInputField.text;
    string password = passwordInputField.text;
    // dropdown input

    if(name == "") {
      alertText.text = "Invalid name";
      ActivateButtons(true);
      yield break;
    }

    if(email.Length < 3 || email.Length > 24) {
      alertText.text = "Invalid email";
      ActivateButtons(true);
      yield break;
    }

    if(!Regex.IsMatch(password, PASSWORD_REGEX) || !Regex.IsMatch(email, EMAIL_REGEX)) {
      Debug.Log("Regexy niepoprawne :(");
      alertText.text = "Invalid credentials";
      ActivateButtons(true);
      yield break;
    }

    RegisterData requestData = new RegisterData();
    requestData.name = name;
    requestData.email = email;
    requestData.password = password;
    requestData.country = "USA";

    string jsonData = JsonUtility.ToJson(requestData);
    var request = new UnityWebRequest(createEndpoint, "POST");
    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
    request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    var handler = request.SendWebRequest();

    float startTime = 0.0f;
    while (!handler.isDone) {
      startTime += Time.deltaTime;
      if(startTime > 10.0f) { // if it's longer than 10 seconds
        Debug.Log("Niepomyślna rejestracja...");
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
      if(responseCode >= 200) { // register success
        ActivateButtons(false);
        alertText.text = "Welcome";
        SceneManager.LoadScene("LoginScene"); // zmiana sceny na scenę logowania
      }
    } else {
      Debug.LogError($"Błąd zapytania POST: {request.error}");
      Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
      Debug.LogError($"Message from server: {response.message}");
      if(request.responseCode == 401 || request.responseCode == 400) {
        alertText.text = "Invalid registration";
      }
      if(request.responseCode >= 500) {
        alertText.text = "Server error";
      }
      ActivateButtons(true);
    }

    yield return null;
  }

  private void ActivateButtons(bool toggle) {
    registerButton.interactable = toggle;
  }
}
