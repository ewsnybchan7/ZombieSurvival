using System;
using System.Collections;
using UnityEngine;


public class State 
{
    protected StateControl stateControl;
    protected BattleEntity ownerEntity;

    private Action<StateControl.BATTLE_STATE> changeCallback = null;

    public State(StateControl control, Action<StateControl.BATTLE_STATE> callback)
    {
        stateControl = control;
        ownerEntity = control.OwnerEntity;
        changeCallback = callback;
    }

    // State 진입
    public virtual void Enter()
    {
        Debug.Log("Enter => Entity : " + ownerEntity + "State : " + stateControl.prevState + "->" + stateControl.eState);
    }

    public virtual void Exit() { }
    public virtual bool CheckState() { return true; }
    
    public virtual void Update()
    {
        var target = stateControl.TargetEntity;

        if (target == null)
            return;

        var dist = Vector3.Distance(ownerEntity.transform.position, target.transform.position);

        if (!(dist > ownerEntity.ChaseRange))
            return;

        stateControl.TargetEntity = null;
        stateControl.ChangeState();
    }

    public void StateChange(StateControl.BATTLE_STATE state)
    {
        changeCallback?.Invoke(state);
    }
}
