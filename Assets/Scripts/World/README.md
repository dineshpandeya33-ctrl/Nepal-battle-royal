# World prototype scripts

- `NepalRegion.cs`: region types and normalized map-region data.
- `NepalMapGenerator.cs`: creates a procedural Unity Terrain heightmap and seven region marker objects.
- `RegionMarker.cs`: stores region metadata for later map UI and gameplay systems.

The generated world is a greybox foundation. It intentionally avoids bundling third-party models or textures. Replace markers with original environment prefabs during the art pass.
