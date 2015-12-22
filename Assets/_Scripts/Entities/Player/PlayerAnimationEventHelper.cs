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
        if(_playerMelee.enabled)
            _playerMelee.ApplyAttack();
        else if(_playerRange.enabled)
            _playerRange.ApplyAttack();
    }

    public void ProcessAttackReset()
    {
        if (_playerMelee.enabled)
            _playerMelee.ResetAttack();
        else if (_playerRange.enabled)
            _playerRange.ResetAttack();
    }

    public void ProcessResetRoll()
    {
        if (_playerRange.enabled)
            _playerRange.ResetRoll();
    }
    public void ProcessCast_Fireball()
    {
        if (_playerMagic.enabled)
            _playerMagic.CastFireball();
    }
    public void ProcessCast_Boulder()
    {
        if (_playerMagic.enabled)
            _playerMagic.CastBoulder();
    }
}
