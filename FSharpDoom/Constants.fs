module Constants

let viewportZoom = 2
let viewportWidth = 320 * viewportZoom
let viewportHeight = 200 * viewportZoom
let zoom = 2
let windowWidth = viewportWidth * zoom
let windowHeight = viewportHeight * zoom
let transparent = -1

[<RequireQualifiedAccess>]
type SkillLevel =
  | ImTooYoungToDie
  | HeyNotTooRough
  | HurtMePlenty
  | UltraViolence
  | Nightmare