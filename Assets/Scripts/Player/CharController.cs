using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CharController : MonoBehaviourPun
{
    [SerializeField]
    // basic set up
    float camSpeed = 3f;
    float maxCamDist = 1f;
    public Rigidbody rb;
    public bool playing = true;
    public bool isDead = false;
    public bool obscured = false;
    bool allLEDsOff = false;
    GameObject[] LEDs;
    Vector3 camOffset = new Vector3(-15f, 12f, -15f);
    Vector3 forward, right;
    PhotonView pv;
    // sounds
    public AudioSource swing;
    public AudioSource hit;
    public AudioSource walk;
    public AudioClip walk1;
    public AudioClip walk2;
    public AudioClip walk3;
    public AudioClip walk4;
    public AudioSource solderSound;
    public AudioSource chargeSound;
    public AudioSource bg;
    public AudioClip bgtrack1;
    public AudioClip bgtrack2;
    public AudioClip coreTrack1;
    public AudioClip coreTrack2;
    // movement
    public float moveSpeed = 3f;
    float jumpSpeed = 4f;
    bool canJump = false;
    public bool isMoving = false;
    // attack
    public GameObject attack;
    public GameObject attackSparks;
    public GameObject deathEffect;
    public GameObject sphere;
    public float health = 5f;
    float maxHealth;
    public bool isAttacking = false;
    public bool isHit = false;
    float timeToAttack = 0.3f;
    float timeAttacked = 0f;
    int attackCount = 0;
    // solder
    CharSolder s;
    // energy
    CharEnergy e;
    // ui
    GameObject ui;
    // revive
    public bool isRevived = false;
    public Image healthBarFill;
    

    // public bool runToTheCoreMusic = false;
    bool isPlayingCoreMusic = false;


    // Start is called before the first frame update
    void Start()
    {
        forward = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.forward;
        right = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.right;
        Camera.main.transform.position = transform.position + camOffset;

        s = GetComponent<CharSolder>();
        e = GetComponent<CharEnergy>();
        pv = GetComponent<PhotonView>();
        ui = transform.Find("PlayerUI").gameObject;
        LEDs = GameObject.FindGameObjectsWithTag("LED");
        StartCoroutine("PlayBackgroundMusic");
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playing = !playing;
        }
        s.HandleSolderUI();
        // isMoving = false;
        if (!isDead && playing && pv.IsMine) {
            ui.SetActive(true);
            obscured = false;
            allLEDsOff = true;
            healthBarFill.fillAmount = health / maxHealth;

            if (Input.GetButtonDown("Jump")){
                s.Solder(false);
                solderSound.Stop();
                Jump();
            } 
            // move
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
                isMoving = true;
                Move();
                s.Solder(false);
                s.solderComplete = false;
            } else {
                isMoving = false;
            }

            // if (Input.GetButtonDown("Fire3")) {
            //     if (!isPlayingCoreMusic) {
            //         bg.Stop();
            //         StopCoroutine("PlayBackgroundMusic");
            //         StartCoroutine("PlayRunToTheCoreMusic");
            //         isPlayingCoreMusic = true;
            //     } else {
            //         bg.Stop();
            //         StopCoroutine("PlayRunToTheCoreMusic");
            //         StartCoroutine("PlayBackgroundMusic");
            //         isPlayingCoreMusic = false;

            //     }
            // }

            if (moveSpeed != 0) MoveCamera();
            
            // can only attack and solder when have energy
            if (e.energy > 0) {
                // attack
                if (!isAttacking && Input.GetButtonDown("Fire") && !isHit) {
                    isAttacking = true;
                    Attack(true);
                    s.Solder(false);
                    s.solderComplete = false;
                }
                // stop attacking after some time
                if (isAttacking) {
                    timeAttacked += Time.deltaTime;
                    if (timeAttacked >= timeToAttack) {
                        Attack(false);
                        isAttacking = false;
                        timeAttacked = 0f;
                        attackCount++;
                        if (attackCount == 5) {
                            e.DecEnergy();
                            attackCount = 0;
                        }
                    }
                }
                s.StartSolder();
            }
            
            foreach(GameObject LED in LEDs) {
                // if (Vector3.Distance(transform.position, LED.transform.position) < 20) {
                if (Vector3.Distance(transform.position, LED.transform.position) < 20 && 
                    !LED.activeSelf) {
                    obscured = true;
                }
                if (LED.activeSelf) {
                    allLEDsOff = false;
                }
            }

            if (allLEDsOff) {
                bg.Stop();
                StopCoroutine("PlayBackgroundMusic");
                StartCoroutine("PlayRunToTheCoreMusic");
            }

            Obscure(obscured);
        }
    }

    void Move()
    {
        Vector3 direction = Vector3.Normalize(new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey")));
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

    // collisions and triggers
    void OnCollisionStay(Collision collision) {
        if (collision.collider.tag == "Ground" || collision.collider.tag == "Switch") {
            canJump = true;
        }
    }
    
    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Attack") {
            isHit = true;
            Spark();
            hit.Play();
            health--;
            if (health <= 0) {
                healthBarFill.fillAmount = 0;
                isDead = true;
                if (isRevived) {
                    StartCoroutine(Death());
                }
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "Attack") {
            isHit = false;
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collision.collider.tag == "Ground" || collision.collider.tag == "Switch") {
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
    
    void Attack(bool isActive) {
        pv.RPC("SwitchActiveObject", RpcTarget.All, "Attack", isActive);
    }

    void Spark() {
        pv.RPC("SwitchActiveObject", RpcTarget.All, "attackSparks", true);
    }

    void Obscure(bool isActive) {
        pv.RPC("SwitchActiveObject", RpcTarget.All, "Sphere", isActive);
    }

    void PlayWalkSound() {
        if (canJump) {
            int clip = Random.Range(0, 3);
            switch (clip) {
                case 0:
                    walk.clip = walk1;
                    break;
                case 1:
                    walk.clip = walk2;
                    break;
                case 2:
                    walk.clip = walk3;
                    break;
                case 3:
                    walk.clip = walk4;
                    break;
            }
            walk.Play();
        }
    }

    void PlaySwingSound() {
        swing.Play();
    }

    public void ContinuePressed() {
        playing = true;
    }

    IEnumerator PlayBackgroundMusic() {
        bg.clip = bgtrack1;
        bg.Play();
        yield return new WaitForSeconds(bg.clip.length);
        bg.clip = bgtrack2;
        bg.Play();
    }

    IEnumerator PlayRunToTheCoreMusic() {
        bg.clip = coreTrack1;
        bg.Play();
        yield return new WaitForSeconds(bg.clip.length);
        bg.clip = coreTrack2;
        bg.Play();
    }

    IEnumerator Death() {
        isDead = true;
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
        else if (obj == "attackSparks") {
            attackSparks.SetActive(isActive);
        }
        else if (obj == "Solder") {
            s.solder.SetActive(isActive);
            s.isSoldering = isActive;
            if (!isActive) {
                s.timeSoldered = 0f;
            }
        }
        else if (obj == "Sphere") {
            sphere.SetActive(!isActive);
        }
    }
}
