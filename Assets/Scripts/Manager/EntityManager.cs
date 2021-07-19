using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : Singleton<EntityManager>
{
    public readonly Dictionary<long, BaseEntity> m_EntityTable = new Dictionary<long, BaseEntity>();
    public PlayerEntity MainPlayer { get; private set; }

    public Action<long> OnRemoveEntity_Event { get; set; }

    public enum EntityType
    {
        None,
        Zombie,
        Player,
        Item
    }

    //public BaseEntity AddEntity()
    //{
    //    //if(m_EntityTable.ContainsKey())
    //}

    public void RemoveEntity()
    {

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
