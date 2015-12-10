using UnityEngine;
using System.Collections;

public class WanderState : IEnemyState

{
    private readonly AIStatePattern myObject;
    private float thinkTimer;

    public WanderState(AIStatePattern statePattern)
    {
        myObject = statePattern;
    }

    public void StartState()
    {
        myObject.meshRendererFlag.material.color = Color.green;
    }

    public void UpdateState()
    {
        Look();
        Wander();
    }

    public void OnTriggerEnter(Collider other)
    {

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
        myObject.navMeshAgent.destination = GetRandomLocation(myObject.transform.position, myObject.wanderRadius, -1);
        Debug.Log("Can't transition to same state");
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

    private void Look()
    {
        if (myObject._lookArea.GetTargetsInView().Count != 0)
        {
            myObject.chaseTarget = myObject._lookArea.GetTargetsInView()[0].transform;
            ToChaseState();
        }
    }

    private void Wander()
    {
        myObject.navMeshAgent.Resume();

        if (myObject.navMeshAgent.remainingDistance <= myObject.navMeshAgent.stoppingDistance && !myObject.navMeshAgent.pathPending)
        {
            myObject.navMeshAgent.destination = GetRandomLocation(myObject.wanderOrigin, myObject.wanderRadius, -1);
            ToIdleState();
        }
    }

    private Vector3 GetRandomLocation(Vector3 origin, float distance, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * distance;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, distance, layermask);

        return navHit.position;
    }

}