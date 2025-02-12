using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float MovementSpeed = 10.0f;
    public float RotationSpeed = 100.0f;
    public GameObject Fireball = null;

    // Update is called once per frame
    void Update()
    {
        // Forward movement
        if (Input.GetAxis("Vertical") != 0)
        {
            Vector3 movement = Quaternion.AngleAxis(-20, Vector3.right) * transform.forward;
            movement *= Input.GetAxis("Vertical") * Time.deltaTime * MovementSpeed;
            GetComponent<Rigidbody>().MovePosition(transform.position + movement);
            GetComponent<Animator>().SetBool("Walking", true);
        }
        else
            GetComponent<Animator>().SetBool("Walking", false);
        // Rotational movement
        float rotation = Input.GetAxis("Horizontal") * Time.deltaTime * RotationSpeed;
        transform.Rotate(0, rotation, 0);
        // Extra gravity when falling
        if (InAir())
            GetComponent<Rigidbody>().AddForce(Physics.gravity * 10);
        // Attack
        if (Input.GetKeyDown("e"))
        {
            GetComponent<Animator>().SetTrigger("Attacking");
            RaycastHit hit;
            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), transform.forward, out hit, 5f))
                Destroy(hit.transform.gameObject);
        }
        // Cast fireball
        if (Input.GetKeyDown("r"))
            Instantiate(Fireball, transform);
    }

    private bool InAir()
    {
        return !Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, 0.1f);
    }
}