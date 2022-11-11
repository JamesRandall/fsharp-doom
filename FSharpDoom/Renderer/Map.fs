module Renderer.Map
open Microsoft.FSharp.NativeInterop
open FSharpDoom.OpenGl.Rgba

// Bresenham line
// https://en.wikipedia.org/wiki/Bresenham's_line_algorithm
// TODO: Doesn't currently check we are in the bounds of the buffer
let line (buffer:nativeptr<Rgba>) bufferWidth color x1 y1 x2 y2 =
  let w = x2-x1
  let h = y2-y1
  let absWidth = abs w
  let absHeight = abs h
  let dx1 = if w < 0 then -1 elif w > 0 then 1 else 0
  let dy1 = if h < 0 then -1 elif h > 0 then 1 else 0
  let dx2 =
    if not (absWidth > absHeight) then
      0
    else
      if w < 0 then -1 elif w > 0 then 1 else 0
  let dy2 =
    if not (absWidth > absHeight) then
      if h < 0 then -1 elif h > 0 then 1 else 0
    else
      0        
  let longest,shortest = if absWidth > absHeight then absWidth,absHeight else absHeight,absWidth
  
  {0..longest}
  |> Seq.fold(fun (x,y,numerator) _ ->
      NativePtr.set buffer (y*bufferWidth+x) color
      let newNumerator = numerator + shortest
      if not (numerator < longest) then
        x+dx1,y+dy1,newNumerator-longest
      else
        x+dx2,y+dy2,newNumerator
  ) (x1,y1,longest >>> 1)
  |> ignore
  

let render (screen:FSharpDoom.Image.Image) (level:Assets.Loader.Level) =
  let left,top,right,bottom =
    level.Map.Linedefs
    |> Array.fold(fun (left,top,right,bottom) lineDef ->
      let startVertex = level.Map.Vertexes.[lineDef.StartVertexIndex]
      let endVertex = level.Map.Vertexes.[lineDef.StartVertexIndex]
      let minX = min startVertex.X endVertex.X
      let maxX = max startVertex.X endVertex.X
      let minY = min startVertex.Y endVertex.Y
      let maxY = max startVertex.Y endVertex.Y
      (
        min left minX,
        min top minY,
        max right maxX,
        max bottom maxY
      )
    ) (System.Int32.MaxValue, System.Int32.MaxValue, System.Int32.MinValue, System.Int32.MinValue)
  let width = right - left
  let height = bottom - top
  let scale = min (float (screen.Width-2) / float width) (float (screen.Height-2) / float height)
  
  use screenPtr = fixed screen.Data
  let drawLine = line screenPtr screen.Width
  
  let lineDefColor = { R = 255uy ; G = 255uy ; B = 255uy ; A = 255uy }
  let playerColor = { R = 255uy ; G = 0uy ; B = 0uy ; A = 255uy }
  //drawLine color 100 100 300 350
  level.Map.Linedefs
  |> Array.iter (fun lineDef ->
    let startVertex = level.Map.Vertexes[lineDef.StartVertexIndex]
    let endVertex = level.Map.Vertexes[lineDef.EndVertexIndex]
    
    let x1 = float (startVertex.X - left) * scale |> int
    let y1 = screen.Height - 1 - (float (startVertex.Y - top) * scale |> int)
    let x2 = float (endVertex.X - left) * scale |> int
    let y2 = screen.Height - 1 - (float (endVertex.Y - top) * scale |> int)
    
    drawLine lineDefColor x1 y1 x2 y2
  )
  
  let px = float (level.PlayerStart.X - left) * scale |> int
  let py = screen.Height - (float (level.PlayerStart.Y - top) * scale |> int)
  drawLine playerColor px (py - 3) px (py + 3)
  drawLine playerColor (px - 3) py (px + 3) py
    