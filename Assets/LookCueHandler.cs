using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCueHandler : MonoBehaviour
{
    ActorDesires _ad;
    ActorMovement _am;
    SpriteRenderer _sr;

    //settings
    [SerializeField][Range(0, 90)] float _minAngleFromUp;
    [SerializeField] [Range(90, 180)] float _maxAngleFromUp;
    [SerializeField] float _defaultAngleFromUp = 90f;



    //state
    float _angle;

    private void Awake()
    {
        _ad = GetComponentInParent<ActorDesires>();
        _ad.DesiredLookChanged += SetLookCuePositionOnLookChange;
        _am = GetComponentInParent<ActorMovement>();
        _am.FacingChanged += SetLookCuePositionOnFacingChange;
        _sr = GetComponent<SpriteRenderer>();
    }

    private void SetLookCuePositionOnLookChange()
    {
        _angle = Vector3.SignedAngle(_ad.DesiredLook, Vector2.up, transform.forward);

        if (!_am.IsFacingRight)
        {
            _angle = Mathf.Clamp(_angle, -1 * _maxAngleFromUp, -1 * _minAngleFromUp);
        }
        else 
        {
            _angle = Mathf.Clamp(_angle, _minAngleFromUp, _maxAngleFromUp);
        }

        transform.rotation = Quaternion.Euler(0, 0, -1 * _angle);


        transform.localPosition = transform.rotation * Vector3.up;
    }

    private void SetLookCuePositionOnFacingChange()
    {      
        if (_am.IsFacingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, -_defaultAngleFromUp);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, _defaultAngleFromUp);
        }

        transform.localPosition = transform.rotation * Vector3.up;
    }
}
