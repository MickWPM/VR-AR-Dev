using UnityEngine;
using Fusion;

namespace workshop
{
    public class CreateTexture : NetworkBehaviour
    {
        private Texture2D texture;
        private int width = 512;
        private int height = 512;

        public override void Spawned()
        {
            texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color col = new Color((float)x / width, (float)y / height, (float)(x + y) / (width + height));
                    texture.SetPixel(x, y, col);
                }
            }

            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            mr.material.mainTexture = texture;
            texture.Apply();
        }


        [Rpc(RpcSources.All, RpcTargets.All)]
        public void ChangeTextureRPC(string tex)
        {
            var playerId = int.Parse(tex);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (playerId % 2 == 0)
                    {
                        Color col = new Color((float)x / width, (float)y / height, 0);
                        texture.SetPixel(x, y, col);
                    }
                    else
                    {
                        Color col = new Color((float)y / width, (float)x / height, 0);
                        texture.SetPixel(x, y, col);
                    }

                }
            }

            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            mr.material.mainTexture = texture;
            texture.Apply();
        }

        public void ChangeTexture(string tex)
        {
            ChangeTextureRPC(tex);
        }
    }
}