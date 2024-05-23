using UnityEngine;

public class BurningCell : MonoBehaviour
{
    
    
    public bool IsBurning {
        get => _isBurning;
        private set {
            _isBurning = value;
        }
    }
    private bool _isBurning;
    
    private float _burningtime;
    private void Update() {
        ManageBurning();
    }
    
    public void SetBurning(float time) {
        if (_burningtime < time) _burningtime = time;
        IsBurning = true;
        gameObject.SetActive(true);
    }

    private void ManageBurning() {
        if(!IsBurning)return;
        _burningtime -=Time.deltaTime;
        if (_burningtime <= 0) {
            IsBurning = false;
            gameObject.SetActive(false);
            _burningtime = 0;
        }
    }
}