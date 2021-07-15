using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour, IDisposable
{
    protected delegate void SetUpOp();
    protected event SetUpOp SetUpOperation;

    protected virtual void Start()
    {
        SetUpOperation?.Invoke();
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public virtual void Clear() { }

    public void Dispose()
    {
        Clear();
    }
}
