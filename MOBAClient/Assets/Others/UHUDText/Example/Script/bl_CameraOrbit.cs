using UnityEngine;
using System.Collections;

public class bl_CameraOrbit : MonoBehaviour {

     public Transform target;
     private float distance = 30f;
     private float xSpeed = 20.0f;
     private float ySpeed = 20.0f;

     private float yMinLimit = 50f;
     private float yMaxLimit = 50f;

     private float distanceMin = 40f;
     private float distanceMax = 40f;
 
     public float smoothTime = 2f;
 
     float rotationYAxis = 0.0f;
     float rotationXAxis = 0.0f;
 
     float velocityX = 0.0f;
     float velocityY = 0.0f;
 
     // Use this for initialization
     void Start()
     {
         Vector3 angles = transform.eulerAngles;
         rotationYAxis = angles.y;
         rotationXAxis = angles.x;
 
         // Make the rigid body not change rotation
         if (GetComponent<Rigidbody>())
         {
             GetComponent<Rigidbody>().freezeRotation = true;
         }
     }
 
     void LateUpdate()
     {
         if (target)
         {
             if (Input.GetMouseButton(0))
             {
                 velocityX += xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f;
                 velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
             }
 
             rotationYAxis += velocityX;
             rotationXAxis -= velocityY;
 
             rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
 
             Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
             Quaternion rotation = toRotation;
             
             distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
 
             /*RaycastHit hit;
             if (Physics.Linecast(target.position, transform.position, out hit))
             {
                 distance -= hit.distance;
             }*/
             Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
             Vector3 position = rotation * negDistance + target.position;
             
             transform.rotation = rotation;
             transform.position = position;
 
             velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
             velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
         }
 
     }
 
     public static float ClampAngle(float angle, float min, float max)
     {
         if (angle < -360F)
             angle += 360F;
         if (angle > 360F)
             angle -= 360F;
         return Mathf.Clamp(angle, min, max);
     }
 }

