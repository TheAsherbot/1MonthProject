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
    private GameInputActions inputActions;


    private void Start()
    {
        inputActions = new GameInputActions();
        inputActions.Game.Enable();
        inputActions.Game.Select.performed += Select_performed;
    }

    private void LateUpdate()
    {
        Mouse2D.FallowMousePosition2D(gameObject);
    }


    public ISelectable GetSelected()
    {
        return selected;
    }
    
    private void Select_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!GridManager.Instance.grid.IsPositionOnGrid(Mouse2D.GetMousePosition2D())) return;

        Select();
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
