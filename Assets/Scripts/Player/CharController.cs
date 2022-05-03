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
    float timeRunningToCore = 0f;
    public Rigidbody rb;
    public bool playing = true;
    public bool isDead = false;
    public bool obscured = false;
    public bool coreTouched = false;
    bool allPlayersAtCore = false;
    public Vector3 originalPos;
    GameObject[] LEDs;
    public Vector3 camOffset = new Vector3(-15f, 12f, -15f);
    Vector3 forward, right;
    PhotonView pv;
    // movement
    public float moveSpeed = 3f;
    float jumpSpeed = 4f;
    public bool canJump = false;
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
    GameObject attacked;
    // solder
    CharSolder s;
    // energy
    CharEnergy e;
    // revive
    CharRevive r;
    // ui
    GameObject ui;
    PlayerSounds ps;
    GameObject creator;
    GameObject[] players;
    public bool isRevived = false;
    public Image healthBarFill;
    // core cutscene
    public GameObject overlay;
    public GameObject task;
    public GameObject taskNum;
    // Start is called before the first frame update
    void Start()
    {
        forward = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.forward;
        right = Quaternion.Euler(new Vector3(0, 45, 0)) * Vector3.right;
        // Camera.main.transform.position = transform.position + camOffset;

        s = GetComponent<CharSolder>();
        e = GetComponent<CharEnergy>();
        r = GetComponent<CharRevive>();
        ps = GetComponent<PlayerSounds>();
        pv = GetComponent<PhotonView>();
        ui = transform.Find("PlayerUI").gameObject;
        if (pv.IsMine) {
            Camera.main.transform.position = transform.position + camOffset;
            originalPos = transform.position;
        }
        LEDs = GameObject.FindGameObjectsWithTag("LED");
        players = GameObject.FindGameObjectsWithTag("Player");
        maxHealth = health;
        creator = GameObject.Find("Creator");
        if (pv.IsMine) pv.RPC("playerName", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
    }
    [PunRPC]
    void playerName(string name) {
        transform.Find("NameUI").gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = name;
    }

    // Update is called once per frame
    void Update()
    {
        // if (!r.reviving && !e.recharging && s.canSolder) s.HandleSolderUI();
        // isMoving = false;
        if (!isDead && playing && !GetComponent<PauseMenu>().isPaused && pv.IsMine) {
            s.HandleSolderUI();
            playing = !GetComponent<PauseMenu>().isPaused;

            ui.SetActive(true);
            obscured = false;
            healthBarFill.fillAmount = health / maxHealth;

            if (Input.GetButtonDown("Jump")){
                Jump();
            } 

            // can only attack and solder when have energy
            if (e.energy > 0) {
                // attack
                if (!isAttacking && Input.GetButtonDown("Fire") && !isHit) {
                    Attack(true);
                    // s.Solder(false);
                    // s.solderComplete = false;
                }
                // stop attacking after some time
                if (isAttacking) {
                    timeAttacked += Time.deltaTime;
                    if (timeAttacked >= timeToAttack) {
                        Attack(false);
                        timeAttacked = 0f;
                        attackCount++;
                        if (attackCount == 5 && !ps.corePlaying) {
                            e.DecEnergy();
                            attackCount = 0;
                        }
                    }
                }
                s.StartSolder();
            }
            
            foreach(GameObject LED in LEDs) {
                // if (Vector3.Distance(transform.position, LED.transform.position) < 20) {
                // if (LEDZone == LED.tag && 
                if (Vector3.Distance(transform.position, LED.transform.position) < 20 && 
                    !LED.activeSelf) {
                    obscured = true;
                }
            }
            if (!ps.corePlaying) { 
                Obscure(obscured);
            } else {
                if (timeRunningToCore >= 196f) {
                    playing = false;
                    Debug.Log("player LOOOOOSSSEEEEE");
                }
                timeRunningToCore += Time.deltaTime;
            }

            allPlayersAtCore = true;
            // if (coreTouched) {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players) {
                if (!player.GetComponent<CharController>().coreTouched) {
                    allPlayersAtCore = false;
                }
            }
            // }
            if (allPlayersAtCore && ps.corePlaying) {
                playing = false;
                isMoving = false;
                Debug.Log("player WIIIIIIINNNNNNN");
            }
        }
    }

    private void FixedUpdate() {
        if (!isDead && playing && pv.IsMine) {
            // move
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
                pv.RPC("SwitchActiveObject", RpcTarget.All, "Move", true);
                Move();
            } else {
                pv.RPC("SwitchActiveObject", RpcTarget.All, "Move", false);
            }

            if (moveSpeed != 0) MoveCamera();

        }
    }

    void Move()
    {
        // if (pv.IsMine) {
            Vector3 direction = Vector3.Normalize(new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey")));
        
            Vector3 rightMovement = right * moveSpeed * Time.fixedDeltaTime * direction.x;
            Vector3 upMovement = forward * moveSpeed * Time.fixedDeltaTime * direction.z;

            // movement heading
            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
            transform.forward = heading;
            transform.position += rightMovement;
            transform.position += upMovement;
        // }
    }

    void Jump() {
        // if (canJump && pv.IsMine) {
        if (canJump) {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        }
    }

    // collisions and triggers
    void OnCollisionStay(Collision collision) {
        // if (pv.IsMine) {
            if (collision.collider.tag == "Ground" || collision.collider.tag == "Switch" || 
                    collision.collider.tag == "Wire" || collision.collider.tag == "Grass") {
                canJump = true;
            }
            if (collision.collider.tag == "Core") {
                coreTouched = true;
            }
        // }
    }
    
    void OnTriggerEnter(Collider collider) {
        // if (pv.IsMine) {
            if (collider.tag == "Attack") {
                pv.RPC("healthDec", RpcTarget.AllViaServer);
                // health--;
                Spark();
                // hit.Play();\
                Debug.Log(health);
                pv.RPC("SwitchActiveObject", RpcTarget.All, "Hit", true);
                if (health <= 0) {
                    healthBarFill.fillAmount = 0;
                    pv.RPC("SwitchActiveObject", RpcTarget.All, "Hit", false);
                    pv.RPC("SwitchActiveObject", RpcTarget.All, "Dead", true);
                    if (isRevived) {
                        StartCoroutine(Death());
                    }
                }
            }
        // }
    }

    void OnTriggerExit(Collider collider) {
        // if (pv.IsMine) {
            if (collider.tag == "Attack") {
                Debug.Log("left");
                Debug.Log(health);
                pv.RPC("SwitchActiveObject", RpcTarget.All, "Hit", false);
            }
        // }
    }

    void OnCollisionExit(Collision collision) {
        // if (pv.IsMine) {
            if (collision.collider.tag == "Ground" || collision.collider.tag == "Switch" || 
                    collision.collider.tag == "Wire" || collision.collider.tag == "Grass") {
                canJump = false;
            }
        // }
    }

    void MoveCamera() {
        // if (pv.IsMine) {
            Vector3 start = Camera.main.transform.position;
            Vector3 dest = transform.position + camOffset;
            Vector3 dir = start - dest;

            float dist = dir.magnitude;
            float step = (dist / maxCamDist) * camSpeed;

            Camera.main.transform.position -= dir.normalized * step * Time.fixedDeltaTime;
        // }
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


    public void ContinuePressed() {
        playing = true;
    }

    public IEnumerator CoreCutscenePause() {
        overlay.SetActive(true);
        yield return new WaitForSeconds(16f);
        overlay.SetActive(false);
        task.GetComponent<Text>().text = "Get to the core";
        taskNum.SetActive(false);
        playing = true;
    }

    IEnumerator Death() {
        if (pv.IsMine) {
            pv.RPC("SwitchActiveObject", RpcTarget.All, "Dead", true);
            deathEffect.SetActive(true);
            yield return new WaitForSeconds(1f);
            // gameObject.SetActive(false);
            PhotonNetwork.Destroy(pv);
        }
    }

    [PunRPC]
    void healthDec() {
        health--;
    }
    [PunRPC]
    void SwitchActiveObject(string obj, bool isActive) {
        if (obj == "Attack") {
            attack.SetActive(isActive);
            isAttacking = isActive;
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
        else if (obj == "Move") {
            isMoving = isActive;
        }
        else if (obj == "Hit") {
            isHit = isActive;
        }
        else if (obj == "Dead") {
            isDead = isActive;
        }
    }
}
