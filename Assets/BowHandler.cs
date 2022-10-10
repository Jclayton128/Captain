using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowHandler : MonoBehaviour
{
    [SerializeField] Transform _lookCue = null;

    ProjectileController _projCon;

    ActorDesires _ad;
    LookCueHandler _cueHandler;

    //settings
    [SerializeField] float _maxDraw = 10f;
    [SerializeField] float _drawRate = 2f;
    [SerializeField] float _minDrawFactorToFire = 0.1f;
    
    [Tooltip("Arrow lifetime = Draw Charge at firing * this modifier")]
    [SerializeField] float _arrowLifetimeMultiplier = 2.0f;

    [Tooltip("Arrow Speed = Draw Charge at firing * this modifier")]
    [SerializeField] float _arrowSpeedMultiplier = 1f;

    //state
    float _currentDraw;
    float _drawFactor;

    private void Awake()
    {
        _ad = GetComponent<ActorDesires>();
        _ad.AttackChargingChanged += HandleAttackChargingChanged;
        _cueHandler = _lookCue.GetComponent<LookCueHandler>();

        _projCon = FindObjectOfType<ProjectileController>();
    }

    private void Update()
    {
        UpdateDrawCharge();
    }

    private void UpdateDrawCharge()
    {
        if (_ad.IsChargingAttack)
        {
            _currentDraw += Time.deltaTime *_drawRate;
            _currentDraw = Mathf.Clamp(_currentDraw, 0, _maxDraw);
            _drawFactor = _currentDraw / _maxDraw;
            _cueHandler.SetLookCueColorOnAttackCharge(_drawFactor);
        }
        
    }

    private void FireArrow()
    {
        ProjectileBrain pb = _projCon.RequestProjectile();
        pb.gameObject.transform.position = _lookCue.transform.position;
        pb.gameObject.transform.rotation = _lookCue.transform.rotation;
        pb.SetupUse(
            _currentDraw * _arrowLifetimeMultiplier,
            _currentDraw * _arrowSpeedMultiplier);
    }

    private void HandleAttackChargingChanged(bool isAttackCharging)
    {
        if (isAttackCharging) return;

        if (_drawFactor > _minDrawFactorToFire)
        {
            FireArrow();
        }
        _currentDraw = 0;
        _drawFactor = _currentDraw / _maxDraw;
        _cueHandler.SetLookCueColorOnAttackCharge(_drawFactor);

    }

    
}
