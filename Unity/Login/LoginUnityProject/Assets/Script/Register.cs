using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class Register : MonoBehaviour {

  private const string PASSWORD_REGEX = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{5,20})";
  private const string EMAIL_REGEX = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
  [SerializeField] private string createEndpoint = "http://3.79.166.123:8080/auth/";
  // [SerializeField] private string createEndpoint = "http://localhost:8080/auth/";
  [SerializeField] private TextMeshProUGUI alertText;
  [SerializeField] private TextMeshProUGUI passwordError;
  [SerializeField] private TextMeshProUGUI emailError;
  [SerializeField] private TMP_InputField emailInputField;
  [SerializeField] private TMP_InputField passwordInputField;
  [SerializeField] private TMP_InputField usernameInputField;
  [SerializeField] private Button registerButton;
  private ClickToChangeScene clickToChangeScene;

  private Transition_Loader transition_Loader;

  [SerializeField] private TMP_Dropdown dropdown;
  [SerializeField] private TextMeshProUGUI dropdownValue;

  void Start() {
    dropdown.options.Clear();
    List<string> items = new List<string>();
    items.Add("Poland");
    items.Add("Germany");
    items.Add("Israel");
    items.Add("China");

    foreach(var item in items) {
      dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
    }
    dropdownValue.text = "Select your country";
    SetDropdownListHeight();
    dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(); });
  }

  private void OnDropdownValueChanged() {
    int selectedIndex = dropdown.value;
    dropdownValue.text = dropdown.options[selectedIndex].text;
    Debug.Log(dropdownValue.text);
  }

  public void OnCreateClick() {
    ActivateButtons(false);
    StartCoroutine(TryCreate());
  }

  public void OnLinkClick() {
    // clickToChangeScene.onSceneChange("LoginScene");
    transition_Loader.LoadNextScene();
  }

  private IEnumerator TryCreate() {
    transition_Loader = new Transition_Loader();
    string username = usernameInputField.text;
    string email = emailInputField.text;
    string password = passwordInputField.text;
    string country = dropdownValue.text; 

    if(username == "") {
      CleanInputs();
      ActivateButtons(true);
      yield break;
    }

    if(email.Length < 6 || email.Length > 24 || !Regex.IsMatch(email, EMAIL_REGEX)) {
      Debug.Log("Not valid email");
      emailError.text = "Invalid email format";
      CleanInputs();
      ActivateButtons(true);
      yield break;
    }

    if(!Regex.IsMatch(password, PASSWORD_REGEX)) {
      Debug.Log("Not valid password");
      passwordError.text = "At least 1 small and big letter and at least 1 number are required";
      CleanInputs();
      ActivateButtons(true);
      yield break;
    }

    RegisterData requestData = new RegisterData();
    requestData.username = username;
    requestData.email = email;
    requestData.password = password;
    requestData.country = country;

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
      if(startTime > 15.0f) { // if it's longer than 10 seconds
        Debug.Log("Niepomyślna rejestracja...");
        CleanInputs();
        ActivateButtons(true);
        break;
      }
      yield return null;
    }
    Debug.Log($"{request.result}");

    if(request.result == UnityWebRequest.Result.Success) {
      RegisterResponse response = JsonUtility.FromJson<RegisterResponse>(request.downloadHandler.text);
      int responseCode = (int)request.responseCode;
      Debug.Log("Kod odpowiedzi HTTP: " + responseCode);
      if(responseCode >= 200) { // register success
        ActivateButtons(false);
        CleanInputs();
        SceneManager.LoadScene("LoginScene"); 
      }
    } else {
      Debug.LogError($"Błąd zapytania POST: {request.error}");
      RegisterResponse response = JsonUtility.FromJson<RegisterResponse>(request.downloadHandler.text);
      Debug.LogError($"Message from server: {response.message}");
      if(request.responseCode == 401 || request.responseCode == 400) {
        alertText.text = "Invalid registration";
      }
      if(request.responseCode >= 500) {
        alertText.text = "Server error";
      }
      CleanInputs();
      ActivateButtons(true);
    }

    yield return null;
  }

  private void ActivateButtons(bool toggle) {
    registerButton.interactable = toggle;
  }

  private void SetDropdownListHeight() {
    float itemHeight = 60f;
    int visibleItems = Mathf.Min(dropdown.options.Count, 5);
    float dropdownHeight = visibleItems * itemHeight;
    RectTransform dropdownTransform = dropdown.template.GetComponent<RectTransform>();
    dropdownTransform.sizeDelta = new Vector2(dropdownTransform.sizeDelta.x, dropdownHeight);
  }

  private void CleanInputs() {
    usernameInputField.text = "";
    emailInputField.text = "";
    passwordInputField.text = "";    
  }
}
