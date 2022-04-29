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
    public bool playing = true;
    float attackInterval = 1.5f;
    bool canAttack = true;
    public GameObject player;
    public bool isHit = false;
    public bool isMove = false;
    public bool isAttacking = false;
    bool canMove = true;
    bool canTurn = true;
    // sounds
    public AudioSource swing;
    public AudioSource hit;
    public AudioSource walk;

    Vector3 forward, right, nextPoint;

    PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        player = null;
        //Changed this because the camera's forward is pointing slightly down
        forward = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.forward;
        right = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.right;

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
            if (!player.GetComponent<CharController>().isDead) {
                pv.RPC("SwitchActiveObject", RpcTarget.All, "Move", true);
                Vector3 dir = player.transform.position - transform.position;
                Vector3 heading = Vector3.Normalize(dir);
                // if (canMove) {
                    // transform.forward = heading;
                    // transform.position += heading * moveSpeed * Time.deltaTime;
                // }
                
                if (dir.magnitude < 1 && !isHit) {
                    // canMove = false;
                    if (canAttack) {
                        // Debug.Log("attack");
                        Attack();
                        canAttack = false;
                        StartCoroutine(AttackOnInterval());
                    }
                } else {
                    transform.forward = heading;
                    transform.position += heading * moveSpeed * Time.deltaTime;
                }
            }
        } else {
            // float wait = Random.Range(1.0f, 3.0f);
            // Turn();
            if (canTurn) {
                pv.RPC("SwitchActiveObject", RpcTarget.All, "Move", false);
                float angle = Random.Range(0.0f, 360.0f);
                // transform.Rotate(0.0f, angle, 0.0f);
                Turn(angle);
                StartCoroutine(Pause());
            } else if (canMove) {
                Vector3 dir = nextPoint - transform.position;
                Vector3 heading = Vector3.Normalize(dir);
                transform.forward = heading;
                transform.position += heading * moveSpeed * Time.deltaTime;
                if (dir.magnitude < 0.2f) {
                    canTurn = true;
                    canMove = false;
                    // StartCoroutine(Pause());
                }
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
        if (collider.tag == "PlayerAttack") {
            Spark();
            hit.Play();
            health--;
            pv.RPC("SwitchActiveObject", RpcTarget.All, "Hit", true);
            if (health <= 0) {
                playing = false;
                StartCoroutine(Death());
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "PlayerAttack") {
            pv.RPC("SwitchActiveObject", RpcTarget.All, "Hit", false);
        }
    }


    void Attack() {
        pv.RPC("SwitchActiveObject", RpcTarget.All, "Attack", true);
    }

    void Spark() {
        pv.RPC("SwitchActiveObject", RpcTarget.All, "Flash", true);
    }

    void PlayWalkSound() {
        walk.Play();
    }

    void PlaySwingSound() {
        swing.Play();
    }

    IEnumerator AttackOnInterval() {
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
    }

    IEnumerator Pause() {
        canTurn = false;
        float wait = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(wait);
        // canTurn = true;
        canMove = true;
        nextPoint = transform.forward * Random.Range(10.0f, 20.0f) - transform.position;
        // pv.RPC("Turn", RpcTarget.All, Random.Range(45.0f, 360.0f));
    }

    IEnumerator Death() {
        // deathEffect.SetActive(true);
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
            isAttacking = isActive;
        }
        else if (obj == "Flash") {
            flash.SetActive(isActive);
        }
        else if (obj == "Move") {
            isMove = isActive;
        }
        else if (obj == "Hit") {
            isHit = isActive;
        }
    }

    [PunRPC]
    void Turn(float angle) {
        // while (transform.rotation.eulerAngles.y != angle) {
        //     transform.Rotate(0.0f, 0.1f, 0.0f);
        // }
        transform.Rotate(0.0f, angle, 0.0f);
    }
}