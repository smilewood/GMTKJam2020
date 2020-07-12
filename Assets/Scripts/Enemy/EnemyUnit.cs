using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class EnemyUnit : Unit
{
    private Unit targetUnit;
    public float agroRange;
    public float speed;
    bool inCombat = false;
    NavMeshAgent pathAgent;

    static int enemyUnitCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        pathAgent = GetComponent<NavMeshAgent>();
        ++enemyUnitCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (inCombat)
        {
            return;
        }

        if(targetUnit is null)
        {
            (Unit unit, float dist) = CloestEnemyUnit(true);
            if(dist < agroRange)
            {
                targetUnit = unit;
            }
        }
        else if(Vector3.Distance(this.gameObject.transform.position, targetUnit.gameObject.transform.position) > agroRange)
        {
            targetUnit = null;
            pathAgent.isStopped = true;
        }
        else
        {
            pathAgent.destination = targetUnit.transform.position;
            pathAgent.isStopped = false;
        }


    }

    public override void EnterCombat()
    {
        inCombat = true;
        pathAgent.isStopped = true;
    }

    public override void EndCombat()
    {
        inCombat = false;
        targetUnit = null;
    }

    public new void OnDestroy()
    {
        base.OnDestroy();
        --enemyUnitCount;
        if(enemyUnitCount == 0)
        {
            GameManager.Instance.GameWin();
        }
    }
}
