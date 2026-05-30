using UnityEngine;

public class FireAuraRotation : MonoBehaviour
{
    float rotationSpeed = 200f;

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.z += rotationSpeed*Time.deltaTime;
        transform.eulerAngles = rotation;
    }
}
