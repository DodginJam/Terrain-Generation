using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayWarning : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField]
    public GameObject WarningObject
    {  get; private set; }

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (WarningObject != null)
        {
            WarningObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (WarningObject != null)
        {
            WarningObject.SetActive(false);
        }
    }
}
