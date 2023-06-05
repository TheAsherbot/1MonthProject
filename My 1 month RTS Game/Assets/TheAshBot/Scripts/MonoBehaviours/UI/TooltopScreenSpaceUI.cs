using System;
using System.Collections;
using System.Collections.Generic;

using TheAshBot;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class TooltopScreenSpaceUI : MonoBehaviour
{

    public static TooltopScreenSpaceUI Instance
    {
        get;
        private set;
    }



    private Func<string> getTooltipTextFunc;


    [SerializeField] private RectTransform canvasRectTransfrom;
    [SerializeField] private RectTransform backgroundRectTransfrom;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private RectTransform rectTransform;


    private void Awake()
    {
        if (Instance != null)
        {
            this.LogError("There is more than one instance of the TooltopScreenSpaceUI Class!!! this should NEVER happen!");
            Destroy(this);
            return;
        }

        Instance = this;

        rectTransform = GetComponent<RectTransform>();

        HideTooltip();
    }

    private void LateUpdate()
    {
        SetText(getTooltipTextFunc());

        Vector2 anchoredPosition = Mouse.current.position.ReadValue() / canvasRectTransfrom.localScale.x; // x, y, or z will work here becouse all of them will be the same.


        if (anchoredPosition.x + backgroundRectTransfrom.rect.width > canvasRectTransfrom.rect.width)
        {
            // Tooltip has left the screen on right side of the screen
            anchoredPosition.x = canvasRectTransfrom.rect.width - backgroundRectTransfrom.rect.width;
        }
        else if (anchoredPosition.x < 0)
        {
            // Tooltip has left the screen on left side of the screen
            anchoredPosition.x = 0;
        }
        if (anchoredPosition.y + backgroundRectTransfrom.rect.height > canvasRectTransfrom.rect.height)
        {
            // Tooltip has left the screen on top side of the screen
            anchoredPosition.y = canvasRectTransfrom.rect.height - backgroundRectTransfrom.rect.height;
        }
        else if (anchoredPosition.y < 0)
        {
            // Tooltip has left the screen on bottom side of the screen
            anchoredPosition.y = 0;
        }


        rectTransform.anchoredPosition = anchoredPosition;
    }


    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);

        backgroundRectTransfrom.sizeDelta = textSize + padding;
    }

    private void ShowTooltip(string tooltipText)
    {
        gameObject.SetActive(true);
        SetText(tooltipText);
    }
    private void ShowTooltip(out Action<string> OnTooltipChanged)
    {
        OnTooltipChanged = ShowTooltip;
    }
    private void ShowTooltip(Func<string> getTooltipTextFunc)
    {
        this.getTooltipTextFunc = getTooltipTextFunc;
        gameObject.SetActive(true);
        SetText(getTooltipTextFunc());
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }





    public static void ShowTooltip_Static(string tooltipText)
    {
        Instance.ShowTooltip(tooltipText);
    }
    public static void ShowTooltip_Static(out Action<string> OnTooltipChanged)
    {
        Instance.ShowTooltip(out OnTooltipChanged);
    }
    public static void ShowTooltip_Static(Func<string> getTooltipTextFunc)
    {
        Instance.ShowTooltip(getTooltipTextFunc);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }


}
