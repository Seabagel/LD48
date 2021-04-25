Public Class Game1
    'A lot of the code found here was made by Aardaerimus D'Aritonyss, I simply followed his tutorials to get started but added my own code snippets to the engine
    Inherits Microsoft.Xna.Framework.Game
    Private ScreenManager As ScreenManager
    Public Sub New()
        Globals.Graphics = New GraphicsDeviceManager(Me)
        Content.RootDirectory = "Content"
    End Sub
    Public Shared Change As Boolean = False
    Public Sub Synch()
        If Globals.Graphics.SynchronizeWithVerticalRetrace = True Then
            Globals.Graphics.SynchronizeWithVerticalRetrace = False
            Me.IsFixedTimeStep = False
        Else
            Globals.Graphics.SynchronizeWithVerticalRetrace = True
            Me.IsFixedTimeStep = True
        End If
        Globals.Graphics.ApplyChanges()
    End Sub
    Protected Overrides Sub Initialize()
        Me.IsMouseVisible = True
        Window.AllowUserResizing = True
        Globals.GameSize = New Vector2(1600, 900)
        Globals.Graphics.PreferredBackBufferWidth = Globals.GameSize.X
        Globals.Graphics.PreferredBackBufferHeight = Globals.GameSize.Y
        Globals.Graphics.ApplyChanges()
        Globals.BackBuffer = New RenderTarget2D(Globals.Graphics.GraphicsDevice, Globals.GameSize.X, Globals.GameSize.Y, False, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents)
        MyBase.Initialize()
        Globals.Graphics.SynchronizeWithVerticalRetrace = True
        Me.IsFixedTimeStep = True
        Globals.Graphics.ApplyChanges()
        Globals.Debugging = False
    End Sub
    Protected Overrides Sub LoadContent()
        Globals.SpriteBatch = New SpriteBatch(GraphicsDevice)
        Globals.Content = Me.Content
        Fonts.Load()
        Textures.Load()
        Sounds.Load()
        ScreenManager = New ScreenManager()
        ScreenManager.AddScreen(New Stage)
    End Sub
    Protected Overrides Sub Update(ByVal gameTime As GameTime)
        If Change Then
            Synch()
            Change = False
        End If
        MyBase.Update(gameTime)
        Globals.WindowFocused = Me.IsActive
        Globals.GameTime = gameTime
        ScreenManager.Update()
        Input.Update()
    End Sub
    Protected Overrides Sub Draw(ByVal gameTime As GameTime)
        GraphicsDevice.Clear(Color.Black)
        Globals.SpriteBatch.Begin()
        MyBase.Draw(gameTime)
        ScreenManager.Draw()
        Globals.SpriteBatch.End()
    End Sub
End Class
