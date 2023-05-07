using System;

using UnityEngine;

using TheAshBot.TwoDimentional;

public class Mouse : MonoBehaviour
{


    public event EventHandler<OnSelectedChangedEventArgs> OnSelectedChanged;
    public class OnSelectedChangedEventArgs : EventArgs
    {
        public ISelectable selected;
    }


    private ISelectable selected;


    private void Update()
    {
        TestInput();
    }

    private void LateUpdate()
    {
        Mouse2D.FallowMousePosition2D(gameObject);
    }


    public ISelectable GetSelected()
    {
        return selected;
    }

    private void TestInput()
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
                OnSelectedChanged?.Invoke(this, new OnSelectedChangedEventArgs
                {
                    selected = selected,
                });
            }
        }
    }


}
