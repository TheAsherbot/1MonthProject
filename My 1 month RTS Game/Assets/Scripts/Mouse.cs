using System.Collections;
using System.Collections.Generic;

using TheAshBot.TwoDimentional;

using UnityEngine;

public class Mouse : MonoBehaviour
{


    private ISelectable selected;


    private void Update()
    {
        HandelInput();
    }

    private void LateUpdate()
    {
        Mouse2D.FallowMousePosition2D(gameObject);
    }


    private void HandelInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Select();
        }
    }

    private void Select()
    {
        if (selected != null)
        {
            selected.Unselect();
            selected = null;
        }

        if (Mouse2D.TryGetObjectAtMousePosition(out GameObject hit))
        {
            if (hit.TryGetComponent(out selected))
            {
                selected.Select();
            }
        }
    }


}
