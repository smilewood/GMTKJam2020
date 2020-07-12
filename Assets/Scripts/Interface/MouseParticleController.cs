using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseParticleController : MonoBehaviour
{
    public Vector3 radiusScale;
    public float radiusRateMultiplier;
    private Camera mainCamera;
    private ParticleSystem mouseParticleSystem;
    private ParticleSystem.ShapeModule particleShape;
    private ParticleSystem.EmissionModule particleEmission;
    bool active;

    private float baseRadius, distanceScale;
    private Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mouseParticleSystem = this.gameObject.GetComponent<ParticleSystem>();
        this.particleShape = mouseParticleSystem.shape;
        this.particleEmission = mouseParticleSystem.emission;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Terrain")))
            {
                this.gameObject.transform.position = hit.point;
            }

            float radius = baseRadius + Vector3.Distance(origin, transform.position) * distanceScale;

            this.particleShape.scale = radius * this.radiusScale;
            this.particleEmission.rateOverTime = radius * this.radiusRateMultiplier;
        }
    }

    public void ActivateDisplay(float baseRadius, Vector3 origin, float distanceScale)
    {
        this.baseRadius = baseRadius;
        this.distanceScale = distanceScale;
        this.origin = origin;
        this.active = true;
        
        this.mouseParticleSystem.Play();
    }

    public void DeactivateDisplay()
    {
        this.active = false;
        this.mouseParticleSystem.Stop();
    }
}
