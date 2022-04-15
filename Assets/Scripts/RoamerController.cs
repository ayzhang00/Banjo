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
    bool obscured = false;
    Vector3 forward, right;
    GameObject[] LEDs;
    public GameObject sphere;

    void Start()
    {
        forward = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.forward;
        right = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.right;
        pv = GetComponent<PhotonView>();
        LEDs = GameObject.FindGameObjectsWithTag("LED");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        foreach(GameObject LED in LEDs) {
            // if (Vector3.Distance(transform.position, LED.transform.position) < 20) {
            if (Vector3.Distance(transform.position, LED.transform.position) < 20 && 
                !LED.activeSelf) {
                obscured = true;
            }
        }
        Obscure(obscured);
    }

    void Move()
    {
        Vector3 dir = points[curPoint] - transform.position;
        Vector3 heading = Vector3.Normalize(dir);
        if (canMove) {
            transform.forward = new Vector3(heading.x, 0.0f, heading.z);
            transform.position += heading * moveSpeed * Time.deltaTime;
            if (dir.magnitude < 0.2f) {
                StartCoroutine(Pause());
                curPoint++;
                if (curPoint >= points.Length) {
                    curPoint = 0;
                }
            }
        }
        // StartCoroutine("Travel");
    }

    void Obscure(bool isActive) {
        pv.RPC("ActiveObject", RpcTarget.All, "Sphere", isActive);
    }

    IEnumerator Pause()
    {
        canMove = false;
        float wait = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(wait);
        canMove = true;
    }

    void PlayWalkSound() {
            walk.Play();
    }

    [PunRPC]
    void ActiveObject(string obj, bool isActive) {
        if (obj == "Sphere") {
            sphere.SetActive(!isActive);
        }
    }
}
