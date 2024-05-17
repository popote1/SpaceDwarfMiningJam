Shader "Custom/DisableZWrite"
{
    subshader {
        Tags{
                "RenderType" = "Opaque"
        }
        
        pass{
            Zwrite Off
        }
    }
}
