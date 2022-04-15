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
    bool isHit = false;
    bool canMove = true;
    // sounds
    public AudioSource swing;
    public AudioSource hit;
    public AudioSource walk;

    Vector3 forward, right;

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
        // if (roaming) {
            
        // } else if (player) {
        if (player) {
            if (!player.GetComponent<CharController>().isDead) {
                Vector3 dir = player.transform.position - transform.position;
                Vector3 heading = Vector3.Normalize(dir);
                if (canMove) {
                    transform.forward = heading;
                    transform.position += heading * moveSpeed * Time.deltaTime;
                }
                
                if (dir.magnitude < 1 && !isHit) {
                    canMove = false;
                    if (canAttack) {
                        // Debug.Log("attack");
                        Attack();
                        canAttack = false;
                        StartCoroutine(AttackOnInterval());
                    }
                }
                else {
                    canMove = true;
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
            isHit = true;
            if (health <= 0) {
                playing = false;
                StartCoroutine(Death());
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "PlayerAttack") {
            isHit = false;
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
        }
        else if (obj == "Flash") {
            flash.SetActive(isActive);
        }
    }
}