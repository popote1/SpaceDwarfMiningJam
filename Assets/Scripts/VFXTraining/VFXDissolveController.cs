using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VFXDissolveController : MonoBehaviour
{
    public SkinnedMeshRenderer _mesh1;
    public SkinnedMeshRenderer _mesh2;
    [SerializeField] private VisualEffect _vfxDissovleEffect;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _dissolveRate = 0.0125f;
    [SerializeField] private float _refreshRate = 0.025f;
    [SerializeField] private AnimationCurve _sparkOverLifeTiume;
    [SerializeField] private float _sparksMax;

    private Material[] _materials;
    void Start() {
        _materials = new Material[2];
        _materials[0] = _mesh1.material;
        _materials[1] = _mesh2.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo()
    {
        if( _animator!=null) _animator.SetBool("Die", true);
        if(_vfxDissovleEffect !=null) _vfxDissovleEffect.Play();

        float sparkRate = 0;
        float counter = 0;
        while (_materials[0].GetFloat("_DissovleAmout") < 1) {
            counter += _dissolveRate;

            sparkRate = _sparkOverLifeTiume.Evaluate(_materials[0].GetFloat("_DissovleAmout") / 1) * _sparksMax;

            _vfxDissovleEffect.SetFloat("SparksRate", sparkRate);
            for (int i = 0; i < _materials.Length; i++) {
                _materials[i].SetFloat("_DissovleAmout" , counter);
            }

            yield return new WaitForSeconds(_refreshRate);
        }
        
    }
}
