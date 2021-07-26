using UnityEngine;

public abstract class ItemEntity : BaseEntity, IItem
{
    public float LivingTime { get; protected set; } = 15.0f;
    public uint ItemCode { get; protected set; }

    protected delegate void ItemOp();
    protected event ItemOp ItemOperation;

    public float turnSpeed = 20f;


    protected virtual void Awake()
    {
        SetUpOperation += ItemSetUpOp;
    }

    protected virtual void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed);
    }

    protected abstract void ItemSetUpOp();

    public void OnUse()
    {
        ItemOperation?.Invoke();
    }
}
