using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChaseState : IEnemyState
{

    private readonly StatePattern myObject;
    private bool isChasing = false;

    public ChaseState(StatePattern statePatternEnemy)
    {
        myObject = statePatternEnemy;
    }

    public void StartState()
    {
        myObject.meshRendererFlag.material.color = Color.blue;
        myObject.chaseTarget = myObject._lookArea.GetTargetsInView()[0].transform;
        myObject.navMeshAgent.destination = myObject.chaseTarget.position;
        myObject.navMeshAgent.Resume();
    }

    public void UpdateState()
    {
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

    private void Chase()
    {
        float distanceFromTarget = Vector3.Distance(myObject.transform.position, myObject.chaseTarget.position);

        if (distanceFromTarget <= myObject.navMeshAgent.stoppingDistance && myObject._attackArea.GetTargetsInView().Count != 0)
        {
            ToAttackState();
        }
        else
        {
            myObject.navMeshAgent.destination = myObject.chaseTarget.position;
            myObject.navMeshAgent.Resume();
        }
    }
}