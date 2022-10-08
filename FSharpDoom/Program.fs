// For more information see https://aka.ms/fsharp-console-apps
open FSharpDoom.Image
open Silk.NET.Maths
open Silk.NET.Windowing
open Constants
open OpenGl
open System.Numerics

type RenderData =
  { Gl: Silk.NET.OpenGL.GL
    ScreenImage: Image
    SpriteRenderer: Vector2 -> Vector2 -> Texture.T -> unit
  }

let mutable renderDataOption = None //:Option<RenderData>

let render (frameTime:double) =
  match renderDataOption with
  | Some rd ->
    let texture = Texture.createWithImage rd.Gl rd.ScreenImage 
    let vSpriteSize = System.Numerics.Vector2(rd.ScreenImage.Width |> float32, rd.ScreenImage.Height |> float32)
    rd.SpriteRenderer (Vector2(0.0f,0.0f)) vSpriteSize texture
    texture |> Texture.dispose
  | None ->
    () // still loading
  
let load (window:IWindow) _ =
  let gl = Silk.NET.OpenGL.GL.GetApi(window)  
  renderDataOption <- Some
    { Gl = gl
      ScreenImage = Image.Create Constants.windowWidth Constants.windowHeight
      SpriteRenderer = SpriteRenderer.create gl (float32 Constants.windowWidth) (float32 Constants.windowHeight)
    }

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
