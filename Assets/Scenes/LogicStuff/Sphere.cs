using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 1f))
        {
            Debug.Log("Sphere is above: " + hitInfo.collider.gameObject.name);

            if (hitInfo.collider.gameObject.CompareTag("Untagged"))
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1f);
    }
}
