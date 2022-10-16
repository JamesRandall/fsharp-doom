// In this folder can be found a file called thingTypes.csv.
// It is basically the date from https://doom.fandom.com/wiki/Thing_types popped into a CSV via Excel.
// This tool then spits out some code for use in the game during the map loading process (the ThingType needs manually
// adjusting from the names)

open System.Globalization

let thingTypeFromDescriptionAndValue description value =
  match value with
  | "10" -> "BloodyMess1"
  | "12" -> "BloodyMess2"
  | "79" -> "PoolOfBlood1"
  | "80" -> "PoolOfBlood2"
  | _ ->
    let textInfo = CultureInfo("en-US", false).TextInfo
    textInfo
      .ToTitleCase(description)
      .Replace(" ", "")
      .Replace("-","")
      .Replace("(","")
      .Replace(")","")
      .Replace(",","")
      .Replace("\"","")
  
let thingClassListFromString (value:string) =
  (* the types we are using are as below:
  type ThingClass =
    | ArtifactItem
    | Pickup
    | Weapon
    | Monster
    | Obstacle
    | HangsOrFloats
  *)
  let content =
    value
    |> Seq.map(fun char ->
      match char with
      | 'A' -> "ArtifactItem"
      | 'P' -> "Pickup"
      | 'W' -> "Weapon"
      | 'M' -> "Monster"
      | 'O' -> "Obstacle"
      | '^' -> "HangsOrFloats"
      | _ -> ""
    )
    |> Seq.filter(fun s -> s.Length > 0)
    |> Seq.map(fun s -> $"ThingClass.{s}")
    |> String.concat ";"
  $"[{content}]"

open FSharp.Data
let csv = CsvFile.Load("thingTypes.csv")
let stringRows =
  csv.Rows
  |> Seq.map(fun row ->
    let value = row.GetColumn "Value"
    let radius = row.GetColumn "Radius"
    let sprite = row.GetColumn "Sprite"
    let sequence = row.GetColumn "Sequence"
    let thingClass = row.GetColumn "Class"
    let description = row.GetColumn "Description"
    let thingType = thingTypeFromDescriptionAndValue description value
    let thingClassList = thingClassListFromString thingClass
    
    // We want something that looks like this:
    //  | 0x7E7 -> { Type = ThingType.Berserk ; Class = Set.ofList [ ThingClass.ArtifactItem ] ; Radius = 20 ; Sprite = "BPAK" ; SpriteSequence = "A" }
    $"    | {value} -> {{ Type = ThingType.{thingType} ; Class = Set.ofList {thingClassList} ; Radius = {radius} ; Sprite = \"{sprite}\" ; SpriteSequence = \"{sequence}\" }}"
  )
  |> Seq.toArray
  |> String.concat "\n"
  
System.IO.File.WriteAllText ("output.fs", stringRows)
  


  


 