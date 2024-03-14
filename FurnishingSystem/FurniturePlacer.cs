using UnityEngine;
public class Furnishing : MonoBehaviour {
    [SerializeField] private GameObject furniture;

    private GameObject currentObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) {

            if (currentObject != null) {
                Destroy(currentObject);
            }

            currentObject = Instantiate(furniture);
            currentObject.GetComponent<PlaceableObject>().OnSelected();
            currentObject.GetComponent<PlaceableObject>().enabled = true;
        }

        if (currentObject == null) return;

        MoveObjectToMouse();
    }

    private void MoveObjectToMouse()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f)) {
            currentObject.transform.position = hit.point;

            currentObject.GetComponent<PlaceableObject>().CheckIfPositionIsValid(hit.transform.gameObject.layer);

            if (Input.GetMouseButtonDown(0) && currentObject.GetComponent<PlaceableObject>().CheckIfPositionIsValid(hit.transform.gameObject.layer)) {
                Instantiate(furniture, currentObject.transform.position, Quaternion.identity);
                Destroy(currentObject);
                currentObject = null;
            }
        }
    }
}