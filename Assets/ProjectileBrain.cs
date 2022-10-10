using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBrain : MonoBehaviour
{
    ProjectileController _projCon;
    Rigidbody2D _rb;
    CircleCollider2D _collider;

    //settings
    [SerializeField] float _arrowSinkAmount_ground = 0.2f;

    float _lifetimeRemaining;
    float _angle;

    public void Initialize(ProjectileController pcRef)
    {
        _projCon = pcRef;
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
    }

    public void SetupUse(float lifetime, float speed)
    {
        _lifetimeRemaining = lifetime;
        _collider.enabled = true;
        _rb.simulated = true;
        _rb.velocity = transform.up * speed;
        _angle = -90f + Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;
    }

    private void Update()
    {
        UpdateProjectileRotation();
        UpdateLifetimeCheck();
    }

    private void UpdateProjectileRotation()
    {
        _angle = -90 + Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
    }

    private void UpdateLifetimeCheck()
    {
        _lifetimeRemaining -= Time.deltaTime;
        if (_lifetimeRemaining <= 0)
        {
            Expire();
        }
    }

    private void Expire()
    {
        _projCon.ReturnProjectile(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;

        if (otherLayer == 6)
        {
            transform.Translate(Vector3.up * _arrowSinkAmount_ground);
        }
        if (otherLayer == 8)
        {
            transform.parent = other.transform;
            transform.Translate(Vector3.up * _arrowSinkAmount_ground);
        }

        _rb.simulated = false;
        _collider.enabled = false;
    }
}
