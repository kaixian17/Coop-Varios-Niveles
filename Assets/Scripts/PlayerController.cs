using Unity.Netcode;
using UnityEngine;
public class PlayerController : NetworkBehaviour
{
    public float speed = 5f;
    
    void Update()
    {
        if (IsOwner)
        {
            Vector3 newPosition = transform.localPosition;
            if (Input.GetKey(KeyCode.W))
            {
                newPosition.z += speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                newPosition.z -= speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                newPosition.x -= speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                newPosition.x += speed * Time.deltaTime;
            }

            if (newPosition != transform.localPosition)
            {
                transform.localPosition = newPosition;
            }

        }
    }

}