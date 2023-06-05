using System.Collections;
using System.Collections.Generic;

using TheAshBot;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler
{


    #region Variables

    [Header("If these are null then it will atomaticly try to find them")]
    [SerializeField] private RectTransform dragRectTransfrom;
    [SerializeField] private Canvas canvas;


    private Vector2 mouseOffset;

    #endregion


    #region Unity Functions

    private void Awake()
    {
        if (dragRectTransfrom == null)
        {
            dragRectTransfrom = transform.parent.GetComponent<RectTransform>();
        }

        if (canvas == null)
        {
            Transform textCanvusTransfrom = transform.parent;
            while (textCanvusTransfrom != null)
            {
                if (textCanvusTransfrom.TryGetComponent(out canvas))
                {

                    break;
                }

                textCanvusTransfrom = textCanvusTransfrom.parent;
            }
        }
    }

    #endregion


    #region Interfaces

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform canvasRectTransfrom = canvas.GetComponent<RectTransform>();

        Vector2 anchoredMouse = eventData.position / canvas.scaleFactor;
        
        Vector2 anchoredPosition = anchoredMouse;
        float padding = 64;

        if (anchoredPosition.x + padding > canvasRectTransfrom.rect.width)
        {
            // Tooltip has left the screen on right side of the screen
            anchoredPosition.x = canvasRectTransfrom.rect.width - padding;
        }
        else if (anchoredPosition.x - padding < 0)
        {
            // Tooltip has left the screen on left side of the screen
            anchoredPosition.x = padding;
        }
        if (anchoredPosition.y + padding > canvasRectTransfrom.rect.height)
        {
            // Tooltip has left the screen on top side of the screen
            anchoredPosition.y = canvasRectTransfrom.rect.height - padding;
        }
        else if (anchoredPosition.y - padding < 0)
        {
            // Tooltip has left the screen on bottom side of the screen
            anchoredPosition.y = padding;
        }

        dragRectTransfrom.anchoredPosition = anchoredPosition - mouseOffset;
        
        //dragRectTransfrom.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTransfrom.SetAsLastSibling();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 anchoredMouse = eventData.position / canvas.scaleFactor;

        mouseOffset = anchoredMouse - dragRectTransfrom.anchoredPosition;
    }

    #endregion


}
