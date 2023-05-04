using UnityEngine;

namespace TheAshBot.TwoDimentional
{
    public class Mouse2D
    {
        static private Vector3 mouseWorldPosition;
        static private Vector3Int mousePositionInt;

        #region Mouse Position Vector2

        #region GetObjectAtMousePosition
        
        public static Vector3 GetMousePosition2D(Camera camera, float zPos)
        {
            mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = zPos;
            return mouseWorldPosition;
        }

        #endregion

        #region GetMousePosition
        /// <summary>
        /// This gets the mouse position in 2D
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <param name="zPos">This is the z position</param>
        /// <returns>This return the mouse posintion</returns>
        public static Vector3 GetMousePosition2D(Camera camera, float zPos)
        {
            mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = zPos;
            return mouseWorldPosition;
        }

        /// <summary>
        /// This gets the mouse position in 2D
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <returns>This return the mouse posintion</returns>
        public static Vector3 GetMousePosition2D(Camera camera)
        {
            mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            return mouseWorldPosition;
        }

        /// <summary>
        /// This gets the mouse position in 2D
        /// </summary>
        /// <param name="zPos">This is the z position</param>
        /// <returns>This return the mouse posintion</returns>
        public static Vector3 GetMousePosition2D(float zPos)
        {
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = zPos;
            return mouseWorldPosition;
        }

        /// <summary>
        /// This gets the mouse position in 2D
        /// </summary>
        /// <returns>This return the mouse posintion</returns>
        public static Vector3 GetMousePosition2D()
        {
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            return mouseWorldPosition;
        }
        #endregion

        #region FallowMousePosition
        /// <summary>
        /// This makes an object faloow the mouse position in 2D
        /// </summary>
        /// <param name="obj">This is the object that fallows the mouse</param>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <param name="zPos">This is the z position</param>
        public static void FallowMousePosition2D(GameObject obj, Camera camera, float zPos)
        {
            obj.transform.position = GetMousePosition2D(camera, zPos);
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
        /// <param name="zPos">This is the z position</param>
        public static void FallowMousePosition2D(GameObject obj, float zPos)
        {
            obj.transform.position = GetMousePosition2D(zPos);
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
        /// <param name="zPos">This is the z position</param>
        public static void DebugLogMousePositionFloat2D(Camera camera, float zPos)
        {
            Vector3 mousePos = GetMousePosition2D(camera, zPos);
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
        /// <param name="zPos">This is the z position</param>
        public static void DebugLogMousePositionFloat2D(float zPos)
        {
            Vector3 mousePos = GetMousePosition2D(zPos);
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
        /// <param name="zPos">This is the z position</param>
        /// <returns>This return the mouse posintion rounded to an Int</returns>
        public static Vector3Int GetMousePositionInt2D(Camera camera, int zPos)
        {
            GetMousePosition2D(camera, zPos);
            mousePositionInt = Vector3Int.RoundToInt(mouseWorldPosition);
            return mousePositionInt;
        }

        /// <summary>
        /// This gets the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <returns>This return the mouse posintion rounded to an Int</returns>
        public static Vector3Int GetMousePositionInt2D(Camera camera)
        {
            GetMousePosition2D(camera);
            mousePositionInt = Vector3Int.RoundToInt(mouseWorldPosition);
            return mousePositionInt;
        }

        /// <summary>
        /// This gets the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="zPos">This is the z position</param>
        /// <returns>This return the mouse posintion rounded to an Int</returns>
        public static Vector3Int GetMousePositionInt2D(int zPos)
        {
            GetMousePosition2D(zPos);
            mousePositionInt = Vector3Int.RoundToInt(mouseWorldPosition);
            return mousePositionInt;
        }

        /// <summary>
        /// This gets the mouse position in 2D rounded to an Int
        /// </summary>
        /// <returns>This return the mouse posintion rounded to an Int</returns>
        public static Vector3Int GetMousePositionInt2D()
        {
            GetMousePosition2D();
            mousePositionInt = Vector3Int.RoundToInt(mouseWorldPosition);
            return mousePositionInt;
        }
        #endregion

        #region FallowMousePositionInt
        /// <summary>
        /// This makes an object faloow the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="obj">This is that object that fallows the mouse</param>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <param name="zPos">This is the z position<</param>
        public static void FallowMousePositionInt2D(GameObject obj, Camera camera, int zPos)
        {
            obj.transform.position = GetMousePositionInt2D(camera, zPos);
        }

        /// <summary>
        /// This makes an object faloow the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="obj">This is that object that fallows the mouse</param>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        public static void FallowMousePositionInt2D(GameObject obj, Camera camera)
        {
            obj.transform.position = GetMousePositionInt2D(camera);
        }

        /// <summary>
        /// This makes an object faloow the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="obj">This is that object that fallows the mouse</param>
        /// <param name="zPos">This is the z position<</param>
        public static void FallowMousePositionInt2D(GameObject obj, int zPos)
        {
            obj.transform.position = GetMousePositionInt2D(zPos);
        }

        /// <summary>
        /// This makes an object faloow the mouse position in 2D rounded to an Int
        /// </summary>
        /// <param name="obj">This is that object that fallows the mouse</param>
        public static void FallowMousePositionInt2D(GameObject obj)
        {
            obj.transform.position = GetMousePositionInt2D();
        }
        #endregion

        #region DebugLogMousePositionInt
        /// <summary>
        /// THis logs the mouse position in 2D rouned to an Int
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        /// <param name="zPos">This is the z position<</param>
        public static void DebugLogMousePositionInt2D(Camera camera, int zPos)
        {
            Vector3Int mousePos = GetMousePositionInt2D(camera, zPos);
            Debug.Log("Mouse Position Int 2D = " + mousePos);
        }

        /// <summary>
        /// THis logs the mouse position in 2D rouned to an Int
        /// </summary>
        /// <param name="camera">This is the camera that the it gets the position from</param>
        public static void DebugLogMousePositionInt2D(Camera camera)
        {
            Vector3Int mousePos = GetMousePositionInt2D(camera);
            Debug.Log("Mouse Position Int 2D = " + mousePos);
        }

        /// <summary>
        /// THis logs the mouse position in 2D rouned to an Int
        /// </summary>
        /// <param name="zPos">This is the z position<</param>
        public static void DebugLogMousePositionInt2D(int zPos)
        {
            Vector3Int mousePos = GetMousePositionInt2D(zPos);
            Debug.Log("Mouse Position Int 2D = " + mousePos);
        }

        /// <summary>
        /// THis logs the mouse position in 2D rouned to an Int
        /// </summary>
        public static void DebugLogMousePositionInt2D()
        {
            Vector3Int mousePos = GetMousePositionInt2D();
            Debug.Log("Mouse Position Int 2D = " + mousePos);
        }
        #endregion

        #endregion
    }
}
