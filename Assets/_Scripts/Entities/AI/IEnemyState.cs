using UnityEngine;
using System.Collections;

public interface IEnemyState
{
    void StartState();

    void UpdateState();

    void ToPatrolState();

    void ToChaseState();

    void ToWanderState();

    void ToIdleState();

    void ToAttackState();
}