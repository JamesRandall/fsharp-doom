module Assets.Map
open Lump

exception MapLoadException of string

type Vertex =
  { X: int
    Y: int
  }
  
type Sidedef =
  { XOffset: int
    YOffset: int
    UpperTextureName: string
    LowerTextureName: string
    MiddleTextureName: string
    FacingSectorNumber: int
  }
  
[<RequireQualifiedAccess>]
type LinedefFlags =
  | BlocksPlayersAndMonsters // 0x0001
  | BlocksMonsters // 0x0002
  | TwoSided // 0x0004
  | UpperTextureUnpegged // 0x0008
  | LowerTextureUnpegged // 0x0010
  | Secret // 0x0020
  | BlocksSound // 0x0040
  | NeverShowsOnAutomap // 0x0080
  | AlwaysShowsOnAutomap // 0x0100
  
type Linedef =
  { StartVertexIndex: int
    EndVertexIndex: int
    Flags: Set<LinedefFlags>
    SpecialType: int
    SectorTag: int
    RightSidedefIndex: int
    LeftSidedefIndex: int
  }
  
[<RequireQualifiedAccess>]
type SegDirection =
  // indicates the seg runs along the right side of the linedef
  | SameAsLinedef
  // indicates the seg runs along the left side of the linedef
  | OppositeOfLinedef

type Seg =
  { StartingVertexNumber: int
    EndingVertexNumber: int
    Angle: int
    LinedefIndex: int
    Direction: SegDirection
    Offset: int
  }
  
type Subsector =
  { SegCount: int
    FirstSegIndex: int
  }
  
type NodeBoundingBox =
  { Top: int
    Bottom: int
    Left: int
    Right: int
  }
  
type Node =
  { PartitionLineX: int
    PartitionLineY: int
    EndOfPartitionLineDeltaX: int
    EndOfPartitionLineDeltaY: int
    RightBoundingBox: NodeBoundingBox
    LeftBoundingBox: NodeBoundingBox
    // The type of each child field is determined by its sign bit (bit 15). If bit 15 is zero, the child field
    // gives the node number of a subnode. If bit 15 is set, then bits 0-14 give the number of a subsector. 
    RightChildIndex: int
    LeftChildIndex: int
  }
  
type SectorType =
  | Normal
  | LightBlinkRandom
  | LightBlinkHalfSecond
  | LightBlinkOneSecond
  | LightBlinkHalfSecondTwentyPercentDamagePerSecond
  | TenPercentDamagePerSecond
  | FivePercentDamagePerSecond
  | LightOscillates
  | Secret
  | CeilingClosesLikeDoor30SecondsAfterLevelStarts
  | End
  | LightBlinkHalfSecondSynchronized
  | LightBlinkOneSecondSynchronized
  | CeilingOpensLikeADoor300SecondsAfterLevelStarts
  | TwentyPercentDamagePerSecond
  | LightFlickersRandomly
    
type Sector =
  { FloorHeight: int
    CeilingHeight: int
    FloorTexture: string
    CeilingTexture: string
    LightLevel: int
    Type: SectorType
    Tag: int
  }
  
type Map =
  { Linedefs: Linedef array
    Sidedefs: Sidedef array
    Vertexes: Vertex array
    Segs: Seg array
    Subsectors: Subsector array
    Nodes: Node array
    Sectors: Sector array
  }
    
let loadVertexes (wad:byte array) (lump:LumpEntry) =
  let vertexByteArray = wad[lump.Offset..lump.Offset+lump.Size-1]
  let vertexSize = 4
  {0..vertexSize..lump.Size-1}
  |> Seq.map(fun baseOffset ->
    { X = getInt16 baseOffset vertexByteArray |> int
      Y = getInt16 (baseOffset+2) vertexByteArray |> int
    }
  )
  |> Seq.toArray
  
let loadSidedefs (wad:byte array) (lump:LumpEntry) =
  let sidedefByteArray = wad[lump.Offset..lump.Offset+lump.Size-1]
  let xOffsetOffset = 0
  let yOffsetOffset = 2
  let upperTextureNameOffset = 4
  let lowerTextureNameOffset = 12
  let middleTextureNameOffset = 20
  let facingSectorNumberOffset = 28
  let sideDefSize = 30
  {0..sideDefSize..lump.Size-1}
  |> Seq.map(fun baseOffset ->
    { XOffset = getInt16 (baseOffset + xOffsetOffset) sidedefByteArray |> int
      YOffset = getInt16 (baseOffset + yOffsetOffset) sidedefByteArray |> int
      UpperTextureName = getString (baseOffset + upperTextureNameOffset) 8 sidedefByteArray
      LowerTextureName = getString (baseOffset + lowerTextureNameOffset) 8 sidedefByteArray
      MiddleTextureName = getString (baseOffset + middleTextureNameOffset) 8 sidedefByteArray
      FacingSectorNumber = getInt16 (baseOffset + facingSectorNumberOffset) sidedefByteArray |> int
    }
  )
  |> Seq.toArray
  
let loadLinedefs (wad:byte array) (lump:LumpEntry) =
  let linedefByteArray = wad[lump.Offset..lump.Offset+lump.Size-1]
  let startVertexIndex = 0
  let endVertexIndex = 2
  let flagsOffset = 4
  let specialType = 6
  let sectorTag = 8
  let rightSidedefIndex = 10
  let leftSidedefIndex = 12
  let linedefSize = 14
  
  let flags value =
    (if value &&& 0x0001 > 0 then [LinedefFlags.BlocksPlayersAndMonsters] else [])
    @ (if value &&& 0x0002 > 0 then [LinedefFlags.BlocksMonsters] else [])
    @ (if value &&& 0x0004 > 0 then [LinedefFlags.TwoSided] else [])
    @ (if value &&& 0x0008 > 0 then [LinedefFlags.UpperTextureUnpegged] else [])
    @ (if value &&& 0x0010 > 0 then [LinedefFlags.LowerTextureUnpegged] else [])
    @ (if value &&& 0x0020 > 0 then [LinedefFlags.Secret] else [])
    @ (if value &&& 0x0040 > 0 then [LinedefFlags.BlocksSound] else [])
    @ (if value &&& 0x0080 > 0 then [LinedefFlags.NeverShowsOnAutomap] else [])
    @ (if value &&& 0x0100 > 0 then [LinedefFlags.AlwaysShowsOnAutomap] else [])
    |> Set.ofList
  
  {0..linedefSize..lump.Size-1}
  |> Seq.map(fun baseOffset ->
    { StartVertexIndex = getInt16 (baseOffset + startVertexIndex) linedefByteArray |> int
      EndVertexIndex = getInt16 (baseOffset + endVertexIndex) linedefByteArray |> int
      Flags = getInt16 (baseOffset + flagsOffset) linedefByteArray |> int |> flags
      SpecialType = getInt16 (baseOffset + specialType) linedefByteArray |> int
      SectorTag = getInt16 (baseOffset + sectorTag) linedefByteArray |> int
      RightSidedefIndex = getInt16 (baseOffset + rightSidedefIndex) linedefByteArray |> int
      LeftSidedefIndex = getInt16 (baseOffset + leftSidedefIndex) linedefByteArray |> int
    }
  )
  |> Seq.toArray
  
let loadSegs (wad:byte array) (lump:LumpEntry) =
  let segsByteArray = wad[lump.Offset..lump.Offset+lump.Size-1]
  let startingVertexNumber = 0
  let endingVertexNumber = 2
  let angle = 4
  let linedefIndex = 6
  let direction = 8
  let offset = 10
  let segsSize = 12
  
  {0..segsSize..lump.Size-1}
  |> Seq.map(fun baseOffset ->
    { StartingVertexNumber = getInt16 (baseOffset + startingVertexNumber) segsByteArray |> int
      EndingVertexNumber = getInt16 (baseOffset + endingVertexNumber) segsByteArray |> int
      Angle = getInt16 (baseOffset + angle) segsByteArray |> int
      LinedefIndex = getInt16 (baseOffset + linedefIndex) segsByteArray |> int
      Direction = match getInt16 (baseOffset + direction) segsByteArray |> int with
                  | 0 -> SegDirection.SameAsLinedef
                  | 1 -> SegDirection.OppositeOfLinedef
                  | _ -> raise (MapLoadException "Unknown seg direction")
      Offset = getInt16 (baseOffset + offset) segsByteArray |> int
    }
  )
  |> Seq.toArray
  
let loadSubSectors (wad:byte array) (lump:LumpEntry) =
  let subSectorsByteArray = wad[lump.Offset..lump.Offset+lump.Size-1]
  let segCount = 0
  let firstSegIndex = 2
  let subSectorSize = 4
  
  {0..subSectorSize..lump.Size-1}
  |> Seq.map(fun baseOffset ->
    { SegCount = getInt16 (baseOffset + segCount) subSectorsByteArray |> int
      FirstSegIndex = getInt16 (baseOffset + firstSegIndex) subSectorsByteArray |> int
    }
  )
  |> Seq.toArray
  
let loadNodes (wad:byte array) (lump:LumpEntry) =
  let nodesByteArray = wad[lump.Offset..lump.Offset+lump.Size-1]
  let partitionLineX = 0
  let partitionLineY = 2
  let endOfPartitionLineDeltaX = 4
  let endOfPartitionLineDeltaY = 6
  let rightBoundingBox = 8
  let leftBoundingBox = 16
  let rightChildIndex = 24
  let leftChildIndex = 26
  let nodeSize = 28
  
  {0..nodeSize..lump.Size-1}
  |> Seq.map(fun baseOffset ->
    { PartitionLineX = getInt16 (baseOffset + partitionLineX) nodesByteArray |> int
      PartitionLineY = getInt16 (baseOffset + partitionLineY) nodesByteArray |> int      
      EndOfPartitionLineDeltaX = getInt16 (baseOffset + endOfPartitionLineDeltaX) nodesByteArray |> int
      EndOfPartitionLineDeltaY = getInt16 (baseOffset + endOfPartitionLineDeltaY) nodesByteArray |> int
      RightBoundingBox = {
        Top = getInt16 (baseOffset + rightBoundingBox) nodesByteArray |> int
        Bottom = getInt16 (baseOffset + rightBoundingBox + 2) nodesByteArray |> int
        Left = getInt16 (baseOffset + rightBoundingBox + 4) nodesByteArray |> int
        Right = getInt16 (baseOffset + rightBoundingBox + 6) nodesByteArray |> int
      }
      LeftBoundingBox= {
        Top = getInt16 (baseOffset + leftBoundingBox) nodesByteArray |> int
        Bottom = getInt16 (baseOffset + leftBoundingBox + 2) nodesByteArray |> int
        Left = getInt16 (baseOffset + leftBoundingBox + 4) nodesByteArray |> int
        Right = getInt16 (baseOffset + leftBoundingBox + 6) nodesByteArray |> int
      }
      RightChildIndex = getInt16 (baseOffset + rightChildIndex) nodesByteArray |> int
      LeftChildIndex = getInt16 (baseOffset + leftChildIndex) nodesByteArray |> int
    }
  )
  |> Seq.toArray
  
let loadSectors (wad:byte array) (lump:LumpEntry) =
  let sectorByteArray = wad[lump.Offset..lump.Offset+lump.Size-1]
  let floorHeight = 0
  let ceilingHeight = 2
  let floorTexture = 4
  let ceilingTexture = 12
  let lightLevel = 20
  let sectorTypeOffset = 22
  let tag = 24
  let sectorSize = 26
  
  let sectorType value =
    match value with
    | 0 -> Normal
    | 1 -> LightBlinkRandom
    | 2 -> LightBlinkHalfSecond
    | 3 -> LightBlinkOneSecond
    | 4 -> LightBlinkHalfSecondTwentyPercentDamagePerSecond
    | 5 -> TenPercentDamagePerSecond
    | 7 -> FivePercentDamagePerSecond
    | 8 -> LightOscillates
    | 9 -> Secret
    | 10 -> CeilingClosesLikeDoor30SecondsAfterLevelStarts
    | 11 -> End
    | 12 -> LightBlinkHalfSecondSynchronized
    | 13 -> LightBlinkOneSecondSynchronized
    | 14 -> CeilingOpensLikeADoor300SecondsAfterLevelStarts
    | 16 -> TwentyPercentDamagePerSecond
    | 17 -> LightFlickersRandomly
    | _ -> raise (MapLoadException "Unrecognised sector type")
  
  {0..sectorSize..lump.Size-1}
  |> Seq.map(fun baseOffset ->
    { FloorHeight = getInt16 (baseOffset + floorHeight) sectorByteArray |> int
      CeilingHeight = getInt16 (baseOffset + ceilingHeight) sectorByteArray |> int
      FloorTexture = getString (baseOffset + floorTexture) 8 sectorByteArray
      CeilingTexture = getString (baseOffset + ceilingTexture) 8 sectorByteArray
      LightLevel = getInt16 (baseOffset + lightLevel) sectorByteArray |> int
      Type = getInt16 (baseOffset + sectorTypeOffset) sectorByteArray |> int |> sectorType
      Tag = getInt16 (baseOffset + tag) sectorByteArray |> int
    }
  )
  |> Seq.toArray
  
let loadMap (wad:byte array) (lumps:LumpEntry array) =
  { Linedefs = loadLinedefs wad (lumps |> Array.find(fun l -> l.Name = "LINEDEFS"))
    Sidedefs = loadSidedefs wad (lumps |> Array.find(fun l -> l.Name = "SIDEDEFS"))
    Vertexes = loadVertexes wad (lumps |> Array.find(fun l -> l.Name = "VERTEXES"))
    Segs = loadSegs wad (lumps |> Array.find(fun l -> l.Name = "SEGS"))
    Subsectors = loadSubSectors wad (lumps |> Array.find(fun l -> l.Name = "SSECTORS"))
    Nodes = loadNodes wad (lumps |> Array.find(fun l -> l.Name = "NODES"))
    Sectors = loadSectors wad (lumps |> Array.find(fun l -> l.Name = "SECTORS"))
  }