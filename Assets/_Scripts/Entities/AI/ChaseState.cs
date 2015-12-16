using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChaseState : IEnemyState
{

    private readonly AIStatePattern myObject;
    private float distanceFromTarget;
    private float chaseTimer;

    public ChaseState(AIStatePattern statePatternEnemy)
    {
        myObject = statePatternEnemy;
    }

    public void StartState()
    {
        chaseTimer = 0;

        myObject.meshRendererFlag.material.color = Color.yellow;
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
            myObject.currentTarget = null;
            myObject.ToBaseState();
        }
        //Debug.Log(chaseTimer);
    }

    private void Chase()
    {
        if (myObject.currentTarget && !myObject.currentTarget.GetComponent<Health>().IsDead)
        {
            distanceFromTarget = Vector3.Distance(myObject.transform.position, myObject.currentTarget.position);
            if (distanceFromTarget <= myObject.navMeshAgent.stoppingDistance)
            {
                myObject.transform.LookAt(myObject.currentTarget);
                if (myObject.canAttack)
                    ToAttackState();
            }
            else
            {
                myObject.navMeshAgent.destination = myObject.currentTarget.position;
                myObject.navMeshAgent.Resume();
            }
        }
        else
        {
            myObject.currentTarget = null;
            myObject.ToBaseState();
        }
    }
}