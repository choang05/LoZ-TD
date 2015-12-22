using UnityEngine;
using System.Collections;

public class FleeState : IEnemyState
{
    private readonly AIStatePattern myObject;
    private float fleeingTimer;
    private float randomFleeTime;

    public FleeState(AIStatePattern statePattern)
    {
        myObject = statePattern;
    }

    public void StartState()
    {
        myObject.meshRendererFlag.material.color = Color.white;
        randomFleeTime = Random.Range(3, myObject.idleMaxTime);
        Flee();
    }

    public void UpdateState()
    {
        Look();
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
        myObject.currentState = myObject.attackState;
        myObject.currentState.StartState();
    }

    public void ToFleeState()
    {
        Debug.Log("Can't transition to same state");
    }

    private void Look()
    {
        if (myObject._lookArea.GetTargetsInView().Count != 0)
        {
            myObject.currentTarget = myObject._lookArea.GetTargetsInView()[0].transform;
            Flee();
        }

        fleeingTimer += Time.deltaTime + myObject.thinkInterval;
        if (fleeingTimer >= randomFleeTime)
        {
            fleeingTimer = 0;
            myObject.currentTarget = null;
            myObject.ToBaseState();
        }
    }

    private void Flee()
    {
        //temporarily point the object to look away from the player
        myObject.transform.rotation = Quaternion.LookRotation(myObject.transform.position - myObject.currentTarget.position);

        //Then we'll get the position on that rotation that's multiplyBy down the path (you could set a Random.range
        // for this if you want variable results) and store it in a new Vector3 called newPosition
        Vector3 newPosition = myObject.transform.position + myObject.transform.forward * 10;
        //Debug.Log("runTo = " + runTo);

        //So now we've got a Vector3 to run to and we can transfer that to a location on the NavMesh with samplePosition.

        NavMeshHit hit;    // stores the output in a variable called hit

        // 5 is the distance to check, assumes you use default for the NavMesh Layer name
        NavMesh.SamplePosition(newPosition, out hit, 5, 1 << NavMesh.GetAreaFromName("Walkable"));
        //Debug.Log("hit = " + hit + " hit.position = " + hit.position);

        // And get it to head towards the found NavMesh position
        myObject.navMeshAgent.Resume();
        myObject.navMeshAgent.SetDestination(hit.position);

        fleeingTimer = 0;
    }
}