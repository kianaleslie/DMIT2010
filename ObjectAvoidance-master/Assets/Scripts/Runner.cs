using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3.0f;
    [SerializeField] float forwardDistance = 1.0f;
    [SerializeField] float sideDistance = 3.0f;
    [SerializeField] float detectionRadius = 5.0f;
    [SerializeField] GameObject target;
    [SerializeField] GameObject runnerPrefab;
    [SerializeField] GameObject disguisePrefab;
    //[SerializeField] Vector3 scaleRunnerPrefab = new Vector3(0.01f, 0.01f, 0.01f);
    //[SerializeField] Vector3 runnerScaleBack = new Vector3(1.0f, 1.0f, 1.0f);
    //[SerializeField] Transform runnerT;

    RaycastHit hit;
    bool isLeft;
    bool isRight;
    //bool isDisguised = false;

    void Start()
    {
        //runnerT = runnerPrefab.transform;
    }
    void Update()
    {
        CheckTargets();
        AvoidWalls();
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hunter"))
        {
            if (HasLineOfSight(other.transform))
            {
                //runner detected a hunter in line of sight, move away from it
                Vector3 runDirection = transform.position - other.transform.position;
                runDirection.y = 0;
                transform.rotation = Quaternion.LookRotation(runDirection.normalized);
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SpeedBoost"))
        {
            if (HasLineOfSightToSpeedBoost(collision.gameObject.transform))
            {
                Vector3 direction = collision.gameObject.transform.position - transform.position;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction.normalized);
            }
            ApplySpeedBoost();
            collision.gameObject.SetActive(false);
            StartCoroutine(RemoveSpeedBoost(4.0f));
        }
        //else
        //     if (collision.gameObject.CompareTag("Disguise") && !isDisguised)
        //{
        //    if (HasLineOfSight(collision.gameObject.transform))
        //    {
        //        //move towards the disguise object
        //        Vector3 direction = collision.gameObject.transform.position - transform.position;
        //        direction.y = 0;
        //        transform.rotation = Quaternion.LookRotation(direction.normalized);
        //    }
        //    if (disguisePrefab != null)
        //    {
        //        //get position of the target (runner)
        //        Vector3 targetPosition = runnerPrefab.transform.position;

        //instantiate at target position
        //runnerT.localScale = scaleRunnerPrefab;
        //Instantiate(disguisePrefab, targetPosition, Quaternion.identity);
        //isDisguised = true;
        //collision.gameObject.SetActive(false);
        //StartCoroutine(RevertDisguise(5.0f));
        //}
        //runnerPrefab.SetActive(false);
        //disguisePrefab.SetActive(true);
        //isDisguised = true;
        //collision.gameObject.SetActive(false);
        //StartCoroutine(RevertDisguise(5.0f));
        //}
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
                    if (hit.transform.tag != "Runner" && hit.transform.tag != "Wall")
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
    void ApplySpeedBoost()
    {
        movementSpeed += 5.0f;
    }
    IEnumerator RemoveSpeedBoost(float timeToHaveBoost)
    {
        yield return new WaitForSeconds(timeToHaveBoost);
        movementSpeed -= 5.0f;
    }
    bool HasLineOfSight(Transform target)
    {
        RaycastHit hit;
        Vector3 direction = target.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit, detectionRadius))
        {
            if (hit.transform.CompareTag("Hunter"))
            {
                return true;
            }
        }
        return false;
    }
    bool HasLineOfSightToSpeedBoost(Transform target)
    {
        RaycastHit hit;
        Vector3 direction = target.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit, detectionRadius))
        {
            if (hit.transform.CompareTag("SpeedBoost"))
            {
                return true;
            }
        }
        return false;
    }
    //IEnumerator RevertDisguise(float duration)
    //{
    //    yield return new WaitForSeconds(duration);

    //    disguisePrefab.SetActive(false);
    //    runnerT.localScale = runnerScaleBack;
    //    isDisguised = false;
    //}
}