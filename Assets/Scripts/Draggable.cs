using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
    public Transform parentToReturnTo;
    public Transform parent;
    public TextAsset TextFile;
    public bool draggable;

    public void OnBeginDrag(PointerEventData eventData){
        if (parentToReturnTo.name == "Play"){
            eventData.pointerDrag = null;
        }
        else if (!draggable){
            eventData.pointerDrag = null;
        }
        else{
            parent = this.transform.parent;
            this.transform.SetParent(parent.parent);
        }
    }

    public void OnDrag(PointerEventData eventData){
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData){
        this.transform.SetParent(parentToReturnTo);
    }
}
