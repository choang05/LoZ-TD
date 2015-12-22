using UnityEngine;
using System.Collections;

public class AnimationEventHelper : MonoBehaviour
{
    private EnemyAttack _enemyAttack;

    void Awake()
    {
        _enemyAttack = GetComponentInParent<EnemyAttack>();
    }

    public void ProcessHitEvent()
    {

    }

    public void ProcessAttackEvent()
    {
        _enemyAttack.Attack();
    }

    public void ProcessAttackReset()
    {
        _enemyAttack.ResetAttack();
    }
}
