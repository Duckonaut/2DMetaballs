using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaballs
{
	public class Metaball: IMetaball
	{
		public Metaball(Vector2 position, float scale)
		{
			Position = position;
			Scale = scale;
			rotationConst = (float)Main.Rand.NextDouble() * 6.28f;

			Main.Starjizz.MetaballsToDraw.Add(this);
		}

		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; set; }
		public float Scale { get; set; }
		private float rotationConst;


		public void Update()
		{
			Velocity = new Vector2((float)Math.Cos(Main.offset) * (float)Math.Sin(rotationConst) * 0.2f + rotationConst * 0.1f, (float)Math.Sin(Main.offset) * (float)Math.Cos(rotationConst) * 0.2f - rotationConst * 0.1f).RotatedBy(rotationConst);

			if (Input.Down)
			{
				Vector2 delta = Input.MousePos - Position;
				float lenSquared = delta.LengthSquared();
				delta.Normalize();

				Velocity += delta * lenSquared * 0.01f;
			}

			Position += Velocity;

			Position = new Vector2((Position.X + Main.WindowBounds.Width) % Main.WindowBounds.Width, (Position.Y + Main.WindowBounds.Height) % Main.WindowBounds.Height);
		}

		public void DrawOnMetaballLayer(SpriteBatch sB)
		{
			Main.Starjizz.borderNoise.Parameters["offset"].SetValue(Main.offset + rotationConst);

			sB.Draw(Main.Mask, Position, null, Color.White, 0f, Vector2.One * 256f, Scale / 16f, SpriteEffects.None, 0);
		}
	}
}
