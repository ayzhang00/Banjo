using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 6f;
    float jumpSpeed = 5f;
    float camSpeed = 3f;
    float maxCamDist = 1f;
    public Rigidbody rb;
    bool canJump = false;
    public GameObject attack;
    public GameObject flash;
    public GameObject deathEffect;
    public float health = 5f;
    public bool isPlayer1 = true;
    bool playing = true;
    Vector3 camOffset = new Vector3(-15f, 12f, -15f);

    string attack_btn;
    string vertical;
    string horizontal;
    string jump;

    Vector3 forward, right;
    // Start is called before the first frame update
    void Start()
    {
        if (isPlayer1) {
            attack_btn = "FirePlayer1";
            horizontal = "HorizontalKey";
            vertical = "VerticalKey";
            jump = "Jump";
        } else {
            attack_btn = "FirePlayer2";
            horizontal = "HorizontalKey2";
            vertical = "VerticalKey2";
            jump = "Jump2";
        }
        //Changed this because the camera's forward is pointing slightly down
        // forward = Camera.main.transform.forward;
        // forward.y = 0f;
        // // make sure length is one
        // forward = Vector3.Normalize(forward);
        forward = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.forward;
        right = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.right;
        Camera.main.transform.position = transform.position + camOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(jump)) {
            Jump();
        }
        if (Input.GetAxisRaw(horizontal) != 0 || Input.GetAxisRaw(vertical) != 0)
        {
            Move();
        }
        if (Input.GetButtonDown(attack_btn)) {
            Attack();
        }
        if (moveSpeed != 0) {
            MoveCamera();
        }
    }

    void Move()
    {
        Vector3 direction = Vector3.Normalize(new Vector3(Input.GetAxis(horizontal), 0, Input.GetAxis(vertical)));
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * direction.x;
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * direction.z;

        // movement heading
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
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
    
    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Attack") {
            Spark();
            health--;
            if (health <= 0) {
                StartCoroutine(Death());
            }
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collision.collider.tag == "Ground") {
            canJump = false;
        }
    }

    void MoveCamera() {
        Vector3 start = Camera.main.transform.position;
        Vector3 dest = transform.position + camOffset;
        Vector3 dir = start - dest;

        float dist = dir.magnitude;
        float step = (dist / maxCamDist) * camSpeed;

        Camera.main.transform.position -= dir.normalized * step * Time.deltaTime;
    }
    
    void Attack() {
        attack.SetActive(true);
    }

    void Spark() {
        flash.SetActive(true);
    }

    IEnumerator Death() {
        playing = false;
        deathEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        // gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
