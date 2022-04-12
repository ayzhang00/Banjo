using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragClone : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("BeginDrag");
    }

    public void OnDrag(PointerEventData eventData) {
        Debug.Log("Dragging");
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("EndDrag");
    }
}
