using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour {

  public TMP_Dropdown dropdown;
  public TextMeshProUGUI selectedValueText;
  private string selectedValue = "";

  void Start() {
    // dropdown.value = "";
    dropdown.options.Clear();

    List<string> items = new List<string>();

    items.Add("Poland");
    items.Add("Germany");
    items.Add("Israel");
    items.Add("China");

    foreach(var item in items) {
      dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
    }
    
    OnDropdownValueChanged();
    dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(); });
  }

  public string getSelectedValue() {
    return selectedValue;
  }

  public void OnDropdownValueChanged() {
    int selectedIndex = dropdown.value;
    selectedValue = dropdown.options[selectedIndex].text;
    Debug.Log(selectedValue);
  }


}
