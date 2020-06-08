Abcvfx
======

![gif](https://i.imgur.com/KFuE0uJ.gif)
![gif](https://i.imgur.com/el5ZkKX.gif)

**Abcvfx** is a Unity package that allows using an [Alembic] animation as a VFX
source.

[Alembic]: https://docs.unity3d.com/Packages/com.unity.formats.alembic@latest/

MeshBaker Component
-------------------

**MeshBaker** is a Unity component that converts an animating mesh into an
animating point cloud. It not only bakes vertex positions and normals but also
vertex colors from a given texture.

It also supports a [HAP video texture], which is useful for rendering a
volumetric video like [4DViews].

[HAP video texture]: https://github.com/keijiro/KlakHap
[4DViews]: https://www.4dviews.com/volumetric-resources

![gif](https://i.imgur.com/UJb4N70.gif)

You can bind a baked point cloud to a VFX component using
**VFXBakedMeshBinder**.
