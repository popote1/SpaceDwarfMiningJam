using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public class VFXDay23CustomPostProcessRenderFeature : ScriptableRendererFeature
{

    [SerializeField] private Shader m_bloomShader;
    [SerializeField] private Shader m_compositeShader;

     private Material m_bloomMaterial;
     private Material m_compositeMaterial;
    
    private VFXDay23CutomPostProcessPass m_customPass;
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_customPass);
    }
    
    public override void Create()
    {

        m_bloomMaterial = CoreUtils.CreateEngineMaterial(m_bloomShader);
        m_compositeMaterial = CoreUtils.CreateEngineMaterial(m_compositeShader);
        
        m_customPass = new VFXDay23CutomPostProcessPass(m_bloomMaterial, m_compositeMaterial);
        
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            m_customPass.ConfigureInput(ScriptableRenderPassInput.Depth);
            m_customPass.ConfigureInput(ScriptableRenderPassInput.Color);
            m_customPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        }
    }

    protected override void Dispose(bool displosing)
    {
        CoreUtils.Destroy(m_bloomMaterial);
        CoreUtils.Destroy(m_compositeMaterial);
    }
}
