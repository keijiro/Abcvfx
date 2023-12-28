using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace Abcvfx {

[AddComponentMenu("VFX/Property Binders/Abcvfx/Mesh Filter Binder")]
[VFXBinder("Abcvfx/Mesh Filter")]
public sealed class VFXMeshFilterBinder : VFXBinderBase
{
    public string MeshProperty
      { get => (string)_meshProperty;
        set => _meshProperty = value; }

    [VFXPropertyBinding("UnityEngine.Mesh"), SerializeField]
    ExposedProperty _meshProperty = "Mesh";

    public MeshFilter Target = null;

    public override bool IsValid(VisualEffect component)
      => Target != null && component.HasMesh(_meshProperty);

    public override void UpdateBinding(VisualEffect component)
      => component.SetMesh(_meshProperty, Target.sharedMesh);

    public override string ToString()
      => $"MeshFilter : '{_meshProperty}' -> {Target?.name ?? "(null)"}";
}

} // namespace Abcvfx
