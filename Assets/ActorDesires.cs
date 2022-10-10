using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class ActorDesires : MonoBehaviour
{
    public Action DesiredLookChanged;
    public Action<bool> AttackChargingChanged;

    #region References
    PlayerInput _playerInput;
    Camera _camera;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _camera = Camera.main;
    }

#endregion


#region State
    [SerializeField] Vector2 _rawLook;
    [SerializeField] Vector2 _mousePos_Raw;
    [SerializeField] Vector2 _mousePos_Relative;
    Ray ray;
    float distance;
    Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
    Vector2 _desiredLook;
    public Vector2 DesiredLook => _desiredLook;

    /// <summary>
    /// Negative desired move = West, Positive = East.
    /// </summary>
    float _desiredMove;
    Vector2 _rawMove;
    public float DesiredMove => _desiredMove;

    [SerializeField] bool _isChargingAttack = false;
    public bool IsChargingAttack => _isChargingAttack;


#endregion

#region Input Handlers
    public void OnMove(InputValue value)
    {
        _rawMove = value.Get<Vector2>();
        if (Mathf.Abs(_rawMove.x) < Mathf.Epsilon)
        {
            _desiredMove = 0;
        }
        else
        {
            _desiredMove = (_rawMove.x > 0) ? 1 : -1;
        }
    }

    public void OnLook(InputValue value)
    {
        _rawLook = value.Get<Vector2>();
        if (_playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            xy.Raycast(ray, out distance);
            _mousePos_Raw = ray.GetPoint(distance);
            _mousePos_Relative = _mousePos_Raw - (Vector2)transform.position;

            _desiredLook = (_mousePos_Relative).normalized;
            DesiredLookChanged?.Invoke();
            return;
        }

        if (_playerInput.currentControlScheme == "Gamepad")
        {
            if (_rawLook.magnitude < Mathf.Epsilon) return;
            else
            {
                _desiredLook = value.Get<Vector2>();
            }
            DesiredLookChanged?.Invoke();
        }



    }

    public void OnFire(InputValue value)
    {
        bool currentStatus = _isChargingAttack;

        if (_playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            //_isChargingAttack = value.Get<bool>();
            _isChargingAttack = value.isPressed;
        }

        if (_playerInput.currentControlScheme == "Gamepad")
        {
            _isChargingAttack = value.isPressed;//(value.Get<float>() > .1f) ? true : false;
        }

        if (currentStatus != _isChargingAttack)
        {
            AttackChargingChanged?.Invoke(_isChargingAttack);
        }
    }

#endregion
}
