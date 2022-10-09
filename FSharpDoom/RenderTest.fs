module FSharpDoom.RenderTest

open Assets.Loader
open Microsoft.FSharp.NativeInterop

let renderDoomImage (x,y) scale (screen:FSharpDoom.Image.Image) (palette:Palette) (sprite:DoomImage) =
  use screenPtr = fixed screen.Data
  {0..sprite.Width-1}
  |> Seq.iter(fun spriteX ->
    let column = sprite.Columns[spriteX]
    {0..sprite.Height-1}
    |> Seq.iter(fun spriteY ->
      match column[spriteY] with
      | None -> ()
      | Some colorIndex ->
        let color = palette.Colors[colorIndex]
        let screenX = spriteX*scale + x
        let screenY = spriteY*scale + y
        {screenX..screenX+scale-1}
        |> Seq.iter(fun outX ->
          {screenY..screenY+scale-1}
          |> Seq.iter(fun outY ->
            NativePtr.set screenPtr (outY*screen.Width+outX) color
          )
        )
    )
  )
