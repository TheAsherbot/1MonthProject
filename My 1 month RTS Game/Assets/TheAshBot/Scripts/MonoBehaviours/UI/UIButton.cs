using System;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ShowOdinSerializedPropertiesInInspector]
public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{


    public enum ColorVisualization
    {
        None,
        SetColor,
        AddToColor,
    }
    public enum RenderType
    {
        None,
        Image,
        SpriteRenderer,
    }


    #region Variables
    // These are public becouse they I want to beable to get/set them in other scripts, and show them in the inspector

#if ODIN_INSPECTOR
    #region ODIN_INSPECTOR
    
    public event Action OnMouseEnterUI;
    public event Action OnMouseExitUI;
    public event Action OnMouseStartClickUI;
    public event Action OnMouseEndClickUI;


    [Header("Renderer")]
    [EnumToggleButtons()]
    public RenderType renderType;
    [ShowIf("@renderType == RenderType.Image")]
    public Image buttonImage;
    [ShowIf("@renderType == RenderType.SpriteRenderer")]
    public SpriteRenderer buttonSpriteRenderer;


    [Header("ColorVisualization")]
    [EnumToggleButtons()]
    public ColorVisualization colorVisualization = ColorVisualization.AddToColor;
    [HideIf("@colorVisualization == ColorVisualization.None")]
    public Color defualtColor = Color.white;
    [HideIf("@colorVisualization == ColorVisualization.None")]
    public Color mouseOverUIColor = Color.white;
    [HideIf("@colorVisualization == ColorVisualization.None")]
    public Color holdingMouseDownOverUIColor = Color.white;

    #endregion
#else
    #region NOT ODIN_INSPECTOR
    public event Action OnMouseEnterUI;
    public event Action OnMouseExitUI;
    public event Action OnMouseStartClickUI;
    public event Action OnMouseEndClickUI;


    [Header("Renderer")]
    public RenderType renderType;
    public Image buttonImage;
    public SpriteRenderer buttonSpriteRenderer;


    [Header("ColorVisualization")]
    public ColorVisualization colorVisualization;
    public Color defualtColor = Color.black;
    public Color mouseOverUIColor = Color.black;
    public Color holdingMouseDownOverUIColor = Color.black;
    #endregion
#endif


    #endregion


    #region Unity Functions

    private void Awake()
    {
        if (buttonImage == null)
        {
            TryGetComponent(out buttonImage);
        }
        if (buttonSpriteRenderer == null)
        {
            TryGetComponent(out buttonSpriteRenderer);
        }

        SetColorTo_DefualtColor();
    }

    #endregion


    #region Interfaces

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterUI?.Invoke();

        SetColorTo_MouseOverUIColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitUI?.Invoke();

        SetColorTo_DefualtColor();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseStartClickUI?.Invoke();

        SetColorTo_HoldingMouseDownOverUIColor();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnMouseEndClickUI?.Invoke();

        SetColorTo_MouseOverUIColor();
    }

    #endregion


    #region Public Helper Functions

    public void SetSprite(Sprite sprite)
    {
        if (renderType == RenderType.Image)
        {
            buttonImage.sprite = sprite;
        }
        else if (renderType == RenderType.SpriteRenderer)
        {
            buttonSpriteRenderer.sprite = sprite;
        }
    }

    public void SetAllEventsToNull()
    {
        OnMouseEnterUI = null;
        OnMouseEnterUI = null;
        OnMouseStartClickUI = null;
        OnMouseEndClickUI = null;
    }

    #endregion


    #region SetColor

    private void SetColorTo_DefualtColor()
    {
        if (renderType == RenderType.None || colorVisualization == ColorVisualization.None) return;

        Base_SetColor(defualtColor);
    }

    private void SetColorTo_MouseOverUIColor()
    {
        if (renderType == RenderType.None || colorVisualization == ColorVisualization.None) return;

        Color color = Base_AddSetColor(mouseOverUIColor);

        Base_SetColor(color);
    }

    private void SetColorTo_HoldingMouseDownOverUIColor()
    {
        if (renderType == RenderType.None || colorVisualization == ColorVisualization.None) return;

        Color color = Base_AddSetColor(holdingMouseDownOverUIColor);

        Base_SetColor(color);
    }


    private Color Base_AddSetColor(Color color)
    {
        switch (colorVisualization)
        {
            case ColorVisualization.SetColor:
                return color;
            case ColorVisualization.AddToColor:
                Color invertedColor = new Color(color.r == 0 ? 0 : 1, color.g == 0 ? 0 : 1, color.b == 0 ? 0 : 1) - color;

                return defualtColor - invertedColor;
        }

        return defualtColor;

    }

    private void Base_SetColor(Color color)
    {
        switch (renderType)
        {
            case RenderType.Image:
                buttonImage.color = color;
                break;
            case RenderType.SpriteRenderer:
                buttonSpriteRenderer.color = color;
                break;
        }
    }

    #endregion

}
