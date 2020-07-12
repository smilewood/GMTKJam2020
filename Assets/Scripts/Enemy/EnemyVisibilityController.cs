using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisibilityController : MonoBehaviour
{
    GameObject visuals;
    public bool ReHideWhenOutOfVision = true;
    // Start is called before the first frame update
    void Start()
    {
        visuals = this.gameObject.transform.Find("Visuals").gameObject;
        visuals.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerUnitInVisibilityRange())
        {
            visuals.SetActive(true);
            if (!ReHideWhenOutOfVision)
            {
                this.enabled = false;
            }
        }
        else
        {
            visuals.SetActive(false);
        }
    }

    private bool PlayerUnitInVisibilityRange()
    {
        foreach (PlayerUnit u in PlayerUnitController.Instance.units)
        {
            if(Vector3.Distance(this.transform.position, u.gameObject.transform.position) < u.VisionRange)
            {
                return true;
            }
        }
        return false;
    }
}
