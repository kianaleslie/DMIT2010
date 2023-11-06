using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float movementSpeed = 3.0f;
    [SerializeField] float forwardDistance = 1.0f;
    [SerializeField] float sideDistance = 3.0f;

    RaycastHit hit;
    bool isLeft;
    bool isRight;

    void Start()
    {

    }
    void Update()
    {
        CheckTargets();

        AvoidWalls();

        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    Rigidbody rb = other.GetComponentInParent<Rigidbody>();

    //    if (rb != null)
    //    {
    //        if (rb.tag == "Collectable")
    //        {
    //            target = other.gameObject;
    //        }
    //    }
    //}
    void AvoidWalls()
    {
        if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out hit, Quaternion.identity, forwardDistance))
        {
            if (hit.transform.gameObject.tag == "Wall")
            {

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -hit.normal, 1, 1));

                // Rotate based on what is to the sides
                isLeft = Physics.Raycast(transform.position, -transform.right, sideDistance);
                isRight = Physics.Raycast(transform.position, transform.right, sideDistance);

                if (isLeft && isRight)
                {
                    transform.Rotate(Vector3.up, 180);
                }
                else if (isLeft && !isRight)
                {
                    transform.Rotate(Vector3.up, 90);
                }
                else if (!isLeft && isRight)
                {
                    transform.Rotate(Vector3.up, -90);
                }
                else
                {
                    if (Random.Range(1, 3) == 1)
                    {
                        transform.Rotate(Vector3.up, 90);
                    }
                    else
                    {
                        transform.Rotate(Vector3.up, -90);
                    }
                }
            }
        }
    }
    void CheckTargets()
    {
        if (target != null)
        {
            if (target.gameObject.activeSelf)
            {
                if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), target.transform.position - transform.position, out hit, Quaternion.identity))
                {
                    if (hit.transform.tag != "Wall")
                    {
                        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 1, 1));
                    }

                    if (Vector3.Distance(target.transform.position, transform.position) < 2.0f)
                    {
                        target.SetActive(false);
                        target = null;
                    }
                }
            }
            else
            {
                target = null;
            }
        }
    }
}