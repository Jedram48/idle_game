using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScrollPanel : MonoBehaviour
{
    ScrollRect panel;
    [SerializeField] RectTransform statystic;
    [SerializeField] RectTransform shop;
    [SerializeField] TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        panel = GetComponent<ScrollRect>();
    }

    public void nextButton()
    {
        text.text = "Shop";
        statystic.gameObject.SetActive(false);
        shop.gameObject.SetActive(true);
        panel.content = shop;
    }

    public void prevButton()
    {
        text.text = "Statistics";
        statystic.gameObject.SetActive(true);
        shop.gameObject.SetActive(false);
        panel.content = statystic;
    }
}
