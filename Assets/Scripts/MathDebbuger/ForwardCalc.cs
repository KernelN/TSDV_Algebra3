using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardCalc : MonoBehaviour
{
    public Transform pointer;
    public float pointDis = 1;
    [Header("GIZMOS")] 
    public float rayLength;
    public bool useCalc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!pointer) return;

        Vector3 customForward;
        Vector3 localForw = new Vector3(0,0,1);
        Vector3 worldForw = transform.rotation * localForw;
        customForward = worldForw.normalized;
        
        pointer.position = transform.position + customForward * pointDis;
    }

    void OnDrawGizmos()
    {
        if (!useCalc)
        {
            Gizmos.color = Color.Lerp(Color.blue, Color.white, 0.25f);
            Gizmos.DrawRay(transform.position, transform.forward * rayLength);
            
            Gizmos.color = Color.Lerp(Color.red, Color.white, 0.25f);
            Gizmos.DrawRay(transform.position, transform.right * rayLength);
            
            Gizmos.color = Color.Lerp(Color.green, Color.white, 0.25f);
            Gizmos.DrawRay(transform.position, transform.up * rayLength);
        }
        else
        {
            
            Vector3 rot = transform.rotation.eulerAngles;
            
            Vector3 radians = rot * Mathf.Deg2Rad;
            
            //Calculate forward
            Vector3 forward = Vector3.zero;
            forward.x =  Mathf.Cos(radians.x) * Mathf.Sin(radians.y);
            forward.y = -Mathf.Sin(radians.x);
            forward.z =  Mathf.Cos(radians.x) * Mathf.Cos(radians.y);
            
            Vector3 right = Vector3.zero;
            right.x = Mathf.Sin(radians.y);
            right.y = Mathf.Sin(radians.x) * Mathf.Cos(radians.y);
            right.z = Mathf.Sin(radians.x) * Mathf.Sin(radians.y);
            
            Vector3 up = Vector3.Cross(forward, right);
            
            Gizmos.color = Color.Lerp(Color.blue, Color.black, 0.5f);
            Gizmos.DrawRay(transform.position, forward * rayLength);
            
            Gizmos.color = Color.Lerp(Color.red, Color.black, 0.5f);
            Gizmos.DrawRay(transform.position, right * rayLength);
            
            Gizmos.color = Color.Lerp(Color.green, Color.black, 0.5f);
            Gizmos.DrawRay(transform.position, up * rayLength);
        }
    }
}
