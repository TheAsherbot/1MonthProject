using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

namespace TheAshBot
{
    public static class UtilsClass
    {
        /// <summary>
        /// This checks to see if the mouse is over UI with a script
        /// </summary>
        /// <typeparam name="T">This is the script that it checks for</typeparam>
        /// <returns>true if the mouse is over the UI with the script</returns>
        public static bool IsMouseOverUIWithIgnores<T>()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> raycastResultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
            for (int raycastNumber = 0; raycastNumber < raycastResultList.Count; raycastNumber++)
            {
                if (raycastResultList[raycastNumber].gameObject.GetComponent<T>() != null)
                {
                    raycastResultList.RemoveAt(raycastNumber);
                    return true;
                }
            }

            return false;
        }

    }
}
