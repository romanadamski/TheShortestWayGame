using Assets.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMonitor : MonoBehaviour
{
    Transform mainCamTransform; // Stores the FPS camera transform
    private bool visible = true;
    Renderer objRenderer;

    private void Start()
    {
        mainCamTransform = Camera.main.transform;
        objRenderer = gameObject.GetComponent<Renderer>();
    }
    private void Update()
    {
        disappearChecker();
    }
    private void disappearChecker()
    {
        float distance = Vector3.Distance(mainCamTransform.position, transform.position);

        if (distance < DistanceToAppearHelper.DistanceToAppear)
        {
            if (!visible)
            {
                objRenderer.enabled = true;
                visible = true;
            }
        }
        else if (visible)
        {
            objRenderer.enabled = false;
            visible = false;
        }
    }
}
