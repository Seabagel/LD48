Public Class Textures
    'Public Shared texture As Texture2D
    Public Shared Null As Texture2D
    Public Shared Map As Texture2D
    Public Shared Map1 As Texture2D
    Public Shared Map2 As Texture2D
    Public Shared Map2_IMG As Texture2D
    Public Shared Layer1 As Texture2D
    Public Shared Layer2 As Texture2D
    Public Shared Layer3 As Texture2D
    Public Shared Sub Load()
        'texture = Globals.Content.Load(Of Texture2D)("GFX/name")
        Null = Globals.Content.Load(Of Texture2D)("menuscreen")
        Map = Globals.Content.Load(Of Texture2D)("Map")
        Map1 = Globals.Content.Load(Of Texture2D)("Map1")
        Map2 = Globals.Content.Load(Of Texture2D)("Map2")
        Map2_IMG = Globals.Content.Load(Of Texture2D)("Map2_IMG")
        Layer1 = Globals.Content.Load(Of Texture2D)("Layer1")
        Layer2 = Globals.Content.Load(Of Texture2D)("Layer2")
        Layer3 = Globals.Content.Load(Of Texture2D)("Layer3")
    End Sub
End Class
