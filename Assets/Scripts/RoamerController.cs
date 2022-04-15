using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoamerController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource walk;
    public Rigidbody rb;
    public float moveSpeed = 3f;
    PhotonView pv;
    public Vector3[] points;
    public int curPoint = 0;
    bool canMove = true;
    Vector3 forward, right;

    void Start()
    {
        forward = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.forward;
        right = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.right;
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 dir = points[curPoint] - transform.position;
        Vector3 heading = Vector3.Normalize(dir);
        if (canMove) {
            transform.forward = heading;
            transform.position += heading * moveSpeed * Time.deltaTime;
        }
        if (dir.magnitude < 0.1f) {
            curPoint++;
            if (curPoint >= points.Length) {
                curPoint = 0;
            }
        }

        StartCoroutine("Travel");

    }

    IEnumerator Travel()
    {
        canMove = false;
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

    void PlayWalkSound() {
            walk.Play();
    }
}
