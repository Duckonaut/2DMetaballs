using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaballs
{
	public class Starachnid: IGalaxySprite, IMetaball
	{
		public Starachnid()
		{
			Main.Starjizz.SpritesToDraw.Add(this);
			Main.Starjizz.MetaballsToDraw.Add(this);
		}

		public Vector2 Position { get; set; }


		public void Update()
		{

			Position = Input.MousePos;
		}

		public void DrawGalaxyMappedSprite(SpriteBatch sB)
		{
			sB.Draw(Main.Starachnid, Position, null, Color.White, 0f, Vector2.One * 16f, 1f, SpriteEffects.None, 0);
		}

		public void DrawOnMetaballLayer(SpriteBatch sB)
		{
			sB.Draw(Main.Mask, Position + new Vector2(0, -4), null, Color.White, 0f, Vector2.One * 256f, 1f / 10f, SpriteEffects.None, 0);
		}
	}
}
