using UnityEngine;
using System.Collections;

public class PlayerAnimationEventHelper : MonoBehaviour
{
    PlayerMelee _playerMelee;
    PlayerRange _playerRange;
    PlayerMagic _playerMagic;

    void Awake()
    {
        _playerMelee = GetComponentInParent<PlayerMelee>();
        _playerRange = GetComponentInParent<PlayerRange>();
        _playerMagic = GetComponentInParent<PlayerMagic>();
    }

    public void ProcessAttackEvent()
    {
        if(_playerMelee.isActiveAndEnabled)
            _playerMelee.ApplyAttack();
        else if(_playerRange.isActiveAndEnabled)
            _playerRange.ApplyAttack();
    }
    public void ProcessAttackReset()
    {
        if (_playerMelee.isActiveAndEnabled)
            _playerMelee.ResetAttack();
        else if (_playerRange.isActiveAndEnabled)
            _playerRange.ResetAttack();
    }
    public void ProcessResetRoll()
    {
        if (_playerRange.isActiveAndEnabled)
            _playerRange.ResetRoll();
    }
}
