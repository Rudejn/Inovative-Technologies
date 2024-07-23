using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfClubSwing : MonoBehaviour
{
    public Transform club;
    public Transform ball;
    public float pullBackSpeed = 5f;
    public float swingSpeed = 20f;
    public float maxPullBackDistance = 2f;
    public float hitForceMultiplier = 10f;

    private bool isPullingBack = false;
    private bool isSwinging = false;
    private Vector3 initialClubPosition;
    private Rigidbody ballRigidbody;

    void Start()
    {
        initialClubPosition = club.position;
        ballRigidbody = ball.GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPullingBack = true;
            isSwinging = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPullingBack = false;
            isSwinging = true;
        }

        if (isPullingBack)
        {
            PullBack();
        }
        else if (isSwinging)
        {
            SwingForward();
        }
    }

    void PullBack()
    {
        float pullBackStep = pullBackSpeed * Time.deltaTime;
        if (Vector3.Distance(club.position, initialClubPosition) < maxPullBackDistance)
        {
            club.Translate(-Vector3.forward * pullBackStep);
        }
    }

    void SwingForward()
    {
        float swingStep = swingSpeed * Time.deltaTime;
        if (club.position.z < initialClubPosition.z)
        {
            club.Translate(Vector3.forward * swingStep);
        }
        else
        {
            isSwinging = false;
            club.position = initialClubPosition;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GolfBall") && isSwinging)
        {
            Vector3 direction = (ball.position - club.position).normalized;
            float hitForce = hitForceMultiplier * Vector3.Distance(club.position, initialClubPosition);
            ballRigidbody.AddForce(direction * hitForce, ForceMode.Impulse);
        }
    }
}
