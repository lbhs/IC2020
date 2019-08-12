using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bond : MonoBehaviour
{
    public charger from;
    public charger to;

    public Color positive;
    public Color negative;

    private MeshRenderer meshRenderer;
    public float ForceMagnitude;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CenterOnChargers();
    }

    void CenterOnChargers()
    {
        Vector3 fromPosition = from.gameObject.transform.position;

        // center between the charger instances
        Vector3 position = Vector3.Lerp(fromPosition, to.gameObject.transform.position, 0.5f);
        float dist = Vector3.Distance(fromPosition, position);

        transform.position = position;
        transform.localScale = new Vector3(dist * 2, dist * 2, 1);
    }

    public void UpdateOnForce(charger a, charger b, float force)
    {
        from = a;
        to = b;

        CenterOnChargers();
        UpdateMaterialAlpha(force);
    }

    void UpdateMaterialAlpha(float force)
    {
        if (!meshRenderer)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        if (meshRenderer)
        {
            Color color = force > 0 ? positive : negative;
            color.a = Math.Abs(force) / ForceMagnitude;
            meshRenderer.material.color = color;
            meshRenderer.material.SetColor("_EmissionColor", color);
        }
    }
}