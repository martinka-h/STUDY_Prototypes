using UnityEngine;

[RequireComponent (typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlaceableObject : MonoBehaviour
{
    protected bool isIntersecting;
    private Material ghostRed;
    private Material ghostGreen;
    public LayerMask requiredLayer;
    private MeshRenderer objRenderer;

    private bool positionIsValid;
    private bool newPositionIsValid;

    private void Awake()
    {
        objRenderer = this.gameObject.GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        ghostGreen = GameAssets.Instance.ghostGreen;
        ghostRed = GameAssets.Instance.ghostRed;
    }

    private void Update()
    {
        if (positionIsValid != newPositionIsValid) {
            positionIsValid = newPositionIsValid;
            objRenderer.material = positionIsValid ? ghostGreen : ghostRed;
        }
    }

    public void OnSelected()
    {
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().useGravity = false;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void OnTriggerEnter(Collider other)
    {
        print(gameObject.name);
        isIntersecting = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isIntersecting = false;
    }

    public bool CheckIfPositionIsValid(LayerMask layer)
    {
        if (!isIntersecting && requiredLayer == (requiredLayer | (1 << layer))) {
            newPositionIsValid = true;
            return true;     
        }
        newPositionIsValid = false;
        return false;
    }
}
