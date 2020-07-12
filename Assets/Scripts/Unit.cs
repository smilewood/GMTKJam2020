using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public abstract class Unit : MonoBehaviour
{
    internal static HashSet<Unit> units { get; private set; }

    protected static int unitIdPool = 0;

    internal int UnitID { get; private set; }

    private void Awake()
    {
        if(units is null)
        {
            units = new HashSet<Unit>();
        }
        this.UnitID = ++unitIdPool;
        units.Add(this);
    }

    public override int GetHashCode()
    {
        return UnitID.GetHashCode();
    }

    public override bool Equals( object other )
    {
        return other is Unit unit && unit.UnitID == this.UnitID;
    }

    public void OnDestroy()
    {
        units.Remove(this);
    }

    public (Unit, float) CloestEnemyUnit(bool findPlayerUnit )
    {
        float closeDist = float.MaxValue;
        Unit closeUnit = null;
        foreach (Unit u in units.Where(u => u != this && (findPlayerUnit ? u is PlayerUnit : u is EnemyUnit)))
        {
            float dist = Vector3.Distance(this.transform.position, u.transform.position);
            if (dist < closeDist)
            {
                closeDist = dist;
                closeUnit = u;
            }
        }
        return (closeUnit, closeDist);
    }

    public abstract void EnterCombat();

    public abstract void EndCombat();
}
