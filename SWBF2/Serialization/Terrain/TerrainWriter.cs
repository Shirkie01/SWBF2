using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace SWBF2.Serialization
{
    internal class TerrainWriter : BinaryWriter
    {
        public TerrainWriter(Stream stream) : base(stream)
        {
        }

        public void Write(Terrain terrain)
        {
            Write(terrain.Header);

            if (BaseStream.Position != 2821)
            {
                throw new InvalidProgramException("Header is invalid");
            }

            var gridSize = terrain.Header.GridSize;
            var gridSizeSquared = gridSize * gridSize;

            // Heights
            var expectedPosition = BaseStream.Position + gridSizeSquared * 2;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                Write(terrain.Blocks[x, y].Height);
            });

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidProgramException("Heights are invalid");
            }

            // Colors
            expectedPosition += gridSizeSquared * 4;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                Write(terrain.Blocks[x, y].ForegroundColor);
            });

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidProgramException("Colors are invalid");
            }

            // More Colors
            expectedPosition += gridSizeSquared * 4;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                Write(terrain.Blocks[x, y].BackgroundColor);
            });
            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidProgramException("Colors are invalid");
            }

            // Texture alphas
            expectedPosition += gridSizeSquared * 16;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                var block = terrain.Blocks[x, y];
                for (int i = 0; i < block.TextureAlphas.Length; i++)
                {
                    Write(block.TextureAlphas[i]);
                }
            });

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidProgramException("TextureAlphas are invalid");
            }

            // Write the first blend height of every 4x4 block
            expectedPosition += gridSizeSquared / 8;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                Write(terrain.Blocks[x, y].BlendHeight1);
            }, 4);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidProgramException("BlendHeight1 values are invalid");
            }

            // Write the second blend height of every 4x4 block
            expectedPosition += gridSizeSquared / 8;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                Write(terrain.Blocks[x, y].BlendHeight2);
            }, 4);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidProgramException("BlendHeight2 values are invalid");
            }

            // Write the water id info. Fixed size
            expectedPosition += gridSizeSquared / 4;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                var block = terrain.Blocks[x, y];

                byte[] bytes = MapTextureLayerIdsToTextureBytes(block.TextureLayerIds);

                Write(bytes[0]);
                Write(bytes[1]);
                Write(block.WaterLayerId);
                Write((byte)0);
            }, 4);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidProgramException("WaterIds are invalid");
            }

            // Foliage is a fixed size
            expectedPosition += 1024 * 1024 / 8;
            WriteFoliage(1024, terrain.Blocks);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidProgramException("Foliage is invalid");
            }

            WriteEndOfFile(terrain);
        }

        /// <summary>
        /// Writes the TER header using the provided writer
        /// </summary>
        /// <param name="writer">The writer</param>
        /// <param name="header">The terrain header</param>
        private void Write(TerrainHeader header)
        {
            Write(Encoding.UTF8.GetBytes("TERR"));
            Write((int)header.Version);

            // Write Extents
            Write(header.Extents.MinX);
            Write(header.Extents.MinY);
            Write(header.Extents.MaxX);
            Write(header.Extents.MaxY);

            Write(header.Unknown1);

            foreach (var layer in header.TextureLayers)
            {
                Write(1 / layer.TileRange);
            }

            foreach (var layer in header.TextureLayers)
            {
                Write(layer.MappingType);
            }

            foreach (var layer in header.TextureLayers)
            {
                Write(layer.Rotation);
            }

            Write(header.MapHeightMultiplier);

            Write(header.GridScale);

            // There's a 1 here. No idea why. Can't remove it though!
            Write(header.Unknown2);

            Write(header.GridSize);

            Write((int)header.InGameOptions);

            // There's a value here, but only on SWBF2 terrains. No idea what it means
            if (header.Version == TerrainVersion.SWBF2)
            {
                Write(header.SWBF2Byte);
            }

            foreach (var layer in header.TextureLayers)
            {
                Write(Encoding.UTF8.GetBytes(layer.DiffuseTexture.PadRight(32, '\0')));
                Write(Encoding.UTF8.GetBytes(layer.DetailTexture.PadRight(32, '\0')));
            }

            foreach (var waterLayer in header.WaterLayers)
            {
                Write(waterLayer);
            }

            foreach (var decalTex in header.DecalTextureNames)
            {
                Write(Encoding.UTF8.GetBytes(decalTex.PadRight(32, '\0')));
            }

            Write(header.DecalLength * 176);

            // unknown
            Write(header.UnknownBytes);
        }

        private void Write(WaterLayer water)
        {
            // Height is stored twice. May have been intended for future use.
            Write(water.Height1);
            Write(water.Height2);

            Write(water.Unknown1);
            Write(water.Unknown2);

            Write(water.UVAnimation.Velocity.X);
            Write(water.UVAnimation.Velocity.Y);
            Write(water.UVAnimation.Repeat.X);
            Write(water.UVAnimation.Repeat.Y);
            Write(water.Color);
            Write(Encoding.UTF8.GetBytes(water.TextureName.PadRight(32, '\0')));
        }

        /// <summary>
        /// Writes the color to the TER file. Stored as BGRA
        /// </summary>
        /// <param name="writer">The writer</param>
        /// <param name="color">The color</param>
        private void Write(Color color)
        {
            Write(new byte[] { color.B, color.G, color.R, color.A });
        }

        /// <summary>
        /// Maps the list of texture layer ids to the required byte fields
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>
        /// A two-byte array, with the ids split into masked enum values 0-7 = 2^n, 8-15 = 2^(n-7)
        /// </returns>
        private byte[] MapTextureLayerIdsToTextureBytes(bool[] ids)
        {
            byte[] bytes = new byte[2];

            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];

                if (id)
                {
                    if (i > 7)
                    {
                        bytes[1] += (byte)(Math.Pow(2, i - 8));
                    }
                    else
                    {
                        bytes[0] += (byte)(Math.Pow(2, i));
                    }
                }
            }

            return bytes;
        }

        /// <summary>
        /// Writes the foliage byte for the terrain file
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="terrain"></param>
        private void WriteFoliage(int gridSize, TerrainBlock[,] blocks)
        {
            // The TER file stores foliage as a nibble for every 4 vertices. As such we don't need to
            // write the value of every vertex, just every other one in both directions
            StringBuilder valuesToWrite = new StringBuilder();
            for (int x = 0; x < gridSize; x += 2)
            {
                valuesToWrite.Clear();

                for (int y = 0; y < gridSize; y += 2)
                {
                    // get the int value of the enum (0-15) and convert it to hex
                    var mask = (int)blocks[x, y].FoliageTypes;
                    valuesToWrite.Append(mask.ToString("X"));
                }

                if (valuesToWrite.Length % 2 != 0)
                {
                    throw new InvalidProgramException("The foliage values must be a multiple of 2");
                }

                var hex = valuesToWrite.ToString();

                //if (hex.Length != size / 2) // ( size / 4 (size of foliage info) * 2 (size of byte)
                //    throw new ArgumentException("Error setting up for writing foliage");

                var array = Enumerable.Range(0, hex.Length)
                 .Where(i => i % 2 == 0)
                 .Select(i => Convert.ToByte(hex.Substring(i, 2), 16))
                 .ToArray();

                Write(array);
            }
        }

        private void WriteEndOfFile(Terrain terrain)
        {
            // Both unknowns below may be related to Decal Edit mode (F11) in the editor. (3 floats
            // every fourth block:scale x,y, and rotation?)

            // Unknown. Appears to be related to gridsize
            Write(terrain.UnknownBytes1);

            // Unknown. Appears to be related to gridsize
            Write(terrain.UnknownBytes2);

            Write(terrain.EndOfFileLength);

            // Ending bytes
            //Write(terrain.EndOfFileBytes);
        }
    }
}