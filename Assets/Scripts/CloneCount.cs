using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

public class CloneCount : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Camera cam;
    public Vector3 offset;
    public int clonesPlaced = 0;
    public int maxSpawned = 6;
    public Sprite[] cloneArray;
    public GameObject DraggedClone;
    public bool pickedUp = false;
    Image sourceImage;
    private RectTransform dragTransform;
    
    public AudioClip click;
    public AudioSource CreatorSounds;
    
    SpawnObjectAtClick Spawner;
    float coreTimer = 0f;

    GameObject[] LEDs;
    bool allLEDsOff = false;
    GameObject[] players;
    bool coreSetupDone = false;
    bool allPlayersDead = false;


    void Start()
    {
        sourceImage = GetComponent<Image>();
        dragTransform = DraggedClone.GetComponent<RectTransform>();
        LEDs = GameObject.FindGameObjectsWithTag("LED");
        // GameObject creator = GameObject.FindGameObjectsWithTag("Creator")[0];
        // CreatorSounds = creator.GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("Clicked");
        CreatorSounds.PlayOneShot(click);
        // make sure clones placed <= 5
        // if (clonesPlaced < 6 && !pickedUp) {
        //     DraggedClone.SetActive(true);
        //     pickedUp = true;
        //     clickedOutside = false;
        // }

    }
    public void OnPointerUp(PointerEventData eventData) {
        if (clonesPlaced < maxSpawned && !pickedUp) {
            DraggedClone.SetActive(true);
            pickedUp = true;
        }
    }
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        if(!CreatorSounds) {
            GameObject[] creators = GameObject.FindGameObjectsWithTag("Creator");
            if (creators.Length > 0) {
                GameObject creator = GameObject.FindGameObjectsWithTag("Creator")[0];
                CreatorSounds = creator.GetComponent<AudioSource>();
                Spawner = creator.GetComponent<SpawnObjectAtClick>();
            }
        } else if (Spawner.playing) {
            if (!pickedUp) {
                DraggedClone.SetActive(false);
            }
            if (DraggedClone.activeSelf) {
                MoveClone();
            }
            SwitchSprite();

            players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length != 0) {
                allPlayersDead = true;
                foreach (GameObject player in players) {
                    if (!player.GetComponent<CharController>().isDead) {
                        allPlayersDead = false;
                    }
                }
            }

            if (allPlayersDead) {
                Spawner.playing = false;
                Debug.Log("creator WIIIIINNNNN");
            } else if (coreSetupDone && coreTimer >= 196f){
                Spawner.playing = false;
                Debug.Log("creator LOOOOOOOSSSSSEEEEE");
            }
            if (coreSetupDone) {
                coreTimer += Time.deltaTime;
            }

            allLEDsOff = true;
            // if (!allLEDsOff) {
            foreach(GameObject LED in LEDs) {
                if (LED.activeSelf) {
                    allLEDsOff = false;
                }
            }
            // }

            if (allLEDsOff && !coreSetupDone) {
                maxSpawned = 12;
                clonesPlaced = 0;
                SwitchSprite();

                GameObject[] roamers = GameObject.FindGameObjectsWithTag("Roamer");
                foreach(GameObject roamer in roamers) {
                    PhotonView pv = roamer.GetComponent<PhotonView>();
                    if (pv.IsMine) {
                        PhotonNetwork.Destroy(pv);
                    }
                }
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach(GameObject enemy in enemies) {
                    Debug.Log(enemy.name);
                    PhotonView pv = enemy.GetComponent<PhotonView>();
                    if (pv.IsMine) {
                        PhotonNetwork.Destroy(pv);
                    }
                }
                coreSetupDone = true;
            }
        }
    }

    void MoveClone() {
        Vector3 pos = Input.mousePosition + offset;
        Vector3 newPos =  cam.ScreenToWorldPoint(pos);
        newPos.y = transform.position.y;
        DraggedClone.transform.position = newPos;
    }

    void SwitchSprite() {
        sourceImage.sprite = cloneArray[clonesPlaced];
    }


}
