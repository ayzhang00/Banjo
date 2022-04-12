using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloneCount : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Camera cam;
    public Vector3 offset;
    public int clonesPlaced = 0;
    public Sprite[] cloneArray;
    public GameObject DraggedClone;
    public bool pickedUp = false;
    Image sourceImage;
    private RectTransform dragTransform;
    
    bool clickedOutside = false;

    void Start()
    {
        sourceImage = GetComponent<Image>();
        dragTransform = DraggedClone.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("Clicked");
        // make sure clones placed <= 5
        // if (clonesPlaced < 6 && !pickedUp) {
        //     DraggedClone.SetActive(true);
        //     pickedUp = true;
        //     clickedOutside = false;
        // }

    }
    public void OnPointerUp(PointerEventData eventData) {
        if (clonesPlaced < 6 && !pickedUp) {
            DraggedClone.SetActive(true);
            pickedUp = true;
            clickedOutside = false;
        }
    }
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        if (!pickedUp) {
            DraggedClone.SetActive(false);
        }
        if (DraggedClone.activeSelf) {
            MoveClone();
        }
        SwitchSprite();
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
