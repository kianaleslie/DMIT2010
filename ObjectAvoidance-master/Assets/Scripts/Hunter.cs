using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Hunter : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3.0f;
    [SerializeField] float forwardDistance = 1.0f;
    [SerializeField] float sideDistance = 3.0f;
    [SerializeField] float detectionRadius = 5.0f;
    [SerializeField] GameObject target;

    RaycastHit hit;
    bool isLeft;
    bool isRight;
    Keyboard kb;

    void Start()
    {
        kb = Keyboard.current;
    }
    void Update()
    {
        CheckTargets();
        AvoidWalls();
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        if (kb.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Runner"))
        {
            if (LineOfSight(other.transform))
            {
                Vector3 chaseDirection = other.transform.position - transform.position;
                chaseDirection.y = 0;
                transform.rotation = Quaternion.LookRotation(chaseDirection.normalized);
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SpeedBoost"))
        {
            if (LineOfSight(collision.gameObject.transform))
            {
                Vector3 direction = collision.gameObject.transform.position - transform.position;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction.normalized);
            }
            ApplySpeedBoost();
            collision.gameObject.SetActive(false);
            StartCoroutine(RemoveSpeedBoost(4.0f));
        }
    }
    void AvoidWalls()
    {
        if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out hit, Quaternion.identity, forwardDistance))
        {
            if (hit.transform.gameObject.tag == "Wall")
            {
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -hit.normal, 1, 1));

                //rotate based on what is to the sides
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
                    if (hit.transform.tag != "Hunter" && hit.transform.tag != "Wall")
                    {
                        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 1, 1));
                    }

                    //if (Vector3.Distance(target.transform.position, transform.position) < 2.0f)
                    //{
                    //    target.SetActive(false);
                    //    target = null;
                    //}
                }
            }
            else
            {
                target = null;
            }
        }
    }
    void ApplySpeedBoost()
    {
        movementSpeed += 5.0f;
    }
    IEnumerator RemoveSpeedBoost(float timeToHaveBoost)
    {
        yield return new WaitForSeconds(timeToHaveBoost);
        movementSpeed -= 5.0f;
    }
    bool LineOfSight(Transform target)
    {
        RaycastHit hit;
        Vector3 direction = target.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit, detectionRadius))
        {
            if (hit.transform.CompareTag("Runner"))
            {
                return true;
            }
        }
        return false;
    }
    //bool LineOfSightToSpeedBoost(Transform target)
    //{
    //    RaycastHit hit;
    //    Vector3 direction = target.position - transform.position;

    //    if (Physics.Raycast(transform.position, direction, out hit, detectionRadius))
    //    {
    //        if (hit.transform.CompareTag("SpeedBoost"))
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}