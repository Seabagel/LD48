Public Class Stage
    Inherits BaseScreen
    Private AniTime As Double = 0
    Public Sub New()
        Name = "Stage"
        State = ScreenState.Active
        MapTex = Textures.Layer1
        Mapsize = New Point(MapTex.Width, MapTex.Height)
        ReDim map(MapTex.Width * MapTex.Height - 1)
        ReDim mapprev(MapTex.Width * MapTex.Height - 1)
        Textures.Layer1.GetData(Of Color)(map)
        Layers = New List(Of TexLayer)
        Dim t As New TexLayer
        t.IMG = Textures.Layer1
        t.OGTEX = 1
        Layers.Add(t)
        Layers.Add(t)
        For q As Integer = 0 To 50
            t = New TexLayer
            Select Case RNG.Free(0, 100)
                Case 0 To 33
                    t.IMG = Textures.Layer1
                    t.OGTEX = 1
                    Layers.Add(t)
                Case 34 To 66
                    t.IMG = Textures.Layer2
                    t.OGTEX = 2
                    Layers.Add(t)
                Case Else
                    t.IMG = Textures.Layer3
                    t.OGTEX = 3
                    Layers.Add(t)
            End Select
        Next
        t = New TexLayer
        t.IMG = Textures.Layer1
        t.OGTEX = 1
        Layers.Add(t)
        Layers.Add(t)
        Z = 47
        Select Case Layers(Z).OGTEX
            Case 1
                MapName = "LAYER1"
            Case 2
                MapName = "LAYER2"
            Case 3
                MapName = "LAYER3"
        End Select
        MapChange()
        Z += 1
        Select Case Layers(Z).OGTEX
            Case 1
                MapName = "LAYER1"
            Case 2
                MapName = "LAYER2"
            Case 3
                MapName = "LAYER3"
        End Select
        MapChange()
    End Sub
    Dim PlayerPos As Point
    Dim playerVel As Point
    Dim spawnpoint As Point = New Point(-60, -3500)
    Dim offset As Point = spawnpoint
    Dim map(Textures.Map1.Width * Textures.Map1.Height - 1) As Color
    Dim mapprev(Textures.Map1.Width * Textures.Map1.Height - 1) As Color
    Dim dircheck As Rectangle
    Dim PlayerSize As Point = New Point(32, 86)
    Dim gravity As Single = 1.3F
    Dim LatSpeed As Single = 6.0F
    Dim MapName As String = "LAYER1"
    Dim Mapsize As Point = New Point(Textures.Map1.Width, Textures.Map1.Height)
    Dim MapTex As Texture2D
    Dim IgnoreFall As Boolean = False
    Dim JumpSpeed As Single = -15.0F
    Dim Layers As List(Of TexLayer)
    Dim Z As Integer
    Public Overrides Sub HandleInput()
        If Input.KeyPressed(Keys.W) Then
            If Collide(map, offset, 0, PlayerSize, 3) Then
                Z -= 1
                Select Case Layers(Z).OGTEX
                    Case 1
                        MapName = "LAYER1"
                    Case 2
                        MapName = "LAYER2"
                    Case 3
                        MapName = "LAYER3"
                End Select
                MapChange()
            End If
        End If
        If Input.KeyPressed(Keys.Space) And dircheck.X = 1 And dircheck.Width = 0 And playerVel.Y >= 0 And playerVel.Y = 0 Then
            offset.Y += 1
            playerVel.Y = JumpSpeed
            offset.Y += 1
        End If
        If Input.KeyDown(Keys.A) And dircheck.Y = 1 And offset.X < -1 * PlayerSize.X Then
            playerVel.X = LatSpeed
        End If
        If Input.KeyDown(Keys.D) And dircheck.Height = 1 And offset.X > -4095 Then
            playerVel.X = -LatSpeed
        End If
        If Input.KeyPressed(Keys.S) Then
            If Collide(mapprev, offset, 0, PlayerSize, 3) Then
                Z += 1
                Select Case Layers(Z).OGTEX
                    Case 1
                        MapName = "LAYER1"
                    Case 2
                        MapName = "LAYER2"
                    Case 3
                        MapName = "LAYER3"
                End Select
                MapChange()
            ElseIf Input.KeyDown(Keys.S) Then
                IgnoreFall = True
            Else
                IgnoreFall = False
            End If
        End If
        If Input.KeyPressed(Keys.O) Then
            PlayerSize.X *= 2
            PlayerSize.Y *= 2
        End If
        If Input.KeyPressed(Keys.P) Then
            PlayerSize.X /= 2
            PlayerSize.Y /= 2
        End If
        If Input.KeyPressed(Keys.G) Then
            Console.WriteLine(map(47 * Textures.Map.Width + 88).ToString)
        End If
    End Sub
    Public Overrides Sub Update()
        AniTime += Globals.GameTime.ElapsedGameTime.TotalMilliseconds
        If AniTime > 2 Then
            AniTime = 0

        End If
        dircheck = New Rectangle(0, 0, 0, 0)
        If Collide(map, offset, 0, PlayerSize, 0) Then
            dircheck.X = 1
        Else
            playerVel.Y = 0
        End If
        If Collide(map, offset, 1, PlayerSize, 0) Then
            dircheck.Y = 1
        End If
        If Collide(map, offset, 2, PlayerSize, 0) Then
            dircheck.Width = 1
            If playerVel.Y < 20 Then playerVel.Y += gravity
        Else
            playerVel.Y = 0
        End If
        If Collide(map, offset, 3, PlayerSize, 0) Then
            dircheck.Height = 1
        End If
        If Collide(map, offset, 2, PlayerSize, 1) Then
            offset = spawnpoint
        End If
        If Collide(map, offset, 0, PlayerSize, 2) Then
            playerVel.Y = -20
        End If
        If Collide(map, offset, 2, PlayerSize, 2) Then
            playerVel.Y = -20
        End If
        If Collide(map, offset, 2, PlayerSize, 4) Then
            dircheck.Width = 1
            playerVel.Y = 0
            If playerVel.X <> 0 Then playerVel.Y = -4
            offset.Y += GroundGap - 1
        End If
        offset.Y -= playerVel.Y
        offset.X += playerVel.X
        playerVel.X = 0
    End Sub
    Public Overrides Sub Draw()
        For q As Integer = 0 To Layers.Count - 1
            If q < Z Then
                Dim cln As Integer = 230 - (q * 2)
                If cln < 0 Then cln = 0
                Globals.SpriteBatch.Draw(Layers(q).IMG, New Rectangle(offset.X + Globals.GameSize.X / 2 + PlayerSize.X / 2, offset.Y + Globals.GameSize.Y / 2 + PlayerSize.Y / 2, Mapsize.X, Mapsize.Y), New Color(cln, cln, cln, 255))
            ElseIf q = z Then
                Globals.SpriteBatch.Draw(Layers(q).IMG, New Rectangle(offset.X + Globals.GameSize.X / 2 + PlayerSize.X / 2, offset.Y + Globals.GameSize.Y / 2 + PlayerSize.Y / 2, Mapsize.X, Mapsize.Y), Color.White)
            ElseIf q = Z + 1 Then
                Globals.SpriteBatch.Draw(Layers(q).IMG, New Rectangle((offset.X * 1.25) + Globals.GameSize.X / 2 + PlayerSize.X / 2, 100 + (offset.Y * 1.25) + Globals.GameSize.Y / 2 + PlayerSize.Y / 2, Mapsize.X * 1.25, Mapsize.Y * 1.25), Color.White * 0.2)
            End If
        Next

        ' Globals.SpriteBatch.Draw(MapTex, New Rectangle((offset.X * 0.9) + Globals.GameSize.X / 2 + PlayerSize.X / 2, -40 + (offset.Y * 0.9) + Globals.GameSize.Y / 2 + PlayerSize.Y / 2, Mapsize.X * 0.9, Mapsize.Y * 0.9), New Color(128, 128, 128, 128))

        'Globals.SpriteBatch.Draw(MapTex, New Rectangle(offset.X + Globals.GameSize.X / 2 + PlayerSize.X / 2, offset.Y + Globals.GameSize.Y / 2 + PlayerSize.Y / 2, Mapsize.X, Mapsize.Y), Color.White)


        'Globals.SpriteBatch.Draw(MapTex, New Rectangle((offset.X * 1.25) + Globals.GameSize.X / 2 + PlayerSize.X / 2, 100 + (offset.Y * 1.25) + Globals.GameSize.Y / 2 + PlayerSize.Y / 2, Mapsize.X * 1.25, Mapsize.Y * 1.25), Color.White * 0.3)
        ' Globals.SpriteBatch.Draw(Textures.Map2_IMG, New Rectangle(offset.X + Globals.GameSize.X / 2 + PlayerSize.X / 2, offset.Y + Globals.GameSize.Y / 2 + PlayerSize.Y / 2, Mapsize.X, Mapsize.Y), Color.White)


        Globals.SpriteBatch.Draw(Textures.Null, New Rectangle(Globals.GameSize.X / 2 - PlayerSize.X / 2, Globals.GameSize.Y / 2 - PlayerSize.Y / 2, PlayerSize.X, PlayerSize.Y), Color.Cyan)
        Globals.SpriteBatch.DrawString(Fonts.Arial_8, offset.ToString & vbCrLf & offset.X Mod 2, New Vector2(0, 0), Color.Red)
    End Sub
    Dim GroundGap As Integer = 0
    Public Function Collide(mapsel() As Color, ByVal pos As Point, ByVal dir As Integer, ByVal size As Point, ByVal goal As Integer) As Boolean
        Dim sideclear As Boolean = True
        Dim death As Boolean = False
        Select Case goal
            Case 0 'Ground
                Select Case dir
                    Case 0 'Up
                        For q As Integer = 0 To size.X - 1
                            For w As Integer = 0 To Math.Abs(playerVel.Y)
                                If col(mapsel((-offset.Y - (size.Y) - w) * Textures.Map.Width + -offset.X - (size.X) + q)) = 1 Then
                                    playerVel.Y = -w
                                    If playerVel.Y = 0 Then
                                        sideclear = False
                                        Exit Function
                                    End If
                                End If
                            Next
                        Next
                        If col(mapsel((-offset.Y - (size.Y)) * Textures.Map.Width + -offset.X - (size.X / 2))) = 1 Then
                            offset.Y -= 1
                        End If
                    Case 1 'Left
                        If col(mapsel((-offset.Y - 1) * Textures.Map.Width + -offset.X - (size.X))) = 1 And col(mapsel((-offset.Y - 1 - size.Y) * Textures.Map.Width + -offset.X - (size.X))) = 0 Then
                            offset.Y += 1
                        End If
                        For q As Integer = 0 To size.Y - 1
                            For w As Integer = 0 To playerVel.X
                                If col(mapsel((-offset.Y - (size.Y) + q) * Textures.Map.Width + -offset.X - (size.X) - w - 1)) = 1 Then
                                    If q = size.Y - 1 And sideclear Then
                                    Else
                                        sideclear = False
                                        Exit Function
                                    End If
                                End If
                                If col(mapsel((-offset.Y - (size.Y) + q) * Textures.Map.Width + -offset.X - (size.X) - w - 1)) = 4 Then
                                    MapChange()
                                    Exit Function
                                End If
                            Next
                        Next
                        If col(mapsel((-offset.Y - (size.Y)) * Textures.Map.Width + -offset.X - (size.X))) = 1 Then
                            offset.X -= 1
                        End If
                    Case 2 'Down
                        For q As Integer = 0 To size.X - 1
                            For w As Integer = 0 To playerVel.Y
                                Select Case col(mapsel((-offset.Y + w) * Textures.Map.Width + -offset.X - (size.X) + q))
                                    Case 1
                                        sideclear = False
                                        Exit For
                                    Case 3
                                        If Not IgnoreFall Then
                                            sideclear = False
                                            Exit For
                                        End If
                                    Case 7

                                End Select
                                'If col(mapsel((-offset.Y + w) * Textures.Map.Width + -offset.X - (size.X) + q)) = 1 Then
                                '    sideclear = False
                                '    Exit For
                                'End If
                                'If Not IgnoreFall Then
                                '    If col(mapsel((-offset.Y + w) * Textures.Map.Width + -offset.X - (size.X) + q)) = 3 Then
                                '        sideclear = False
                                '        Exit For
                                '    End If
                                'End If
                            Next
                        Next
                    Case 3 'Right
                        If col(mapsel((-offset.Y - 1) * Textures.Map.Width + -offset.X - 1)) = 1 And col(mapsel((-offset.Y - 1 - size.Y) * Textures.Map.Width + -offset.X)) = 0 Then
                            offset.Y += 1
                        End If
                        For q As Integer = 0 To size.Y - 1
                            For w As Integer = 0 To Math.Abs(playerVel.X)
                                If col(mapsel((-offset.Y - size.Y + q) * Textures.Map.Width + -offset.X + w)) = 1 Then
                                    If q = size.Y - 1 And sideclear Then
                                    Else
                                        sideclear = False
                                        Exit For
                                    End If
                                End If
                                If col(mapsel((-offset.Y - size.Y + q) * Textures.Map.Width + -offset.X + w)) = 4 Then
                                    MapChange()
                                    Exit Function
                                End If
                            Next
                        Next
                        If col(mapsel((-offset.Y - (size.Y)) * Textures.Map.Width + -offset.X - 1)) = 1 Then
                            offset.X += 1
                        End If
                End Select
                Return sideclear
            Case 1 'Water
                Select Case dir
                    Case 2
                        For q As Integer = 0 To size.X - 1
                            For w As Integer = 0 To playerVel.Y
                                If col(mapsel((-offset.Y + w - (size.Y / 2)) * Textures.Map.Width + -offset.X - (size.X) + q)) = 2 Then
                                    death = True
                                    Exit For
                                End If
                            Next
                        Next
                End Select
                Return death
            Case 2 ' Elevate material
                Select Case dir
                    Case 0 'Up
                        If col(mapsel((-offset.Y - (size.Y)) * Textures.Map.Width + -offset.X - (size.X / 2))) = 3 Then
                            Return True
                        End If
                    Case 2 'Down
                        For q As Integer = 0 To size.X - 1
                            For w As Integer = 0 To playerVel.Y
                                If Not IgnoreFall Then
                                    If col(mapsel((-offset.Y + w) * Textures.Map.Width + -offset.X - (size.X) + q)) = 3 Then
                                        Return True
                                        Exit Function
                                    End If
                                Else
                                    If col(mapsel((-offset.Y + w) * Textures.Map.Width + -offset.X - (size.X) + q)) = 3 Then
                                        Return False
                                        Exit Function
                                    End If
                                End If
                            Next
                        Next
                End Select
            Case 3 'Transparent
                For q As Integer = 0 To size.X - 1
                    For w As Integer = 0 To Math.Abs(playerVel.Y)
                        If col(mapsel((-offset.Y - (size.Y / 2) - w) * Textures.Map.Width + -offset.X - (size.X) + q)) = 5 Then
                            Return True
                            Exit Function
                        End If
                    Next
                Next
            Case 4 ' Quicksand
                For q As Integer = 0 To size.X - 1
                    For w As Integer = 0 To 1 'size.Y
                        If col(mapsel((-offset.Y + w) * Textures.Map.Width + -offset.X - (size.X) + q)) = 6 Then
                            GroundGap = w
                            Return True
                        End If
                    Next
                Next

                'If Not IgnoreFall Then
                '    For q As Integer = 0 To size.X - 1
                '        For w As Integer = 0 To 0
                '            If col(map((-offset.Y - (size.Y) - w) * Textures.Map.Width + -offset.X - (size.X) + q)) = 5 Then
                '                Return True
                '            End If
                '        Next
                '    Next
                'End If
        End Select
    End Function
    Public Function col(ByVal c As Color)
        Select Case c
            Case New Color(0, 38, 255) 'Almost Blue False Water
                Return 0
            Case Color.White 'Background
                Return 0
            Case Color.Blue 'Water
                Return 2
            Case Color.Black 'Ground Flat
                Return 1
            Case Color.Red 'Elevate
                Return 3
            Case Color.Yellow ' Quicksand
                Return 6
            Case New Color(255, 216, 0) 'Paint.NET yellow
                Return 4
            Case New Color(0, 0, 0, 0) 'Transparent
                Return 5
            Case New Color(127, 51, 0) 'Brown Ground
                Return 7
        End Select
    End Function
    Sub MapChange()
        Layers(Z + 1).IMG.GetData(Of Color)(mapprev)
        Select Case MapName
            Case "START"
                MapName = "LEVEL1"
                MapTex = Textures.Map
                Mapsize = New Point(Textures.Map.Width, Textures.Map.Height)
                ReDim map(Mapsize.X * Mapsize.Y - 1)
                MapTex.GetData(Of Color)(map)
                offset = spawnpoint
            Case "LEVEL1"
                MapName = "DW"
                MapTex = Textures.Map1
                Mapsize = New Point(Textures.Map1.Width, Textures.Map1.Height)
                ReDim map(Mapsize.X * Mapsize.Y - 1)
                MapTex.GetData(Of Color)(map)
                PlayerPos = New Point(100, 100)
                offset = spawnpoint
            Case "LAYER1"
                MapName = "LAYER1"
                MapTex = Textures.Layer1
                Mapsize = New Point(Textures.Layer1.Width, Textures.Layer1.Height)
                ReDim map(Mapsize.X * Mapsize.Y - 1)
                MapTex.GetData(Of Color)(map)
                'offset = spawnpoint
            Case "LAYER2"
                MapName = "LAYER2"
                MapTex = Textures.Layer2
                Mapsize = New Point(Textures.Layer2.Width, Textures.Layer2.Height)
                ReDim map(Mapsize.X * Mapsize.Y - 1)
                MapTex.GetData(Of Color)(map)
               ' offset = spawnpoint
            Case "LAYER3"
                MapName = "LAYER3"
                MapTex = Textures.Layer3
                Mapsize = New Point(Textures.Layer3.Width, Textures.Layer3.Height)
                ReDim map(Mapsize.X * Mapsize.Y - 1)
                MapTex.GetData(Of Color)(map)
                'offset = spawnpoint
        End Select
    End Sub
End Class