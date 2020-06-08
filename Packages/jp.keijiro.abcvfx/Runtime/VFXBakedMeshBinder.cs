using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace Abcvfx {

//
// VFX property binder for MeshBaker
//
[AddComponentMenu("VFX/Property Binders/Abcvfx/Baked Mesh Binder")]
[VFXBinder("Abcvfx/Baked Mesh")]
sealed class VFXBakedMeshBinder : VFXBinderBase
{
    public string PositionMapProperty {
        get => (string)_positionMapProperty;
        set => _positionMapProperty = value;
    }

    public string NormalMapProperty {
        get => (string)_normalMapProperty;
        set => _normalMapProperty = value;
    }

    public string ColorMapProperty {
        get => (string)_colorMapProperty;
        set => _colorMapProperty = value;
    }

    [VFXPropertyBinding("UnityEngine.Texture2D"), SerializeField]
    ExposedProperty _positionMapProperty = "PositionMap";

    [VFXPropertyBinding("UnityEngine.Texture2D"), SerializeField]
    ExposedProperty _normalMapProperty = "NormalMap";

    [VFXPropertyBinding("UnityEngine.Texture2D"), SerializeField]
    ExposedProperty _colorMapProperty = "ColorMap";

    public MeshBaker Target = null;

    public override bool IsValid(VisualEffect component)
      => Target != null &&
       component.HasTexture(_positionMapProperty) &&
       component.HasTexture(_normalMapProperty) &&
       component.HasTexture(_colorMapProperty);

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetTexture(_positionMapProperty, Target.PositionMap);
        component.SetTexture(_normalMapProperty, Target.NormalMap);
        component.SetTexture(_colorMapProperty, Target.ColorMap);
    }

    public override string ToString()
      => $"Baked Mesh : '{_positionMapProperty}' -> {Target?.name ?? "(null)"}";
}

}
