using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(ActorDesires))]
public class ActorMovement : MonoBehaviour
{
    public Action FacingChanged;

    #region References
    ActorDesires _ad;

    private void Awake()
    {
        _ad = GetComponent<ActorDesires>();
    }

    #endregion

    #region Settings
    [Tooltip("World-units per second")]
    [SerializeField] float _moveSpeed = 1.0f;

    #endregion

    #region State
    bool _isFacingRight;
    public bool IsFacingRight => _isFacingRight;

    /// <summary>
    /// Negative X is West, Positive X is East. Y+Z should be constrained to zero.
    /// </summary>
    Vector3 _velocity;
    #endregion

    #region Flow

    private void FixedUpdate()
    {
        _velocity = Vector3.right * _ad.DesiredMove * _moveSpeed;
        transform.position += _velocity * Time.fixedDeltaTime;
        UpdateFacing();
    }

    private void UpdateFacing()
    {
        if (Mathf.Abs(_velocity.x) < Mathf.Epsilon) return; // Retain current heading if no desired move.
        bool currentFacing = _isFacingRight;
        if (_velocity.x > 0)
        {
            _isFacingRight = true;
        }
        if (_velocity.x < 0)
        {
            _isFacingRight = false;
        }

        if (currentFacing != _isFacingRight)
        {
            FacingChanged?.Invoke();
        }
        
    }


    #endregion
}
