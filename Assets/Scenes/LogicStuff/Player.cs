using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject capsulePrefab;

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 pressPos = Mouse.current.position.ReadValue();

            Ray origin = Camera.main.ScreenPointToRay(pressPos);
            RaycastHit hitInfo;

            if (Physics.Raycast(origin, out hitInfo))
            {
                Instantiate(capsulePrefab, hitInfo.point, Quaternion.identity);
            }
        }
    }
}
