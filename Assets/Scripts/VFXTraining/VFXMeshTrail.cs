using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXMeshTrail : MonoBehaviour
{

    [SerializeField] private VFXDay9sequencer _vfxDay9Sequencer;
    [SerializeField] private float _activeTime = 2f;

    [SerializeField, Header("Mesh RefreshRate")] private float _meshRefresh = 0.1f;
    
    [SerializeField] private float _meshDestroyDelay=3f;
    [Header("Shader Related")]
    [SerializeField] private Material _mat;

    [SerializeField] private string _shaderVarRef;
    [SerializeField] private float _shaderVarRate = 0.1f;
    [SerializeField] private float _shaderVarRefreshRate = 0.05f;
    private bool _isTrailActive;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isTrailActive = true;
            _vfxDay9Sequencer.StartAnimation();
            StartCoroutine(ActivateTrail(_activeTime));

        }
    }

    IEnumerator ActivateTrail(float timeActive) {
        
        if (_skinnedMeshRenderers == null) {
            _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        }
        while(timeActive>0) {
            timeActive -= _meshRefresh;
            
            for (int i = 0; i < _skinnedMeshRenderers.Length; i++)
            {

                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(_skinnedMeshRenderers[i].transform.position, _skinnedMeshRenderers[i].transform.rotation);
                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();
                Mesh mesh = new Mesh();
                _skinnedMeshRenderers[i].BakeMesh(mesh);
                mf.mesh = mesh;
                mr.material = _mat;

                StartCoroutine(AnimateMaterialFloat(mr.material, 0, _shaderVarRate, _shaderVarRefreshRate));
                
                Destroy(gObj , _meshDestroyDelay);
            }
            
            yield return new WaitForSeconds(_meshRefresh);
        }
        _isTrailActive = false;
    }


    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(_shaderVarRef);

        while (valueToAnimate>goal) {
            valueToAnimate= Mathf.Clamp01(valueToAnimate -= rate);
            mat.SetFloat(_shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}