module Timer

open System.Diagnostics

let create () =
  let collectedTimes = Array.create 100 0.0
  let mutable recordingIndex = 0
  let start () =
    let sw = Stopwatch.StartNew()
    let stop () =
      sw.Stop()
      collectedTimes[recordingIndex] <- float sw.ElapsedTicks
      recordingIndex <- if recordingIndex = collectedTimes.Length-1 then 0 else recordingIndex + 1
    stop
  let average () = collectedTimes |> Seq.average
  start,average