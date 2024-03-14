using UnityEngine;

public class GameAssets : MonoBehaviour {
    private static GameAssets _Instance;

    public static GameAssets Instance
    {
        get {
            if (_Instance == null) _Instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _Instance;
        }
    }

    [Header("Materials")]
    public Material ghostRed;
    public Material ghostGreen;
}
