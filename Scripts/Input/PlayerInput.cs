using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.VisualScripting;

[CreateAssetMenu(fileName="Player Input")]
public class PlayerInput : ScriptableObject,InputActions.IGameplayActions
{
    public event UnityAction<Vector2> onMove;

    public event UnityAction onStopMove;

    public event UnityAction onDodge;

    public event UnityAction onMeleeAttack;

    InputActions inputActions;

    private void OnEnable()
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);
    }

    public void EnableGameplayInput()
    {
        SwitchActionMap(inputActions.Gameplay);//启用Gameplay动作表
    }

    
    void SwitchActionMap(InputActionMap actionMap)
    {
        inputActions.Disable();
        actionMap.Enable();
    }//切换动作表

    //禁用所有动作表
    public void DisableAllInputs()
    {
        inputActions.Disable();
    }
    public void OnDodge(InputAction.CallbackContext context)
    {
        if(context.started)//按下那一刻
        {
            onDodge?.Invoke();
        }
    }

    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        if(context.started)//按下那一刻
        {
            onMeleeAttack?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)//持续按下 
        {
            onMove?.Invoke(context.ReadValue<Vector2>());
        }
        if(context.canceled)
        {
            onStopMove?.Invoke();
        }


        //switch(context.phase)
        //{
        //    case InputActionPhase.Started://按下的那一刻
        //        Debug.Log("start");
        //        break;
        //    case InputActionPhase.Performed://持续按下
        //        Debug.Log("perfoem");
        //        break;
        //    case InputActionPhase.Canceled://松开
        //        Debug.Log("cancel");
        //        break;
        //}
    }
}
