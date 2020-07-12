using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerUnitController : MonoBehaviour
{
    public static PlayerUnitController Instance { get; private set; }
    PlayerUnit currentUnit, UnitToSelect;
    Camera mainCamera;
    private static int moveLayerMask;
    MouseParticleController particleController;
    public Vector4 bounds;
    public HashSet<PlayerUnit> units;
    private void Awake()
    {
        units = new HashSet<PlayerUnit>();

        moveLayerMask = LayerMask.GetMask("Terrain");
        if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this + " in scene!"); }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        particleController = GameObject.Find("MouseParticleController").GetComponent<MouseParticleController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentUnit is null && !(UnitToSelect is null))
        {
            currentUnit = UnitToSelect;
            UnitToSelect = null;
        }
        else if (!(currentUnit is null) && Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); 
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ~LayerMask.GetMask("Ignore Raycast")) && (moveLayerMask == (moveLayerMask | (1 << hit.collider.gameObject.layer))))
            { 
                Debug.Log("hit");
                currentUnit?.QueueMoveTo(hit.point);
                particleController.DeactivateDisplay();
                currentUnit._particleSystem.Stop();
                currentUnit = null;
            }
        }
    }

    public void RegisterUnit( PlayerUnit unit )
    {
        this.units.Add(unit);
    }

    public void UnregisterUnit( PlayerUnit unit )
    {
        this.units.Remove(unit);
        if(units.Count == 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void UnitClicked( PlayerUnit unit )
    {
        if(currentUnit is null)
        {
            Debug.Log(string.Format("Unit {0} Set to active", unit.UnitID));
            particleController.ActivateDisplay(unit.accuracyDiameter, unit.transform.position, unit.distanceInaccuracy);
            unit._particleSystem.Play();
            currentUnit = unit;
        }
        else if(currentUnit == unit)
        {
            particleController.DeactivateDisplay();
            unit._particleSystem.Stop();
            currentUnit = null;
        }
    }

    
}
