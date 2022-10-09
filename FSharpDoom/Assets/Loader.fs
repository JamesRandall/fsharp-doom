module Assets.Loader

open System
open FSharpDoom.OpenGl.Rgba

exception WadLoaderException of string

type LumpEntry =
  { Offset: int
    Size: int
    Name: string
  }
  
type DoomImage =
  { Width: int
    Height: int
    Left: int
    Top: int
    Columns: int array array
  }
  
type Palette =
  { Colors: Rgba array }

let getString offset maxLength (bytes:byte array) =
  bytes
  |> Array.skip(offset)
  |> Array.take(maxLength)
  |> Array.takeWhile(fun item -> item <> 0uy)
  |> System.Text.Encoding.ASCII.GetString
  
let getInt32 offset bytes =
  BitConverter.ToInt32(bytes, offset)
  
let getInt16 offset bytes =
  BitConverter.ToInt16(bytes, offset)

let loadPalettes (wad:byte array) lump =
  let paletteEntries = 256
  let paletteSize = paletteEntries * 3
  let numberOfPalettes = lump.Size / paletteSize
  {0..numberOfPalettes-1}
  |> Seq.map(fun paletteIndex ->
    let offset = lump.Offset + (paletteIndex * paletteSize)
    { Colors =
        {0..paletteEntries-1}
        |> Seq.map(fun entryIndex ->
          let entryOffset = offset + entryIndex * 3
          { R = wad[entryOffset] ; G = wad[entryOffset+1] ; B = wad[entryOffset+2] ; A = 0xFFuy }
        )
        |> Seq.toArray
    }
  )
  |> Seq.toArray

let loadDoomImage wad lump =
  let offset = lump.Offset
  let width = wad |> getInt16 offset |> int
  let height = wad |> getInt16 (offset+2) |> int
  let left = wad |> getInt16 (offset+4) |> int
  let top = wad |> getInt16 (offset+6) |> int
  
  let columnIndexes =
    {0..width-1}
    |> Seq.map(fun ci ->
      offset + (getInt32 (offset+8 + 4*ci) wad)
    )
    |> Seq.toArray
    
  let columns =
    columnIndexes
    |> Array.map(fun columnOffset ->
      // TODO: make this part of the fold - i.e. do not mutate the column array
      let column = Array.create height Constants.transparent
      
      Seq.initInfinite id
      |> Seq.scan(fun (postOffset,shouldContinue) _ ->
        let row = wad[postOffset] |> int
        if row = 0xFF then
          (postOffset,false)
        else
          let postHeight = wad[postOffset+1] |> int
          {0..postHeight-1}
          |> Seq.iter(fun postIndex ->
            column[row+postIndex] <- wad[postOffset+3+postIndex] |> int
          )
          // the offset moves on by the row byte, the height byte, a dummy byte before the pixels, a dummy byte after
          // the pixels and the number of pixels in the post (postHeight)
          (postOffset+4+postHeight,shouldContinue)
      ) (columnOffset,true)
      |> Seq.takeWhile snd
      |> Seq.toList
      |> ignore
      
      column
    )
    
  { Width = width
    Height = height
    Left = left
    Top = top
    Columns = columns
  }

let load () =
  let lumpSize = 4 + 4 + 8
  let wad = System.IO.File.ReadAllBytes("Assets/DOOM1.WAD")
  let fileType = getString 0 4 wad
  if fileType <> "IWAD" then raise (WadLoaderException "Only IWAD files are supported")
  let numberOfLumps = getInt32 0x4 wad
  let directoryOffset = getInt32 0x8 wad
  let lumps =
    {0..numberOfLumps-1}
    |> Seq.map(fun index ->
      let entryOffset = directoryOffset + index * lumpSize
      { Offset = getInt32 entryOffset wad
        Size = getInt32 (entryOffset+0x4) wad
        Name = getString (entryOffset+0x8) 0x8 wad
      }
    )
    |> Seq.toArray
  
  let soldier =
    lumps
    |> Array.find(fun lump -> lump.Name = "POSSA1")
    |> loadDoomImage wad
  
  let palettes =
    lumps
    |> Array.find(fun lump -> lump.Name = "PLAYPAL")
    |> loadPalettes wad
  
  (soldier,palettes[0])
  