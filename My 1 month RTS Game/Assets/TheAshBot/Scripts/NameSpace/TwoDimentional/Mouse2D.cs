using UnityEngine;
using UnityEngine.UIElements;

namespace TheAshBot.TwoDimentional
{
    public class Mouse2D
    {

        #region Mouse Position Vector2

        #region GetObjectAtMousePosition
        
        public static bool TryGetObjectAtMousePosition(Camera camera, out GameObject hit)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D raycastHit = Physics2D.Raycast(ray.origin, ray.direction);

            if (raycastHit.transform != null)
            {
                hit = raycastHit.transform.gameObject;
                return true;
            }
            hit = null;
            return false;
        }
        
        public static bool TryGetObjectAtMousePosition(out GameObject hit)
        {
            return TryGetObjectAtMousePosition(Camera.main, out hit);
        }

        #endregion

        #region GetMousePosition
        /// <summary>
        /// This gets the mouse position in 2D
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <param name="zPosition">This is the z position</param>
        /// <returns>This return the mouse posintion</returns>
        public static Vector2 GetMousePosition2D(Camera camera, float zPosition)
        {
            Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = zPosition;
            return mouseWorldPosition;
        }

        /// <summary>
        /// This gets the mouse position in 2D
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <returns>This return the mouse posintion</returns>
        public static Vector2 GetMousePosition2D(Camera camera)
        {
            return GetMousePosition2D(camera, 0);
        }

        /// <summary>
        /// This gets the mouse position in 2D
        /// </summary>
        /// <param name="zPosition">This is the z position</param>
        /// <returns>This return the mouse posintion</returns>
        public static Vector2 GetMousePosition2D(float zPosition)
        {
            return GetMousePosition2D(Camera.main, zPosition);
        }

        /// <summary>
        /// This gets the mouse position in 2D
        /// </summary>
        /// <returns>This return the mouse posintion</returns>
        public static Vector2 GetMousePosition2D()
        {
            return GetMousePosition2D(Camera.main, 0);
        }
        #endregion

        #region FallowMousePosition
        /// <summary>
        /// This makes an object faloow the mouse position in 2D
        /// </summary>
        /// <param name="obj">This is the object that fallows the mouse</param>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <param name="zPosition">This is the z position</param>
        public static void FallowMousePosition2D(GameObject obj, Camera camera, float zPosition)
        {
            obj.transform.position = GetMousePosition2D(camera, zPosition);
        }

        /// <summary>
        /// This makes an object faloow the mouse position in 2D
        /// </summary>
        /// <param name="obj">This is the object that fallows the mouse</param>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        public static void FallowMousePosition2D(GameObject obj, Camera camera)
        {
            obj.transform.position = GetMousePosition2D(camera);
        }

        /// <summary>
        /// This makes an object faloow the mouse position in 2D
        /// </summary>
        /// <param name="obj">This is the object that fallows the mouse</param>
        /// <param name="zPosition">This is the z position</param>
        public static void FallowMousePosition2D(GameObject obj, float zPosition)
        {
            obj.transform.position = GetMousePosition2D(zPosition);
        }

        /// <summary>
        /// This makes an object faloow the mouse position in 2D
        /// </summary>
        /// <param name="obj">This is the object that fallows the mouse</param>
        public static void FallowMousePosition2D(GameObject obj)
        {
            obj.transform.position = GetMousePosition2D();
        }
        #endregion

        #region DebugLogMousePositionFloat
        /// <summary>
        /// This logs the mouse position in 2D
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <param name="zPosition">This is the z position</param>
        public static void DebugLogMousePositionFloat2D(Camera camera, float zPosition)
        {
            Vector3 mousePos = GetMousePosition2D(camera, zPosition);
            Debug.Log("Mouse Position Float 2D = " + mousePos);
        }

        /// <summary>
        /// This logs the mouse position in 2D
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        public static void DebugLogMousePositionFloat2D(Camera camera)
        {
            Vector3 mousePos = GetMousePosition2D(camera);
            Debug.Log("Mouse Position Float 2D = " + mousePos);
        }

        /// <summary>
        /// This logs the mouse position in 2D
        /// </summary>
        /// <param name="zPosition">This is the z position</param>
        public static void DebugLogMousePositionFloat2D(float zPosition)
        {
            Vector3 mousePos = GetMousePosition2D(zPosition);
            Debug.Log("Mouse Position Float 2D = " + mousePos);
        }

        /// <summary>
        /// This logs the mouse position in 2D
        /// </summary>
        public static void DebugLogMousePositionFloat2D()
        {
            Vector3 mousePos = GetMousePosition2D();
            Debug.Log("Mouse Position Float 2D = " + mousePos);
        }
        #endregion

        #endregion

        #region Mouse Position Vector2Int

        #region GetMousePositionInt
        /// <summary>
        /// This gets the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <param name="zPosition">This is the z position</param>
        /// <returns>This return the mouse posintion rounded to an Int</returns>
        public static Vector2Int GetMousePositionInt2D(Camera camera, int zPosition)
        {
            return Vector2Int.RoundToInt(GetMousePosition2D(camera, zPosition));
        }

        /// <summary>
        /// This gets the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <returns>This return the mouse posintion rounded to an Int</returns>
        public static Vector2Int GetMousePositionInt2D(Camera camera)
        {
            return GetMousePositionInt2D(camera, 0);
        }

        /// <summary>
        /// This gets the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="zPosition">This is the z position</param>
        /// <returns>This return the mouse posintion rounded to an Int</returns>
        public static Vector2Int GetMousePositionInt2D(int zPosition)
        {
            return GetMousePositionInt2D(Camera.main, zPosition);
        }

        /// <summary>
        /// This gets the mouse position in 2D rounded to an Int
        /// </summary>
        /// <returns>This return the mouse posintion rounded to an Int</returns>
        public static Vector2Int GetMousePositionInt2D()
        {
            return GetMousePositionInt2D(Camera.main, 0);
        }
        #endregion

        #region FallowMousePositionInt
        /// <summary>
        /// This makes an object faloow the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="obj">This is that object that fallows the mouse</param>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <param name="zPosition">This is the z position<</param>
        public static void FallowMousePositionInt2D(GameObject obj, Camera camera, int zPosition)
        {
            Vector2Int mousePosition = GetMousePositionInt2D(camera, zPosition);
            obj.transform.position = new Vector3(mousePosition.x, mousePosition.y);
        }

        /// <summary>
        /// This makes an object faloow the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="obj">This is that object that fallows the mouse</param>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        public static void FallowMousePositionInt2D(GameObject obj, Camera camera)
        {
            FallowMousePositionInt2D(obj, camera, 0);
        }

        /// <summary>
        /// This makes an object faloow the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="obj">This is that object that fallows the mouse</param>
        /// <param name="zPosition">This is the z position<</param>
        public static void FallowMousePositionInt2D(GameObject obj, int zPosition)
        {
            FallowMousePositionInt2D(obj, Camera.main, zPosition);
        }

        /// <summary>
        /// This makes an object faloow the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="obj">This is that object that fallows the mouse</param>
        public static void FallowMousePositionInt2D(GameObject obj)
        {
            FallowMousePositionInt2D(obj, Camera.main, 0);
        }
        #endregion

        #region DebugLogMousePositionInt
        /// <summary>
        /// THis logs the mouse position in 2D rouned to an Int
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <param name="zPosition">This is the z position<</param>
        public static void DebugLogMousePositionInt2D(Camera camera, int zPosition)
        {
            Vector2Int mousePos = GetMousePositionInt2D(camera, zPosition);
            Debug.Log("Mouse Position Int 2D = " + mousePos);
        }

        /// <summary>
        /// THis logs the mouse position in 2D rouned to an Int
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        public static void DebugLogMousePositionInt2D(Camera camera)
        {
            DebugLogMousePositionInt2D(camera, 0);
        }

        /// <summary>
        /// THis logs the mouse position in 2D rouned to an Int
        /// </summary>
        /// <param name="zPosition">This is the z position<</param>
        public static void DebugLogMousePositionInt2D(int zPosition)
        {
            DebugLogMousePositionInt2D(Camera.main, zPosition);
        }

        /// <summary>
        /// THis logs the mouse position in 2D rouned to an Int
        /// </summary>
        public static void DebugLogMousePositionInt2D()
        {
            DebugLogMousePositionInt2D(Camera.main, 0);
        }
        #endregion

        #endregion
    }
}
