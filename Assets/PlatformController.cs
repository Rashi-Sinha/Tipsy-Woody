using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float rotationRate = 10.0f;
    public Vector3 rotationBounds = new Vector3(15.0f, 0.0f, 15.0f);

    private GameObject player;
    private List<GameObject> _objectsInContact;

    void Start()
    {
        _objectsInContact = new List<GameObject>();
    }

    void FixedUpdate()
    {
        Vector3 totalRotation = Vector3.zero;
        Vector3 platformCentre = GetComponent<Renderer>().bounds.center;
        foreach (GameObject obj in _objectsInContact)
        {
            float mass = 1.0f;
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb)
            {
                mass = rb.mass;
            }
            
            Vector3 objPos = obj.transform.position;
            Vector3 rotToApply = (platformCentre - objPos) * mass;
            totalRotation += new Vector3(-rotToApply.z, 0.0f, rotToApply.x);
        }

        totalRotation *= rotationRate * Time.fixedDeltaTime;
        transform.Rotate(totalRotation);
        transform.rotation = ClampRotation(transform.rotation, rotationBounds);
    }

    public void AddToObjectsInContact(GameObject obj)
    {
        if (!_objectsInContact.Contains(obj))
        {
            Debug.Log("Added object: " + obj.name);
            _objectsInContact.Add(obj);
        }
    }

    public void RemoveFromObjectsInContact(GameObject obj)
    {
        if (_objectsInContact.Contains(obj))
        {
            Debug.Log("Removed object: " + obj.name);
            _objectsInContact.Remove(obj);
        }
    }
    
    private Quaternion ClampRotation(Quaternion q, Vector3 bounds)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
 
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -bounds.x, bounds.x);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
 
        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
        angleY = Mathf.Clamp(angleY, -bounds.y, bounds.y);
        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);
 
        float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
        angleZ = Mathf.Clamp(angleZ, -bounds.z, bounds.z);
        q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);
 
        return q;
    }
}
