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
		public static Rectangle WindowBounds;
		public static Texture2D Mask { get; set; }
		public static Texture2D Starachnid { get; set; }
		public static StarMetaballManager Starjizz { get; set; }
		public static List<Metaball> Metaballs { get; set; }

		public static float offset = 0f;

		private Starachnid s;
		public Main()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.GraphicsProfile = GraphicsProfile.HiDef;
			Content.RootDirectory = "Content";
			Window.AllowUserResizing = true;
			Starjizz = new StarMetaballManager();

			Window.ClientSizeChanged += WindowSizeChange;
		}

		private void WindowSizeChange(object sender = null, EventArgs e = null)
		{
			WindowBounds = Window.ClientBounds;
			WindowBounds.Width /= 2;
			WindowBounds.Height /= 2;

			Starjizz.UpdateWindowSize(GraphicsDevice, WindowBounds.Width, WindowBounds.Height);
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
			WindowSizeChange();

			Metaballs = new List<Metaball>();
			Starjizz = new StarMetaballManager();
			Starjizz.Initialize(GraphicsDevice);

			s = new Starachnid();

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
			Starachnid = Content.Load<Texture2D>("Starachnid");
			Starjizz.LoadContent(Content);
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
				Metaballs.Add(new Metaball(Input.MousePos, 0.5f + (float)Rand.NextDouble()));
			}


			foreach (var m in Metaballs)
			{
				m.Update();
			}
			s.Update();
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
			Starjizz.DrawToTarget(spriteBatch, GraphicsDevice);

			GraphicsDevice.SetRenderTarget(null);

			GraphicsDevice.Clear(Color.Black);

			Starjizz.Draw(spriteBatch);

			base.Draw(gameTime);
		}
	}
}
