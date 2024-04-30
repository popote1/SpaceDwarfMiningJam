using UnityEngine;

public class AutoDestroy : MonoBehaviour {
    [SerializeField] private float _destroyDelay;

    private void Start() {
        Destroy(gameObject, _destroyDelay);
    }
}