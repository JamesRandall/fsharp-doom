module OpenGl.Texture

open System
open FSharpDoom.Image
open FSharpDoom.OpenGl.Rgba
open Silk.NET.OpenGL
//open SixLabors.ImageSharp
//open SixLabors.ImageSharp.PixelFormats
//open SixLabors.ImageSharp.Processing
open Microsoft.FSharp.NativeInterop
open SixLabors.ImageSharp.PixelFormats

exception TextureException

type T =
  { Gl: GL
    Handle: uint32
  }

let bindWithSlot (slot:TextureUnit) texture =
  texture.Gl.ActiveTexture slot
  texture.Gl.BindTexture (TextureTarget.Texture2D, texture.Handle)

let bind = bindWithSlot TextureUnit.Texture0

let createWithImage (gl:GL) (img:Image) =
  //img.Mutate(fun x -> x.Flip FlipMode.Vertical |> ignore)
  let width = img.Width |> uint
  let height = img.Height |> uint
  let handle = gl.GenTexture()
  let texture = { Gl = gl ; Handle = handle }
  texture |> bind
  use ptr = fixed img.Data
  let voidPtr = ptr |> NativePtr.toVoidPtr
  gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba |> int, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, voidPtr)
  gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, GLEnum.Repeat |> int)
  gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, GLEnum.Repeat |> int)
  // using nearest texture scaling gives us nice blocky scaling - combined with multiples for zoom and this looks
  // pretty authentic
  gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, GLEnum.Nearest |> int)
  gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, GLEnum.Nearest |> int)
  gl.GenerateMipmap TextureTarget.Texture2D
  texture
    
let create (gl:GL) (path:string) =
  let sourceImage = SixLabors.ImageSharp.Image.Load(path) :?> SixLabors.ImageSharp.Image<Rgba32>
  let mutable pixelSpan:Span<Rgba32> = Span<Rgba32>()
  if sourceImage.TryGetSinglePixelSpan &pixelSpan then
    let rgba32Array = pixelSpan.ToArray()
    let rgbaArray =
      rgba32Array
      |> Array.map(fun src -> { R = src.R ; G = src.G ; B = src.B ; A = src.A })
    createWithImage gl { Data = rgbaArray ; Width = sourceImage.Width ; Height = sourceImage.Height }
  else
    raise TextureException
    
let dispose texture = texture.Gl.DeleteTexture texture.Handle