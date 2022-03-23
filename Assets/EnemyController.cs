using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyController : MonoBehaviourPun
{
    [SerializeField]
    float moveSpeed = 5f;
    float jumpSpeed = 10f;
    bool canJump = false;
    public Rigidbody rb;
    public GameObject attack;
    public GameObject flash;
    public GameObject deathEffect;
    public float health = 5f;
    bool playing = true;
    float attackInterval = 0.7f;
    bool canAttack = true;
    public GameObject player;
    Vector3 camOffset = new Vector3(-15f, 12f, -15f);

    Vector3 forward, right;

    PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        //Changed this because the camera's forward is pointing slightly down
        // forward = Camera.main.transform.forward;
        // forward.y = 0f;
        // // make sure length is one
        // forward = Vector3.Normalize(forward);
        forward = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.forward;
        right = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.right;
        Camera.main.transform.position = transform.position + camOffset;

        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing && pv.IsMine) {
            Move();
        }
    }

    void Move()
    {
        if (player) {
            Vector3 dir = player.transform.position - transform.position;
            Vector3 heading = Vector3.Normalize(dir);
            transform.forward = heading;
            transform.position += heading * moveSpeed * Time.deltaTime;
            
            if (dir.magnitude < 5 && canAttack) {
                Attack();
                canAttack = false;
                StartCoroutine(AttackOnInterval());
            }
        }
    }

    void Jump() {
        if (canJump) {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Ground") {
            canJump = true;
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collision.collider.tag == "Ground") {
            canJump = false;
        }
    }
    
    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Attack") {
            Spark();
            health--;
            if (health <= 0) {
                StartCoroutine(Death());
            }
        }
    }


    void Attack() {
        pv.RPC("SwitchActiveObject", RpcTarget.All, "Attack", true);
    }

    void Spark() {
        pv.RPC("SwitchActiveObject", RpcTarget.All, "Flash", true);
    }

    IEnumerator AttackOnInterval() {
        yield return new WaitForSeconds(0.7f);
        canAttack = true;
    }

    IEnumerator Death() {
        playing = false;
        deathEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        // gameObject.SetActive(false);
        if (pv.IsMine) {
            PhotonNetwork.Destroy(pv);
        }
    }

    [PunRPC]
    void SwitchActiveObject(string obj, bool isActive) {
        if (obj == "Attack") {
            attack.SetActive(isActive);
        }
        else if (obj == "Flash") {
            flash.SetActive(isActive);
        }
    }
}