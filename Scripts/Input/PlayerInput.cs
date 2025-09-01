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
        SwitchActionMap(inputActions.Gameplay);//����Gameplay������
    }

    
    void SwitchActionMap(InputActionMap actionMap)
    {
        inputActions.Disable();
        actionMap.Enable();
    }//�л�������

    //�������ж�����
    public void DisableAllInputs()
    {
        inputActions.Disable();
    }
    public void OnDodge(InputAction.CallbackContext context)
    {
        if(context.started)//������һ��
        {
            onDodge?.Invoke();
        }
    }

    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        if(context.started)//������һ��
        {
            onMeleeAttack?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)//�������� 
        {
            onMove?.Invoke(context.ReadValue<Vector2>());
        }
        if(context.canceled)
        {
            onStopMove?.Invoke();
        }


        //switch(context.phase)
        //{
        //    case InputActionPhase.Started://���µ���һ��
        //        Debug.Log("start");
        //        break;
        //    case InputActionPhase.Performed://��������
        //        Debug.Log("perfoem");
        //        break;
        //    case InputActionPhase.Canceled://�ɿ�
        //        Debug.Log("cancel");
        //        break;
        //}
    }
}
