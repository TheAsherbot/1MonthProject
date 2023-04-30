using System.Collections;
using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using UnityEngine;

public class TestPointInsideHexagon : MonoBehaviour
{


    [SerializeField] private MeshRenderer mouseMeshRenderer;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;


    private Hexagon hexagon;


    private void Start()
    {
        hexagon = new Hexagon(new Vector2(0, 0), 0.5f);
    }

    private void Update()
    {
        Vector2 testPosition = Mouse2D.GetMousePosition2D();

        mouseMeshRenderer.material = redMaterial;

        if (testPosition.x < hexagon.upperRightCorner.x &&
            testPosition.x > hexagon.upperLeftCorner.x)
        {
            // Inside the horizonal bounds
            if (testPosition.y < hexagon.upperCorner.y &&
                testPosition.y > hexagon.lowerCorner.y)
            {
                // Inside the vertical bounds

                Vector3 directionFromUpperRightCornerToUpperCorner = hexagon.upperCorner - hexagon.upperRightCorner;
                Vector3 dotDirectionUpperRightCorner = Quaternion.Euler(0, 0, -90) * directionFromUpperRightCornerToUpperCorner;
                Vector3 directionFromUpperRightCornerToTestPoint = testPosition - hexagon.upperRightCorner;
                float dotUpperRightCorner = Vector3.Dot(dotDirectionUpperRightCorner.normalized, directionFromUpperRightCornerToTestPoint.normalized);
                
                Vector3 directionFromUpperLeftCornerToUpperCorner = hexagon.upperCorner - hexagon.upperLeftCorner;
                Vector3 dotDirectionUpperLeftCorner = Quaternion.Euler(0, 0, +90) * directionFromUpperLeftCornerToUpperCorner;
                Vector3 directionFromUpperLeftCornerToTestPoint = testPosition - hexagon.upperLeftCorner;
                float dotUpperLeftCorner = Vector3.Dot(dotDirectionUpperLeftCorner.normalized, directionFromUpperLeftCornerToTestPoint.normalized);
                
                Vector3 directionFromLowerRightCornerToLowerCorner = hexagon.lowerCorner - hexagon.lowerRightCorner;
                Vector3 dotDirectionLowerRightCorner = Quaternion.Euler(0, 0, +90) * directionFromLowerRightCornerToLowerCorner;
                Vector3 directionFromLowerRightCornerToTestPoint = testPosition - hexagon.lowerRightCorner;
                float dotLowerRightCorner = Vector3.Dot(dotDirectionLowerRightCorner.normalized, directionFromLowerRightCornerToTestPoint.normalized);
                
                Vector3 directionFromLowerLeftCornerToLowerCorner = hexagon.lowerCorner - hexagon.lowerLeftCorner;
                Vector3 dotDirectionLowerLeftCorner = Quaternion.Euler(0, 0, -90) * directionFromLowerLeftCornerToLowerCorner;
                Vector3 directionFromLowerLeftCornerToTestPoint = testPosition - hexagon.lowerLeftCorner;
                float dotLowerLeftCorner = Vector3.Dot(dotDirectionLowerLeftCorner.normalized, directionFromLowerLeftCornerToTestPoint.normalized);
                


                if (dotUpperRightCorner < 0 && dotUpperLeftCorner < 0 && dotLowerRightCorner < 0 && dotLowerLeftCorner < 0)
                {
                    mouseMeshRenderer.material = greenMaterial;
                }
            }
        }

        Mouse2D.FallowMousePosition2D(gameObject);
    }


}


public struct Hexagon
{
    public float halfSize;
    public Vector2 centerPoint;
    public Vector2 upperRightCorner;
    public Vector2 upperLeftCorner;
    public Vector2 upperCorner;
    public Vector2 lowerCorner;
    public Vector2 lowerLeftCorner;
    public Vector2 lowerRightCorner;


    public Hexagon(Vector2 centerPoint, float halfSize)
    {
        this.centerPoint = centerPoint;
        this.halfSize = halfSize;

        upperCorner = centerPoint + new Vector2(0, +1) * halfSize; 
        lowerCorner = centerPoint + new Vector2(0, -1) * halfSize;

        upperRightCorner = centerPoint + new Vector2(+1, +0.5f) * halfSize;
        upperLeftCorner  = centerPoint + new Vector2(-1, +0.5f) * halfSize;
        lowerRightCorner = centerPoint + new Vector2(+1, -0.5f) * halfSize;
        lowerLeftCorner  = centerPoint + new Vector2(-1, -0.5f) * halfSize;
        
    }
}
