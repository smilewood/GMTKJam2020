using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Unit))]
public class CombatUnit : MonoBehaviour
{
    internal static HashSet<CombatUnit> units { get; private set; }
    public List<ParticleSystem> combatParticles;

    public float damageRate;
    public int damageChances;
    public int health;
    public float engageRange;
    public GameObject CombatManagerPrefab;
    internal Unit parent;
    private CombatManager activeCombat;
    private bool playerUnit;
    // Use this for initialization
    void Start()
    {
        if(units is null)
        {
            units = new HashSet<CombatUnit>();
        }
        parent = this.gameObject.GetComponent<Unit>();
        playerUnit = parent is PlayerUnit;
        units.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(activeCombat is null)
        {
            foreach (CombatUnit unit in units.Where(u=>u!=this && u.playerUnit != this.playerUnit))
            {
                //The odds of simaltionusly finding two units that are not already in combat should be so low I can ignote them. 
                if(Vector3.Distance(this.transform.position, unit.transform.position) < engageRange)
                {
                    this.activeCombat = unit.EngageInCombat();
                    PlayParticles();
                    activeCombat.AddUnitToCombat(this);
                }
            }
        }
    }

    public CombatManager EngageInCombat()
    {
        if (activeCombat is null)
        {
            CombatManager manager = Instantiate(CombatManagerPrefab).GetComponent<CombatManager>();
            manager.StartCombat(this);
            this.activeCombat = manager;
            PlayParticles();
            return manager;
        }
        else return activeCombat;
    }

    internal void EndCombat()
    {
        this.activeCombat = null;
        foreach(ParticleSystem s in combatParticles)
        {
            s.Stop();
        }
    }

    private void PlayParticles()
    {
        foreach (ParticleSystem s in combatParticles)
        {
            s.Play();
        }
    }

    public override bool Equals( object obj )
    {
        return obj is CombatUnit unit && unit.parent == this.parent;
    }

    public override int GetHashCode()
    {
        return parent.GetHashCode();
    }

    private void OnDestroy()
    {
        units.Remove(this);
    }
}
