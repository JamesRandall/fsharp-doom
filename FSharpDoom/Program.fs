open Assets.Loader
open FSharpDoom
open FSharpDoom.Image
open Silk.NET.Maths
open Silk.NET.Windowing
open Constants

open System.Numerics

type RenderData =
  { Gl: Silk.NET.OpenGL.GL
    ScreenImage: Image
    SpriteRenderer: Vector2 -> Vector2 -> OpenGl.Texture.T -> unit
    TestSprite: DoomImage*Palette
    Level: Assets.Loader.Level
  }

let mutable renderDataOption = None

let render (frameTime:double) =
  match renderDataOption with
  | Some rd ->
    let doomImage,palette = rd.TestSprite

    
    //RenderTest.renderDoomImage (0,0) Constants.viewportZoom rd.ScreenImage palette doomImage
    Renderer.Map.render rd.ScreenImage rd.Level
    let texture = OpenGl.Texture.createWithImage rd.Gl rd.ScreenImage
    let vSpriteSize = System.Numerics.Vector2(Constants.windowWidth |> float32, Constants.windowHeight |> float32)
    rd.SpriteRenderer (Vector2(0.0f,0.0f)) vSpriteSize texture
    texture |> OpenGl.Texture.dispose    
        
  | None ->
    () // still loading
  
let load (window:IWindow) _ =
  let gl = Silk.NET.OpenGL.GL.GetApi(window)
  
  let testSprite = Assets.Loader.load ()
  let level = Assets.Loader.loadLevel 1 1
  renderDataOption <- Some
    { Gl = gl
      ScreenImage = Image.Create Constants.viewportWidth Constants.viewportHeight
      SpriteRenderer = OpenGl.SpriteRenderer.create gl (float32 Constants.windowWidth) (float32 Constants.windowHeight)
      TestSprite = testSprite
      Level = level
    }
  ()

[<EntryPoint>]
let main _ =
  let mutable options = WindowOptions.Default
  options.Size <- Vector2D(windowWidth,windowHeight)
  options.Title <- "F# Doom"
  let window = Window.Create(options)
  window.add_Load (load window)
  window.add_Render render
  window.Run ()
  0
