Abcvfx
======

![gif](https://i.imgur.com/el5ZkKX.gif)
![gif](https://i.imgur.com/UJb4N70.gif)

**Abcvfx** is a Unity sample project showing using VFX Graph with Alembic animation.

Previously, this repository contained a special package to support Alembic animation on VFX Graph.
This special package is no longer needed because VFX Graph accepts dynamic mesh input as a standard feature in the recent versions.

"Static Texture" sample
-----------------------

`StaticTexture` is a sample scene of Alembic animation with a static texture/topology.

"Video Texture" sample
----------------------

`VideoTexture` is a sample scene of Alembic animation with dynamic texture/topology ("volumetric video").

It uses "BMX" sample sequence from 4Dviews, but this repository doesn't contain those files.
You can download them from the [4Dviews website]. [^1]

[4Dviews website]: https://www.4dviews.com

[^1]: The "Samples" section on their home page only contains sample files in their proprietary .4ds format.
      The Alembic files are available on the Creator Platform page (registration required).

You can use ffmpeg to create a HAP video clip from the image sequence in the 4Dviews sample.

```
ffmpeg -r 30 -i BMX_%06d.jpg -c:v hap BMX.mov
```

Put the generated `BMX.mov` file in the `Assets/StreamingAssets` directory.
