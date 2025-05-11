using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public abstract class Interactee : MonoBehaviour
{
    [SerializeField] private bool isDisplayingMessage;
    private GameObject text;
    protected TextMeshProUGUI tmpText;
    protected string message = " Interact ";
    [SerializeField] private Vector3 textPos = new Vector3(0, 1, 0);
    [SerializeField] private float textSize = 0.5f;
    private void Start()
    {
        SetMessage();
        text = CreateTextObject(transform, message);
    }
    public abstract void Interact(Transform player);
    protected abstract void SetMessage();

    public void DisplayMessage() { isDisplayingMessage = true; }

    private void LateUpdate()
    {
        text.SetActive(isDisplayingMessage);
        isDisplayingMessage = false;
    }

    public GameObject CreateTextObject(Transform transform, string message)
    {
        GameObject g = new GameObject("WorldCanvas");
        g.transform.SetParent(transform);
        Canvas canvas = g.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "UI";

        RectTransform rt = canvas.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(10, 1);
        rt.localPosition = textPos;


        GameObject g2 = new GameObject("TMP_Text");
        g2.transform.SetParent(g.transform);


        tmpText = g2.AddComponent<TextMeshProUGUI>();
        tmpText.text = message;
        tmpText.color = Color.white;
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.fontSize = textSize;
        tmpText.enableWordWrapping = false;
        tmpText.overflowMode = TextOverflowModes.Overflow;

        tmpText.fontMaterial.EnableKeyword("OUTLINE_ON");
        tmpText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.1f);
        tmpText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);



        RectTransform rt2 = g2.GetComponent<RectTransform>();
        rt2.anchorMin = Vector2.zero;
        rt2.anchorMax = Vector2.one;
        rt2.offsetMin = Vector2.zero;
        rt2.offsetMax = Vector2.zero;
        rt2.localScale = Vector3.one;
        rt2.localPosition = Vector3.zero;

        return g2;


    }
}
