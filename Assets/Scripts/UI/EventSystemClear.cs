using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemClear : MonoBehaviour
{
    private EventSystem eventSystem; 

    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
    }

    public void EventSystemSelect(GameObject selectedGameObject)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedGameObject);
    }
}
