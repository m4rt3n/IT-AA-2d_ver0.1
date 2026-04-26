/*
 * Datei: WinterTownTilesetCleaner.cs
 * Zweck: Erzeugt aus dem Winter-Town-Preview-Bild ein label-freies 32x32-Tileset und separate Prop-PNGs.
 * Verantwortung: Schneidet definierte Nutzbereiche aus, entfernt Preview-Hintergrund, schreibt Mapping-JSON und setzt Unity-Importsettings.
 * Abhaengigkeiten: UnityEditor, TextureImporter, Texture2D, vorhandenes Winter-Town-Preview-PNG.
 * Verwendung: Menue `ITAA/Tilesets/Create Clean 32x32 Winter Town Tileset` oder Batchmode ExecuteMethod.
 */

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ITAA.EditorTools.SceneBuilding
{
    public static class WinterTownTilesetCleaner
    {
        private const string SourcePath = "Assets/Projekt/Content/Art/Floors/winter_town_tileset.png";
        private const string OutputFolder = "Assets/Projekt/Content/Art/Tiles";
        private const string OutputPath = OutputFolder + "/WinterTownTileset_Clean_32.png";
        private const string MappingPath = OutputFolder + "/WinterTownTileset_Clean_32.mapping.json";
        private const string PropsFolder = "Assets/Projekt/Content/Art/Props/TestWorld";
        private const int TileSize = 32;
        private const int SheetTiles = 32;
        private const int SheetPixels = TileSize * SheetTiles;

        [MenuItem("ITAA/Tilesets/Create Clean 32x32 Winter Town Tileset")]
        public static void CreateCleanTileset()
        {
            if (!File.Exists(SourcePath))
            {
                Debug.LogError($"[TilesetCleaner] Source fehlt: {SourcePath}");
                return;
            }

            EnsureFolder("Assets/Projekt/Content/Art", "Tiles");
            EnsureFolder("Assets/Projekt/Content/Art", "Props");
            EnsureFolder("Assets/Projekt/Content/Art/Props", "TestWorld");

            TextureImporter sourceImporter = AssetImporter.GetAtPath(SourcePath) as TextureImporter;
            bool wasReadable = sourceImporter != null && sourceImporter.isReadable;
            SetReadable(SourcePath, true);

            Texture2D source = AssetDatabase.LoadAssetAtPath<Texture2D>(SourcePath);
            if (source == null)
            {
                Debug.LogError($"[TilesetCleaner] Source konnte nicht geladen werden: {SourcePath}");
                return;
            }

            TileSpec[] tiles = CreateTileSpecs();
            PropSpec[] props = CreatePropSpecs();
            Texture2D sheet = CreateTransparentTexture(SheetPixels, SheetPixels);

            for (int i = 0; i < tiles.Length; i++)
            {
                CopyToSheet(source, sheet, tiles[i]);
            }

            sheet.Apply(false, false);
            File.WriteAllBytes(OutputPath, sheet.EncodeToPNG());
            Object.DestroyImmediate(sheet);
            AssetDatabase.ImportAsset(OutputPath, ImportAssetOptions.ForceUpdate);
            ConfigureSheetImporter(tiles);

            for (int i = 0; i < props.Length; i++)
            {
                WriteProp(source, props[i]);
            }

            WriteMapping(tiles, props);
            SetReadable(SourcePath, wasReadable);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[TilesetCleaner] Clean Tileset erzeugt: {OutputPath}");
        }

        private static void CopyToSheet(Texture2D source, Texture2D sheet, TileSpec spec)
        {
            Color[] pixels = ReadCleanPixels(source, spec.SourceX, spec.SourceY, TileSize, TileSize);
            int destinationX = spec.GridX * TileSize;
            int destinationY = SheetPixels - ((spec.GridY + 1) * TileSize);
            sheet.SetPixels(destinationX, destinationY, TileSize, TileSize, pixels);
        }

        private static void WriteProp(Texture2D source, PropSpec spec)
        {
            Color[] pixels = ReadCleanPixels(source, spec.SourceX, spec.SourceY, spec.Width, spec.Height);
            RectInt bounds = FindContentBounds(pixels, spec.Width, spec.Height);
            int width = Mathf.Max(1, bounds.width);
            int height = Mathf.Max(1, bounds.height);
            Texture2D prop = CreateTransparentTexture(width, height);

            Color[] cropped = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cropped[y * width + x] = pixels[(bounds.y + y) * spec.Width + bounds.x + x];
                }
            }

            prop.SetPixels(cropped);
            prop.Apply(false, false);
            string path = $"{PropsFolder}/{spec.Name}.png";
            File.WriteAllBytes(path, prop.EncodeToPNG());
            Object.DestroyImmediate(prop);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            ConfigurePropImporter(path);
        }

        private static Color[] ReadCleanPixels(Texture2D source, int sourceX, int sourceYTop, int width, int height)
        {
            int unityY = source.height - sourceYTop - height;
            Color[] pixels = source.GetPixels(sourceX, unityY, width, height);

            for (int i = 0; i < pixels.Length; i++)
            {
                if (IsPreviewBackground(pixels[i]))
                {
                    pixels[i] = Color.clear;
                }
            }

            return pixels;
        }

        private static bool IsPreviewBackground(Color color)
        {
            if (color.a <= 0.02f)
            {
                return true;
            }

            float r = color.r;
            float g = color.g;
            float b = color.b;
            bool darkNavy = r < 0.12f && g < 0.20f && b < 0.27f && b > r + 0.03f;
            bool sectionLine = r > 0.18f && r < 0.34f && g > 0.24f && g < 0.42f && b > 0.30f && b < 0.52f;
            return darkNavy || sectionLine;
        }

        private static RectInt FindContentBounds(Color[] pixels, int width, int height)
        {
            int minX = width;
            int minY = height;
            int maxX = -1;
            int maxY = -1;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (pixels[y * width + x].a <= 0.02f)
                    {
                        continue;
                    }

                    minX = Mathf.Min(minX, x);
                    minY = Mathf.Min(minY, y);
                    maxX = Mathf.Max(maxX, x);
                    maxY = Mathf.Max(maxY, y);
                }
            }

            if (maxX < minX || maxY < minY)
            {
                return new RectInt(0, 0, width, height);
            }

            return new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }

        private static Texture2D CreateTransparentTexture(int width, int height)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.clear;
            }

            texture.SetPixels(pixels);
            return texture;
        }

        private static void ConfigureSheetImporter(TileSpec[] tiles)
        {
            TextureImporter importer = AssetImporter.GetAtPath(OutputPath) as TextureImporter;
            if (importer == null)
            {
                return;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = TileSize;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;

#pragma warning disable 0618
            SpriteMetaData[] metadata = new SpriteMetaData[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
            {
                metadata[i] = new SpriteMetaData
                {
                    name = tiles[i].Name,
                    rect = new Rect(tiles[i].GridX * TileSize, SheetPixels - ((tiles[i].GridY + 1) * TileSize), TileSize, TileSize),
                    alignment = (int)SpriteAlignment.Center,
                    pivot = new Vector2(0.5f, 0.5f)
                };
            }

            importer.spritesheet = metadata;
#pragma warning restore 0618
            importer.SaveAndReimport();
        }

        private static void ConfigurePropImporter(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                return;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = TileSize;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.spritePivot = new Vector2(0.5f, 0f);
            importer.SaveAndReimport();
        }

        private static void WriteMapping(TileSpec[] tiles, PropSpec[] props)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"tileSize\": 32,");
            builder.AppendLine("  \"sheet\": \"WinterTownTileset_Clean_32.png\",");
            builder.AppendLine("  \"tiles\": [");

            for (int i = 0; i < tiles.Length; i++)
            {
                TileSpec tile = tiles[i];
                builder.AppendLine("    {");
                builder.AppendLine($"      \"name\": \"{tile.Name}\",");
                builder.AppendLine($"      \"category\": \"{tile.Category}\",");
                builder.AppendLine($"      \"x\": {tile.GridX},");
                builder.AppendLine($"      \"y\": {tile.GridY},");
                builder.AppendLine($"      \"collision\": {tile.Collision.ToString().ToLowerInvariant()},");
                builder.AppendLine($"      \"sortingOrder\": {tile.SortingOrder}");
                builder.Append(i < tiles.Length - 1 ? "    },\n" : "    }\n");
            }

            builder.AppendLine("  ],");
            builder.AppendLine("  \"props\": [");

            for (int i = 0; i < props.Length; i++)
            {
                PropSpec prop = props[i];
                builder.AppendLine("    {");
                builder.AppendLine($"      \"name\": \"{prop.Name}\",");
                builder.AppendLine($"      \"path\": \"{PropsFolder}/{prop.Name}.png\",");
                builder.AppendLine($"      \"collision\": {prop.Collision.ToString().ToLowerInvariant()},");
                builder.AppendLine($"      \"sortingOrder\": {prop.SortingOrder}");
                builder.Append(i < props.Length - 1 ? "    },\n" : "    }\n");
            }

            builder.AppendLine("  ]");
            builder.AppendLine("}");
            File.WriteAllText(MappingPath, builder.ToString());
            AssetDatabase.ImportAsset(MappingPath, ImportAssetOptions.ForceUpdate);
        }

        private static TileSpec[] CreateTileSpecs()
        {
            List<TileSpec> specs = new List<TileSpec>();

            Add(specs, "snow_base_01", "Ground", 0, 0, 16, 68, false, 0);
            Add(specs, "snow_base_02", "Ground", 1, 0, 68, 68, false, 0);
            Add(specs, "snow_detail_01", "Ground", 2, 0, 16, 124, false, 2);
            Add(specs, "snow_edge_top", "Ground", 3, 0, 172, 68, false, 2);
            Add(specs, "snow_edge_bottom", "Ground", 4, 0, 224, 68, false, 2);
            Add(specs, "snow_edge_left", "Ground", 5, 0, 276, 68, false, 2);
            Add(specs, "snow_edge_right", "Ground", 6, 0, 328, 68, false, 2);
            Add(specs, "snow_corner_tl", "Ground", 7, 0, 172, 124, false, 2);
            Add(specs, "snow_corner_tr", "Ground", 8, 0, 224, 124, false, 2);
            Add(specs, "snow_corner_bl", "Ground", 9, 0, 276, 124, false, 2);
            Add(specs, "snow_corner_br", "Ground", 10, 0, 328, 124, false, 2);
            Add(specs, "ice_base_01", "Ground", 11, 0, 16, 180, false, 0);
            Add(specs, "ice_detail_01", "Ground", 12, 0, 120, 180, false, 2);

            Add(specs, "road_cobble_center", "Roads", 0, 4, 16, 267, false, 1);
            Add(specs, "road_cobble_h", "Roads", 1, 4, 68, 267, false, 1);
            Add(specs, "road_cobble_v", "Roads", 2, 4, 120, 267, false, 1);
            Add(specs, "road_cobble_cross", "Roads", 3, 4, 172, 267, false, 1);
            Add(specs, "road_cobble_t_up", "Roads", 4, 4, 224, 267, false, 1);
            Add(specs, "road_cobble_t_down", "Roads", 5, 4, 276, 267, false, 1);
            Add(specs, "road_cobble_t_left", "Roads", 6, 4, 328, 267, false, 1);
            Add(specs, "road_cobble_t_right", "Roads", 7, 4, 380, 267, false, 1);
            Add(specs, "road_cobble_corner_tl", "Roads", 8, 4, 412, 267, false, 1);
            Add(specs, "road_cobble_corner_tr", "Roads", 9, 4, 16, 324, false, 1);
            Add(specs, "road_cobble_corner_bl", "Roads", 10, 4, 68, 324, false, 1);
            Add(specs, "road_cobble_corner_br", "Roads", 11, 4, 120, 324, false, 1);
            Add(specs, "road_dark_center", "Roads", 12, 4, 172, 324, false, 1);
            Add(specs, "road_dark_edge", "Roads", 13, 4, 224, 324, false, 1);

            Add(specs, "wall_stone_h", "Walls", 0, 8, 16, 413, true, 6);
            Add(specs, "wall_stone_v", "Walls", 1, 8, 120, 413, true, 6);
            Add(specs, "wall_stone_corner", "Walls", 2, 8, 276, 413, true, 6);
            Add(specs, "wall_stone_end", "Walls", 3, 8, 68, 413, true, 6);
            Add(specs, "fence_wood_h", "Walls", 4, 8, 328, 413, true, 6);
            Add(specs, "fence_wood_v", "Walls", 5, 8, 432, 413, true, 6);
            Add(specs, "fence_wood_corner", "Walls", 6, 8, 380, 413, true, 6);
            Add(specs, "fence_gate", "Walls", 7, 8, 224, 465, true, 6);

            Add(specs, "building_wall_wood", "Buildings", 0, 12, 16, 536, true, 5);
            Add(specs, "building_wall_stone", "Buildings", 1, 12, 224, 536, true, 5);
            Add(specs, "building_window_blue", "Buildings", 2, 12, 16, 588, true, 5);
            Add(specs, "building_window_small", "Buildings", 3, 12, 328, 588, true, 5);
            Add(specs, "building_door_wood", "Buildings", 4, 12, 120, 588, false, 5);
            Add(specs, "building_door_stone", "Buildings", 5, 12, 224, 588, false, 5);
            Add(specs, "building_arch", "Buildings", 6, 12, 328, 536, true, 5);
            Add(specs, "building_shop_front", "Buildings", 7, 12, 432, 536, true, 5);
            Add(specs, "building_corner_left", "Buildings", 8, 12, 380, 536, true, 5);
            Add(specs, "building_corner_right", "Buildings", 9, 12, 432, 588, true, 5);

            Add(specs, "roof_blue_center", "Roofs", 0, 18, 502, 68, true, 12);
            Add(specs, "roof_blue_edge_top", "Roofs", 1, 18, 554, 68, true, 12);
            Add(specs, "roof_blue_edge_bottom", "Roofs", 2, 18, 606, 68, true, 12);
            Add(specs, "roof_blue_edge_left", "Roofs", 3, 18, 658, 68, true, 12);
            Add(specs, "roof_blue_edge_right", "Roofs", 4, 18, 710, 68, true, 12);
            Add(specs, "roof_blue_corner_tl", "Roofs", 5, 18, 762, 68, true, 12);
            Add(specs, "roof_blue_corner_tr", "Roofs", 6, 18, 814, 68, true, 12);
            Add(specs, "roof_blue_corner_bl", "Roofs", 7, 18, 866, 68, true, 12);
            Add(specs, "roof_blue_corner_br", "Roofs", 8, 18, 918, 68, true, 12);
            Add(specs, "roof_gray_center", "Roofs", 9, 18, 502, 124, true, 12);
            Add(specs, "roof_orange_center", "Roofs", 10, 18, 502, 179, true, 12);
            Add(specs, "roof_purple_center", "Roofs", 11, 18, 762, 179, true, 12);
            Add(specs, "roof_gable_blue", "Roofs", 12, 18, 502, 226, true, 12);
            Add(specs, "roof_gable_orange", "Roofs", 13, 18, 658, 226, true, 12);
            Add(specs, "roof_gable_purple", "Roofs", 14, 18, 814, 226, true, 12);

            Add(specs, "tree_pine_small", "Nature", 0, 24, 502, 326, true, 6);
            Add(specs, "tree_pine_medium", "Nature", 1, 24, 554, 326, true, 6);
            Add(specs, "tree_pine_large", "Nature", 2, 24, 606, 326, true, 6);
            Add(specs, "bush_snow", "Nature", 3, 24, 606, 399, false, 6);
            Add(specs, "rock_snow", "Nature", 4, 24, 710, 326, true, 6);
            Add(specs, "flower_white", "Nature", 5, 24, 814, 327, false, 6);
            Add(specs, "flower_purple", "Nature", 6, 24, 866, 327, false, 6);
            Add(specs, "flower_red", "Nature", 7, 24, 918, 327, false, 6);
            Add(specs, "stump_snow", "Nature", 8, 24, 710, 399, true, 6);

            Add(specs, "crate_01", "Props", 0, 28, 502, 477, true, 4);
            Add(specs, "barrel_01", "Props", 1, 28, 659, 477, true, 4);
            Add(specs, "lamp_post", "Props", 2, 28, 918, 477, true, 4);
            Add(specs, "bench", "Props", 3, 28, 502, 590, true, 4);
            Add(specs, "sign", "Props", 4, 28, 550, 745, false, 4);
            Add(specs, "terminal", "Interactables", 5, 28, 1104, 73, false, 8);
            Add(specs, "notice_board", "Interactables", 6, 28, 1190, 73, false, 8);
            Add(specs, "item_marker", "Interactables", 7, 28, 1280, 134, false, 8);
            Add(specs, "water_ice", "Water", 8, 28, 16, 735, false, 0);
            Add(specs, "water_edge", "Water", 9, 28, 68, 735, false, 0);
            Add(specs, "waterfall_tile", "Water", 10, 28, 276, 735, false, 0);
            Add(specs, "market_stall_piece", "Props", 11, 28, 16, 888, true, 4);
            Add(specs, "tool_prop", "Props", 12, 28, 502, 890, true, 4);

            return specs.ToArray();
        }

        private static void Add(List<TileSpec> specs, string name, string category, int gridX, int gridY, int sourceX, int sourceY, bool collision, int sortingOrder)
        {
            specs.Add(new TileSpec(name, category, gridX, gridY, sourceX, sourceY, collision, sortingOrder));
        }

        private static PropSpec[] CreatePropSpecs()
        {
            return new[]
            {
                new PropSpec("fountain", 815, 590, 96, 100, true, 7),
                new PropSpec("large_tree", 552, 326, 96, 116, true, 7),
                new PropSpec("full_house_quiz", 16, 536, 192, 110, true, 7),
                new PropSpec("wagon", 1038, 710, 118, 76, true, 7),
                new PropSpec("market_stall", 16, 888, 154, 96, true, 7),
                new PropSpec("large_gate", 1220, 300, 110, 112, true, 7)
            };
        }

        private sealed class TileSpec
        {
            public TileSpec(string name, string category, int gridX, int gridY, int sourceX, int sourceY, bool collision, int sortingOrder)
            {
                Name = name;
                Category = category;
                GridX = gridX;
                GridY = gridY;
                SourceX = sourceX;
                SourceY = sourceY;
                Collision = collision;
                SortingOrder = sortingOrder;
            }

            public string Name { get; }
            public string Category { get; }
            public int GridX { get; }
            public int GridY { get; }
            public int SourceX { get; }
            public int SourceY { get; }
            public bool Collision { get; }
            public int SortingOrder { get; }
        }

        private sealed class PropSpec
        {
            public PropSpec(string name, int sourceX, int sourceY, int width, int height, bool collision, int sortingOrder)
            {
                Name = name;
                SourceX = sourceX;
                SourceY = sourceY;
                Width = width;
                Height = height;
                Collision = collision;
                SortingOrder = sortingOrder;
            }

            public string Name { get; }
            public int SourceX { get; }
            public int SourceY { get; }
            public int Width { get; }
            public int Height { get; }
            public bool Collision { get; }
            public int SortingOrder { get; }
        }

        private static void SetReadable(string path, bool readable)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null || importer.isReadable == readable)
            {
                return;
            }

            importer.isReadable = readable;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = parent + "/" + child;
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }
    }
}
#endif
