using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChaseState : IEnemyState
{

    private readonly AIStatePattern myObject;
    private float chaseTimer;

    public ChaseState(AIStatePattern statePatternEnemy)
    {
        myObject = statePatternEnemy;
    }

    public void StartState()
    {
        myObject.meshRendererFlag.material.color = Color.yellow;
        myObject.chaseTarget = myObject._lookArea.GetTargetsInView()[0].transform;
        myObject.navMeshAgent.destination = myObject.chaseTarget.position;
        myObject.navMeshAgent.Resume();
    }

    public void UpdateState()
    {
        ChaseTimer();
        Chase();
    }

    public void ToPatrolState()
    {
        myObject.currentState = myObject.patrolState;
        myObject.currentState.StartState();
    }

    public void ToChaseState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToWanderState()
    {
        myObject.currentState = myObject.wanderState;
        myObject.currentState.StartState();
    }

    public void ToIdleState()
    {
        myObject.currentState = myObject.idleState;
        myObject.currentState.StartState();
    }

    public void ToAttackState()
    {
        myObject.currentState = myObject.attackState;
        myObject.currentState.StartState();
    }

    private void ChaseTimer()
    {
        chaseTimer += Time.deltaTime + myObject.thinkInterval;

        if (chaseTimer >= myObject.chaseMaxTime)
        {
            chaseTimer = 0;
            myObject.chaseTarget = null;
            myObject.ToBaseState();
        }
        //Debug.Log(chaseTimer);
    }

    private void Chase()
    {
        float distanceFromTarget = Vector3.Distance(myObject.transform.position, myObject.chaseTarget.position);

        if (distanceFromTarget <= myObject.navMeshAgent.stoppingDistance)
        {
            myObject.transform.LookAt(myObject.chaseTarget);

            if(myObject._attackArea.GetTargetsInView().Count != 0)
                if (myObject.canAttack)
                    ToAttackState();
        }
        else
        {
            myObject.navMeshAgent.destination = myObject.chaseTarget.position;
            myObject.navMeshAgent.Resume();
        }
    }
}