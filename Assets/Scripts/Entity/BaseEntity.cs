using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour, IDisposable
{
    protected delegate void SetUpOp();
    protected event SetUpOp SetUpOperation;

    public Vector3 Forward => (gameObject != null) ? gameObject.transform.forward : Vector3.zero;
    public Vector3 Position => (gameObject != null) ? gameObject.transform.position : Vector3.zero;
    public Quaternion Rotation => (gameObject != null) ? gameObject.transform.rotation : Quaternion.identity;
    public Transform Transform => (gameObject != null) ? gameObject.transform : null;

    public EntityManager.EntityType EntityType { get; protected set; }

    protected virtual void OnEnable()
    {
        SetUpOperation?.Invoke();
    }

    protected virtual void Start()
    {
        SetUpOperation?.Invoke();
    }

    public virtual void Clear() { }

    public void Dispose()
    {
        Clear();
    }
}
