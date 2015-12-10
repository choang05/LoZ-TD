using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackState : IEnemyState
{

    private readonly AIStatePattern myObject;
    private float thinkTimer;

    public AttackState(AIStatePattern statePatternEnemy)
    {
        myObject = statePatternEnemy;
    }

    public void StartState()
    {
        myObject.meshRendererFlag.material.color = Color.red;
        myObject.chaseTarget = myObject._lookArea.GetTargetsInView()[0].transform;
        myObject.navMeshAgent.destination = myObject.chaseTarget.position;
        myObject.navMeshAgent.Resume();
    }

    public void UpdateState()
    {
        Attack();
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
        myObject.currentState = myObject.idleState;
        myObject.currentState.StartState();
    }

    public void ToAttackState()
    {
        Debug.Log("Can't transition to same state");
    }


    private void Attack()
    {
        if (myObject._attackArea.GetTargetsInView().Count != 0)
        {
            //Debug.Log("Attack!");
            myObject._enemyAttack.Attack();
        }
        else
        {
            ToChaseState();
        }
    }
}