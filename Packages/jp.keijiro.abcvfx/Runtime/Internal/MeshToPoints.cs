using System.Collections.Generic;
using UnityEngine;

namespace Abcvfx {

//
// MeshToPoints: Manages resources required for mesh-to-points conversion.
//
sealed class MeshToPoints
{
    #region Public accessor properties

    public Texture PositionMap => _converted.P;
    public Texture NormalMap   => _converted.N;
    public Texture ColorMap    => _converted.C;

    #endregion

    #region Public methods

    public MeshToPoints(ComputeShader compute)
      => _compute = compute;

    public void ReleaseOnDisable()
      => DeallocateSourceBuffers();

    public void ReleaseOnDestroy()
      => DeallocateConversionRTs();

    public void ProcessMesh(Mesh mesh, Texture texture, int vertexCount)
    {
        // Copy the mesh contents to the temporary lists.
        mesh.GetIndices(_tempList.I, 0);
        mesh.GetVertices(_tempList.P);
        mesh.GetUVs(0, _tempList.T);

        // Check if the source size has been changed.
        if (_source.I != null && !CheckValidSourceBufferSize())
            DeallocateSourceBuffers();

        // Check if the texture size has to be changed.
        if (_converted.P != null && !CheckValidConversionRTSize(vertexCount))
            DeallocateConversionRTs();

        // Buffer (re)allocation
        if (_source.I == null) AllocateSourceBuffers();
        if (_converted.P == null) AllocateConversionRTs(vertexCount);

        // Source buffer update
        _source.I.SetData(_tempList.I);
        _source.P.SetData(_tempList.P);
        _source.T.SetData(_tempList.T);

        // Conversion compute shader dispatching
        _compute.SetInt(IDs.IndexCount, _tempList.I.Count);

        _compute.SetBuffer(0, IDs.Indices, _source.I);
        _compute.SetBuffer(0, IDs.Positions, _source.P);
        _compute.SetBuffer(0, IDs.TexCoords, _source.T);

        _compute.SetTexture(0, IDs.BaseTexture, texture);
        _compute.SetTexture(0, IDs.PositionMap, _converted.P);
        _compute.SetTexture(0, IDs.NormalMap, _converted.N);
        _compute.SetTexture(0, IDs.ColorMap, _converted.C);

        _compute.Dispatch
          (0, _converted.P.width / 8, _converted.P.height / 8, 1);
    }

    #endregion

    #region Private objects

    (List<Vector3> P, List<Vector2> T, List<int> I) _tempList =
      (new List<Vector3>(), new List<Vector2>(), new List<int>());

    ComputeShader _compute;

    #endregion

    #region Source buffers

    // Index, Position, TexCoord
    (ComputeBuffer I, ComputeBuffer P, ComputeBuffer T) _source;

    bool CheckValidSourceBufferSize()
      => _source.I.count == _tempList.I.Count &&
         _source.P.count == _tempList.P.Count * 3;

    void AllocateSourceBuffers()
    {
        var icount = _tempList.I.Count;
        var vcount = _tempList.P.Count;
        _source.I = new ComputeBuffer(icount, sizeof(int));
        _source.P = new ComputeBuffer(vcount * 3, sizeof(float));
        _source.T = new ComputeBuffer(vcount * 2, sizeof(float));
    }

    void DeallocateSourceBuffers()
    {
        _source.I?.Dispose();
        _source.P?.Dispose();
        _source.T?.Dispose();
        _source = (null, null, null);
    }

    #endregion

    #region Render textures storing conversion results

    // Position, Normal, Color
    (RenderTexture P, RenderTexture N, RenderTexture C) _converted;

    // The texture width is fixed at 1024.
    const int _textureWidth = 1024;

    // Calculate the texture height from a given target vertex count.
    static int CalculateTextureHeight(int targetVertexCount)
      => (targetVertexCount + _textureWidth - 1) / _textureWidth;

    bool CheckValidConversionRTSize(int targetVertexCount)
      => _converted.P.height == CalculateTextureHeight(targetVertexCount);

    void AllocateConversionRTs(int targetVertexCount)
    {
        var w = _textureWidth;
        var h = CalculateTextureHeight(targetVertexCount);
        _converted.P = NewRenderTexture(w, h, RenderTextureFormat.ARGBFloat);
        _converted.N = NewRenderTexture(w, h, RenderTextureFormat.ARGBHalf);
        _converted.C = NewRenderTexture(w, h, RenderTextureFormat.Default);
    }

    void DeallocateConversionRTs()
    {
        if (_converted.P != null) Object.Destroy(_converted.P);
        if (_converted.N != null) Object.Destroy(_converted.N);
        if (_converted.C != null) Object.Destroy(_converted.C);
        _converted = (null, null, null);
    }

    #endregion

    #region Internal utility function

    static RenderTexture NewRenderTexture
      (int width, int height, RenderTextureFormat format)
    {
        var rt = new RenderTexture(width, height, 0, format);
        rt.enableRandomWrite = true;
        rt.Create();
        return rt;
    }

    #endregion

    #region Pre-calculated ID hashes

    static class IDs
    {
        public static int IndexCount  = Shader.PropertyToID("IndexCount");
        public static int Indices     = Shader.PropertyToID("Indices");
        public static int Positions   = Shader.PropertyToID("Positions");
        public static int TexCoords   = Shader.PropertyToID("TexCoords");
        public static int BaseTexture = Shader.PropertyToID("BaseTexture");
        public static int PositionMap = Shader.PropertyToID("PositionMap");
        public static int NormalMap   = Shader.PropertyToID("NormalMap");
        public static int ColorMap    = Shader.PropertyToID("ColorMap");
    }

    #endregion
}

}
