module Assets.Lump

open System

type LumpEntry =
  { Offset: int
    Size: int
    Name: string
  }
  
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