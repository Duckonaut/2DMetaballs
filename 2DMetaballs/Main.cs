using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Metaballs
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Main : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public static Random Rand { get; protected set; }
		public static Rectangle WindowBounds { get; set; }
		public static RenderTarget2D MetaballTarget { get; set; }
		public static RenderTarget2D TmpTarget { get; set; }
		public static Texture2D Mask { get; set; }
		public static Texture2D Galaxy { get; set; }
		public static List<Metaball> Metaballs { get; set; }

		private Effect metaballEffect;
		public static float offset = 0f;
		public Main()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.GraphicsProfile = GraphicsProfile.HiDef;
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			Rand = new Random();
			WindowBounds = Window.ClientBounds;

			MetaballTarget = new RenderTarget2D(GraphicsDevice, WindowBounds.Width, WindowBounds.Height);
			TmpTarget = new RenderTarget2D(GraphicsDevice, WindowBounds.Width, WindowBounds.Height);

			Metaballs = new List<Metaball>();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			Mask = Content.Load<Texture2D>("Mask");
			Galaxy = Content.Load<Texture2D>("Galaxy");
			metaballEffect = Content.Load<Effect>("MetaballEffect");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			Input.CurrentState = Keyboard.GetState();
			Input.CurrentMouseState = Mouse.GetState();

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (Input.MouseHold)
			{
				Metaballs.Add(new Metaball(Input.MousePos.ToVector2(), 0.5f + (float)Rand.NextDouble()));
			}


			foreach (var m in Metaballs)
			{
				m.Update();
			}

			offset += 0.01f;


			Input.PastState = Input.CurrentState;
			Input.PastMouseState = Input.CurrentMouseState;

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(MetaballTarget);
			GraphicsDevice.Clear(Color.Transparent);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

			foreach (var m in Metaballs)
			{
				m.Draw(spriteBatch);
			}

			spriteBatch.Draw(Mask, Input.MousePos.ToVector2(), null, Color.White, 0f, Vector2.One * 256f, 1f / 32f, SpriteEffects.None, 0);


			spriteBatch.End();

			metaballEffect.Parameters["width"].SetValue((float)WindowBounds.Width);
			metaballEffect.Parameters["height"].SetValue((float)WindowBounds.Height);
			metaballEffect.Parameters["GalaxyTexture"].SetValue(Galaxy);
			metaballEffect.Parameters["offset"].SetValue(new Vector2((float)Math.Sin(offset), (float)Math.Cos(offset)) * 0.1f);
			AddEffect(metaballEffect);



			GraphicsDevice.SetRenderTarget(null);

			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(2));

			spriteBatch.Draw(MetaballTarget, Vector2.Zero, null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

			spriteBatch.End();


			base.Draw(gameTime);
		}

		private void AddEffect(Effect effect)
		{
			GraphicsDevice.SetRenderTarget(TmpTarget);
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, effect: effect, samplerState: SamplerState.PointClamp);

			//effect.CurrentTechnique.Passes[0].Apply();

			spriteBatch.Draw(MetaballTarget, position: Vector2.Zero, color: Color.White);

			spriteBatch.End();

			GraphicsDevice.SetRenderTarget(MetaballTarget);
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, effect: null, samplerState: SamplerState.PointClamp);

			spriteBatch.Draw(TmpTarget, position: Vector2.Zero, color: Color.White);

			spriteBatch.End();
		}
	}
}
