using UnityEngine;
using System.Collections;

public class IdleState : IEnemyState

{
    private readonly AIStatePattern myObject;
    private float idleTimer;
    private float randomIdletime;

    public IdleState(AIStatePattern statePatternEnemy)
    {
        myObject = statePatternEnemy;
    }

    public void StartState()
    {
        myObject.meshRendererFlag.material.color = Color.cyan;
        randomIdletime = Random.Range(1, myObject.idleMaxTime);
    }

    public void UpdateState()
    {
        Look();
        Idle();
    }

    public void ToPatrolState()
    {
        myObject.currentState = myObject.patrolState;
        myObject.currentState.StartState();
    }

    public void ToChaseState()
    {
        myObject.currentState = myObject.chaseState;
        myObject.currentState.StartState();
    }

    public void ToWanderState()
    {
        myObject.currentState = myObject.wanderState;
        myObject.currentState.StartState();
    }

    public void ToIdleState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToAttackState()
    {
        myObject.currentState = myObject.attackState;
        myObject.currentState.StartState();
    }

    private void Look()
    {
        if (myObject._lookArea.GetTargetsInView().Count != 0)
        {
            myObject.chaseTarget = myObject._lookArea.GetTargetsInView()[0].transform;
            if(myObject.canChase)
                ToChaseState();
        }
    }

    private void Idle()
    {
        myObject.navMeshAgent.Stop();

        idleTimer += Time.deltaTime + myObject.thinkInterval;

        if (idleTimer >= randomIdletime)
        {
            idleTimer = 0;
            myObject.ToBaseState();
        }
        //Debug.Log(idleTimer);
    }
}