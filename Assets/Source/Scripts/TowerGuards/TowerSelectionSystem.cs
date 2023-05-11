using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSelectionSystem : MonoBehaviour
{
    public static TowerSelectionSystem Current;

    public UnityEngine.Color SelectedColour;
    public UnityEngine.Color NotSelectedColour;

    public GameObject SelectedTower;

    public GameObject TowerSpotPrefab;
    public GameObject GuardPrefab;
    public LayerMask mask;

    int touchIndex;

    private void Awake()
    {
        Current = this;
        
    }

    //unity code fix from unity forum user https://forum.unity.com/members/adncg.641094/
    //https://forum.unity.com/threads/ispointerovergameobject-is-not-working-on-mobile.798528/
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    void Update()
    {
        if (IsPointerOverUIObject())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),new Vector3(0,0,-1),10, mask,-5,5);

            if (hit.collider != null&& hit.transform.tag=="Tower")
            {
                if (SelectedTower)
                {
                    SelectedTower.GetComponent<SpriteRenderer>().color = NotSelectedColour;
                    if (SelectedTower.tag == "Guard")
                    {
                        SelectedTower.GetComponent<GuardAI>().RangeDisplay(false);
                    }
                }

                hit.transform.GetComponent<SpriteRenderer>().color = SelectedColour;
                SelectedTower = hit.transform.gameObject;
                GuardSelectionMenu.current.SelectEmptyPlot(SelectedTower);

            }
            else if (hit.collider != null && hit.transform.tag == "Guard")
            {
                if (SelectedTower)
                {
                    SelectedTower.GetComponent<SpriteRenderer>().color = NotSelectedColour;
                    if (SelectedTower.tag == "Guard")
                    {
                        SelectedTower.GetComponent<GuardAI>().RangeDisplay(false);
                    }
                }

                hit.transform.GetComponent<SpriteRenderer>().color = SelectedColour;
                SelectedTower = hit.transform.gameObject;

                GuardSelectionMenu.current.SelectExistingGuard(SelectedTower);

            }
            else
            {
                if (hit && hit.transform.gameObject.layer == 5)
                {
                    return;
                }
                if (SelectedTower)
                {
                    if (SelectedTower)
                    {
                        SelectedTower.GetComponent<SpriteRenderer>().color = NotSelectedColour;
                        if (SelectedTower.tag == "Guard")
                        {
                            SelectedTower.GetComponent<GuardAI>().RangeDisplay(false);
                        }
                    }
                    SelectedTower = null;
                    GuardSelectionMenu.current.CloseMenu();
                }
            }
        }
    }

    public void CreateGuard()
    {
        if (SelectedTower)
        {
            Instantiate(GuardPrefab, SelectedTower.transform.position, SelectedTower.transform.rotation, null);

            Destroy(SelectedTower);
        }
    }

    public void DestroyGuard(GameObject guard)
    {
        Instantiate(TowerSpotPrefab, guard.transform.position, guard.transform.rotation, null);
        Destroy(guard);
    }
}
