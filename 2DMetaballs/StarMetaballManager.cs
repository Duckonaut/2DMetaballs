using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaballs
{
	public class StarMetaballManager
	{
		public List<IMetaball> MetaballsToDraw { get; protected set; }
		public List<IGalaxySprite> SpritesToDraw { get; protected set; }
		public RenderTarget2D MetaballTarget { get; protected set; }
		public RenderTarget2D TmpTarget { get; protected set; }
		public Texture2D Galaxy { get; set; }

		private Effect metaballColorCode;
		private Effect metaballEdgeDetection;
		public Effect borderNoise;
		private Effect galaxyParallax;

		public StarMetaballManager()
		{
			MetaballsToDraw = new List<IMetaball>();
			SpritesToDraw = new List<IGalaxySprite>();
		}

		public void Initialize(GraphicsDevice graphicsDevice)
		{
			UpdateWindowSize(graphicsDevice, Main.WindowBounds.Width, Main.WindowBounds.Height);
		}
		public void LoadContent(ContentManager content)
		{
			Galaxy = content.Load<Texture2D>("Galaxy");
			metaballColorCode = content.Load<Effect>("MetaballColorCode");
			metaballEdgeDetection = content.Load<Effect>("MetaballEdgeDetection");
			borderNoise = content.Load<Effect>("BorderNoise");
			galaxyParallax = content.Load<Effect>("GalaxyParallax");
		}

		public void UpdateWindowSize(GraphicsDevice graphicsDevice, int width, int height)
		{
			MetaballTarget = new RenderTarget2D(graphicsDevice, width, height);
			TmpTarget = new RenderTarget2D(graphicsDevice, width, height);
		}

		public void DrawToTarget(SpriteBatch sB, GraphicsDevice graphicsDevice)
		{
			graphicsDevice.SetRenderTarget(MetaballTarget);
			graphicsDevice.Clear(Color.Transparent);

			borderNoise.Parameters["offset"].SetValue(Main.offset);

			sB.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, effect: borderNoise);

			foreach (var m in MetaballsToDraw)
			{
				m.DrawOnMetaballLayer(sB);
			}

			sB.End();


			if (!Input.Right)
			{
				AddEffect(sB, graphicsDevice, metaballColorCode);
			}

			sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, effect: null);

			foreach (var s in SpritesToDraw)
			{
				s.DrawGalaxyMappedSprite(sB);
			}

			sB.End();

			metaballEdgeDetection.Parameters["width"].SetValue((float)Main.WindowBounds.Width);
			metaballEdgeDetection.Parameters["height"].SetValue((float)Main.WindowBounds.Height);
			AddEffect(sB, graphicsDevice, metaballEdgeDetection);
		}

		public void Draw(SpriteBatch sB)
		{
			galaxyParallax.Parameters["screenWidth"].SetValue((float)Main.WindowBounds.Width);
			galaxyParallax.Parameters["screenHeight"].SetValue((float)Main.WindowBounds.Height);
			galaxyParallax.Parameters["width"].SetValue((float)Galaxy.Width);
			galaxyParallax.Parameters["height"].SetValue((float)Galaxy.Height);
			galaxyParallax.Parameters["GalaxyTexture"].SetValue(Galaxy);
			galaxyParallax.Parameters["offset"].SetValue(new Vector2((float)Math.Sin(Main.offset) * 0.2f + Input.MousePos.X, (float)Math.Cos(Main.offset) * 0.2f + Input.MousePos.Y) * 0.1f);

			sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, galaxyParallax, Matrix.CreateScale(2));

			sB.Draw(MetaballTarget, Vector2.Zero, null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

			

			sB.End();
		}

		private void AddEffect(SpriteBatch sB, GraphicsDevice graphicsDevice, Effect effect)
		{
			graphicsDevice.SetRenderTarget(TmpTarget);
			graphicsDevice.Clear(Color.Black);

			sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, effect: effect, samplerState: SamplerState.PointClamp);

			//effect.CurrentTechnique.Passes[0].Apply();

			sB.Draw(MetaballTarget, position: Vector2.Zero, color: Color.White);

			sB.End();

			graphicsDevice.SetRenderTarget(MetaballTarget);
			graphicsDevice.Clear(Color.Black);

			sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, effect: null, samplerState: SamplerState.PointClamp);

			sB.Draw(TmpTarget, position: Vector2.Zero, color: Color.White);

			sB.End();
		}
	}

	public interface IGalaxySprite
	{
		/// <summary>
		/// Draw parts of sprite that are color coded, and should be drawn with the metaball layer. Galaxy Parallax shader is active.
		/// </summary>
		/// <param name="sB"></param>
		void DrawGalaxyMappedSprite(SpriteBatch sB);
	}

	public interface IMetaball
	{
		/// <summary>
		/// Draws metaball masks on the metaball target. The borded noise shader is active.
		/// </summary>
		/// <param name="sB">SpriteBatch to draw to</param>
		void DrawOnMetaballLayer(SpriteBatch sB);
	}
}
