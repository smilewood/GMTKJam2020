using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    private HashSet<CombatUnit> combatents;
    private int playerHealth, enemyHealth;

    public Image CombatStatusBar;

    // Use this for initialization
    void Start()
    {
    }

    public void StartCombat(params CombatUnit[] units)
    {
        combatents = new HashSet<CombatUnit>();
        foreach (CombatUnit u in units)
        {
            AddUnitToCombat(u);
        }
    }

    public void AddUnitToCombat( CombatUnit u )
    {
        combatents.Add(u);
        switch (u.parent)
        {
            case PlayerUnit _:
            {
                playerHealth += u.health;
                StartCoroutine(UnitCombat(u.damageRate, u.damageChances, true));
                break;
            }
            case EnemyUnit _:
            {
                enemyHealth += u.health;
                StartCoroutine(UnitCombat(u.damageRate, u.damageChances, false));
                break;
            }
            default:
            {
                throw new System.NotImplementedException("Unit is not player or enemy???");
            }
        }
        u.parent.EnterCombat();
        this.transform.position = combatents.Select(c => c.transform.position).Aggregate(( v1, v2 ) => v1 + v2) / combatents.Count;
    }

    private IEnumerator UnitCombat(float damageRate, int damageAmmount, bool playerUnit)
    {
        while (true)
        {
            yield return new WaitForSeconds(damageRate);
            int damageProc = Random.Range(0, damageAmmount);
            (playerUnit ? ref enemyHealth : ref playerHealth) -= damageProc;

           // Debug.Log(string.Format("{0} does {1} damage", playerUnit ? "Player unit" : "Enemy unit", damageProc));
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCombatUI();
        if(playerHealth <= 0 || enemyHealth <= 0)
        {
            EndCombat();
        }
    }

    private void UpdateCombatUI()
    {
        int totalHealth = enemyHealth + playerHealth;
        float playerAdvantage = (float)playerHealth / (float)totalHealth;
        CombatStatusBar.fillAmount = playerAdvantage;
    }

    private void EndCombat()
    {
        StopAllCoroutines();
        foreach (CombatUnit u in combatents)
        {
            if(playerHealth <= 0 && u.parent is PlayerUnit)
            {
                Destroy(u.gameObject);
                continue;
            }
            if(enemyHealth <= 0 && u.parent is EnemyUnit)
            {
                Destroy(u.gameObject);
                continue;
            }
            u.EndCombat();
            u.parent.EndCombat();
        }
        Destroy(this.gameObject);
    }
}
