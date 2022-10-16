module Assets.Thing
open Assets.Lump
open Assets.MapPosition
open Constants
exception ThingLoaderException of string

[<RequireQualifiedAccess>]
type ThingType =
  // Artifacts
  | Berserk
  | ComputerMap
  | HealthPotion
  | Invisibility
  | Invulnerability
  | LightAmplificationVisor
  | Megasphere
  | SoulSphere
  | SpiritualArmor
  // Powerups
  | Backpack
  | BlueArmor
  | GreenArmor
  | Medikit
  | RadiationSuit
  | Stimpack
  // Weapons
  | BFG9000
  | Chaingun
  | Chainsaw
  | PlasmaRifle
  | RocketLauncher
  | Shotgun
  | SuperShotgun
  // Ammunition
  | AmmoClip
  | BoxOfAmmo
  | BoxOfRockets
  | BoxOfShells
  | CellCharge
  | CellChargePack
  | Rocket
  | ShotgunShells
  // Keys
  | BlueKeycard
  | BlueSkullKey
  | RedKeycard
  | RedSkullKey
  | YellowKeycard
  | YellowSkullKey
  // Monsters
  | Arachnotron
  | ArchVile
  | BaronOfHell
  | Cacodemon
  | HeavyWeaponDude
  | CommanderKeen
  | Cyberdemon
  | Demon
  | Zombieman
  | ShotgunGuy
  | HellKnight
  | Imp
  | LostSoul
  | Mancubus
  | PainElemental
  | Revenant
  | Spectre
  | SpiderMastermind
  | WolfensteinSS
  // Obstacles
  | Barrel
  | BurningBarrel
  | BurntTree
  | Candelabra
  | EvilEye
  | FiveSkullsShishKebab
  | FloatingSkull
  | FloorLamp
  | HangingLeg
  | HangingPairOfLegs
  | HangingTorsoBrainRemoved
  | HangingTorsoLookingDown
  | HangingTorsoLookingUp
  | HangingTorsoOpenSkull
  | HangingVictimGutsAndBrainRemoved
  | HangingVictimGutsRemoved
  | HangingVictimOneLegged
  | HangingVictimTwitching
  | ImpaledHuman
  | LargeBrownTree
  | PileOfSkullsAndCandles
  | ShortBlueFirestick
  | ShortGreenFirestick
  | ShortRedFirestick
  | ShortGreenPillar
  | ShortGreenPillarWithBeatingHeart
  | ShortRedPillar
  | ShortRedPillarWithSkull
  | ShortTechnoFloorLamp
  | SkullOnAPole
  | Stalagmite
  | TallBlueFirestick
  | TallGreenFirestick
  | TallGreenPillar
  | TallRedFirestick
  | TallRedPillar
  | TallTechnoFloorLamp
  | TallTechnoPillar
  | TwitchingImpaledHuman
  // Decorations
  | BloodyMess1
  | BloodyMess2
  | Candle
  | DeadCacodemon
  | DeadDemon
  | DeadFormerHuman
  | DeadFormerSergeant
  | DeadImp
  | DeadLostSoulInvisible
  | DeadPlayer
  | HangingLegDecoration
  | HangingPairOfLegsDecoration
  | HangingVictimArmsOut
  | HangingVictimOneLeggedDecoration
  | HangingVictimTwitchingDecoration
  | PoolOfBlood1
  | PoolOfBlood2
  | PoolOfBloodAndFlesh
  | PoolOfBrains
  // Other
  | BossBrain
  | DeathmatchStart
  | Player1Start
  | Player2Start
  | Player3Start
  | Player4Start
  | SpawnShooter
  | SpawnSpot
  | TeleportLanding
  
[<RequireQualifiedAccess>]
type ThingClass =
  | ArtifactItem
  | Pickup
  | Weapon
  | Monster
  | Obstacle
  | HangsOrFloats
  
type ThingDefinition =
  { Type: ThingType
    Class: Set<ThingClass>
    Radius: int
    Sprite: string
    SpriteSequence: string
  }
  with
  static member Create value =
    match value with
    | 2023 -> { Type = ThingType.Berserk ; Class = Set.ofList [ThingClass.ArtifactItem;ThingClass.Pickup] ; Radius = 20 ; Sprite = "PSTR" ; SpriteSequence = "A" }
    | 2026 -> { Type = ThingType.ComputerMap ; Class = Set.ofList [ThingClass.ArtifactItem;ThingClass.Pickup] ; Radius = 20 ; Sprite = "PMAP" ; SpriteSequence = "ABCDCB" }
    | 2014 -> { Type = ThingType.HealthPotion ; Class = Set.ofList [ThingClass.ArtifactItem;ThingClass.Pickup] ; Radius = 20 ; Sprite = "BON1" ; SpriteSequence = "ABCDCB" }
    | 2024 -> { Type = ThingType.Invisibility ; Class = Set.ofList [ThingClass.ArtifactItem;ThingClass.Pickup] ; Radius = 20 ; Sprite = "PINS" ; SpriteSequence = "ABCD" }
    | 2022 -> { Type = ThingType.Invulnerability ; Class = Set.ofList [ThingClass.ArtifactItem;ThingClass.Pickup] ; Radius = 20 ; Sprite = "PINV" ; SpriteSequence = "ABCD" }
    | 2045 -> { Type = ThingType.LightAmplificationVisor ; Class = Set.ofList [ThingClass.ArtifactItem;ThingClass.Pickup] ; Radius = 20 ; Sprite = "PVIS" ; SpriteSequence = "AB" }
    | 83 -> { Type = ThingType.Megasphere ; Class = Set.ofList [ThingClass.ArtifactItem;ThingClass.Pickup] ; Radius = 20 ; Sprite = "MEGA" ; SpriteSequence = "ABCD" }
    | 2013 -> { Type = ThingType.SoulSphere ; Class = Set.ofList [ThingClass.ArtifactItem;ThingClass.Pickup] ; Radius = 20 ; Sprite = "SOUL" ; SpriteSequence = "ABCDCB" }
    | 2015 -> { Type = ThingType.SpiritualArmor ; Class = Set.ofList [ThingClass.ArtifactItem;ThingClass.Pickup] ; Radius = 20 ; Sprite = "BON2" ; SpriteSequence = "ABCDCB" }
    | 8 -> { Type = ThingType.Backpack ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "BPAK" ; SpriteSequence = "A" }
    | 2019 -> { Type = ThingType.BlueArmor ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "ARM2" ; SpriteSequence = "AB" }
    | 2018 -> { Type = ThingType.GreenArmor ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "ARM1" ; SpriteSequence = "AB" }
    | 2012 -> { Type = ThingType.Medikit ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "MEDI" ; SpriteSequence = "A" }
    | 2025 -> { Type = ThingType.RadiationSuit ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "SUIT" ; SpriteSequence = "A" }
    | 2011 -> { Type = ThingType.Stimpack ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "STIM" ; SpriteSequence = "A" }
    | 2006 -> { Type = ThingType.BFG9000 ; Class = Set.ofList [ThingClass.Weapon;ThingClass.Pickup] ; Radius = 20 ; Sprite = "BFUG" ; SpriteSequence = "A" }
    | 2002 -> { Type = ThingType.Chaingun ; Class = Set.ofList [ThingClass.Weapon;ThingClass.Pickup] ; Radius = 20 ; Sprite = "MGUN" ; SpriteSequence = "A" }
    | 2005 -> { Type = ThingType.Chainsaw ; Class = Set.ofList [ThingClass.Weapon;ThingClass.Pickup] ; Radius = 20 ; Sprite = "CSAW" ; SpriteSequence = "A" }
    | 2004 -> { Type = ThingType.PlasmaRifle ; Class = Set.ofList [ThingClass.Weapon;ThingClass.Pickup] ; Radius = 20 ; Sprite = "PLAS" ; SpriteSequence = "A" }
    | 2003 -> { Type = ThingType.RocketLauncher ; Class = Set.ofList [ThingClass.Weapon;ThingClass.Pickup] ; Radius = 20 ; Sprite = "LAUN" ; SpriteSequence = "A" }
    | 2001 -> { Type = ThingType.Shotgun ; Class = Set.ofList [ThingClass.Weapon;ThingClass.Pickup] ; Radius = 20 ; Sprite = "SHOT" ; SpriteSequence = "A" }
    | 82 -> { Type = ThingType.SuperShotgun ; Class = Set.ofList [ThingClass.Weapon;ThingClass.Pickup] ; Radius = 20 ; Sprite = "SGN2" ; SpriteSequence = "A" }
    | 2007 -> { Type = ThingType.AmmoClip ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "CLIP" ; SpriteSequence = "A" }
    | 2048 -> { Type = ThingType.BoxOfAmmo ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "AMMO" ; SpriteSequence = "A" }
    | 2046 -> { Type = ThingType.BoxOfRockets ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "BROK" ; SpriteSequence = "A" }
    | 2049 -> { Type = ThingType.BoxOfShells ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "SBOX" ; SpriteSequence = "A" }
    | 2047 -> { Type = ThingType.CellCharge ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "CELL" ; SpriteSequence = "A" }
    | 17 -> { Type = ThingType.CellChargePack ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "CELP" ; SpriteSequence = "A" }
    | 2010 -> { Type = ThingType.Rocket ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "ROCK" ; SpriteSequence = "A" }
    | 2008 -> { Type = ThingType.ShotgunShells ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "SHEL" ; SpriteSequence = "A" }
    | 5 -> { Type = ThingType.BlueKeycard ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "BKEY" ; SpriteSequence = "AB" }
    | 40 -> { Type = ThingType.BlueSkullKey ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "BSKU" ; SpriteSequence = "AB" }
    | 13 -> { Type = ThingType.RedKeycard ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "RKEY" ; SpriteSequence = "AB" }
    | 38 -> { Type = ThingType.RedSkullKey ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "RSKU" ; SpriteSequence = "AB" }
    | 6 -> { Type = ThingType.YellowKeycard ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "YKEY" ; SpriteSequence = "AB" }
    | 39 -> { Type = ThingType.YellowSkullKey ; Class = Set.ofList [ThingClass.Pickup] ; Radius = 20 ; Sprite = "YSKU" ; SpriteSequence = "AB" }
    | 68 -> { Type = ThingType.Arachnotron ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 64 ; Sprite = "BSPI" ; SpriteSequence = "+" }
    | 64 -> { Type = ThingType.ArchVile ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 20 ; Sprite = "VILE" ; SpriteSequence = "+" }
    | 3003 -> { Type = ThingType.BaronOfHell ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 24 ; Sprite = "BOSS" ; SpriteSequence = "+" }
    | 3005 -> { Type = ThingType.Cacodemon ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 31 ; Sprite = "HEAD" ; SpriteSequence = "+" }
    | 65 -> { Type = ThingType.HeavyWeaponDude ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 20 ; Sprite = "CPOS" ; SpriteSequence = "+" }
    | 72 -> { Type = ThingType.CommanderKeen ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "KEEN" ; SpriteSequence = "A+" }
    | 16 -> { Type = ThingType.Cyberdemon ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 40 ; Sprite = "CYBR" ; SpriteSequence = "+" }
    | 3002 -> { Type = ThingType.Demon ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 30 ; Sprite = "SARG" ; SpriteSequence = "+" }
    | 3004 -> { Type = ThingType.Zombieman ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 20 ; Sprite = "POSS" ; SpriteSequence = "+" }
    | 9 -> { Type = ThingType.ShotgunGuy ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 20 ; Sprite = "SPOS" ; SpriteSequence = "+" }
    | 69 -> { Type = ThingType.HellKnight ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 24 ; Sprite = "BOS2" ; SpriteSequence = "+" }
    | 3001 -> { Type = ThingType.Imp ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 20 ; Sprite = "TROO" ; SpriteSequence = "+" }
    | 3006 -> { Type = ThingType.LostSoul ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "SKUL" ; SpriteSequence = "+" }
    | 67 -> { Type = ThingType.Mancubus ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 48 ; Sprite = "FATT" ; SpriteSequence = "+" }
    | 71 -> { Type = ThingType.PainElemental ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 31 ; Sprite = "PAIN" ; SpriteSequence = "+" }
    | 66 -> { Type = ThingType.Revenant ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 20 ; Sprite = "SKEL" ; SpriteSequence = "+" }
    | 58 -> { Type = ThingType.Spectre ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 30 ; Sprite = "SARG" ; SpriteSequence = "+" }
    | 7 -> { Type = ThingType.SpiderMastermind ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 128 ; Sprite = "SPID" ; SpriteSequence = "+" }
    | 84 -> { Type = ThingType.WolfensteinSS ; Class = Set.ofList [ThingClass.Monster;ThingClass.Obstacle] ; Radius = 20 ; Sprite = "SSWV" ; SpriteSequence = "+" }
    | 2035 -> { Type = ThingType.Barrel ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 10 ; Sprite = "BAR1" ; SpriteSequence = "AB+" }
    | 70 -> { Type = ThingType.BurningBarrel ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 10 ; Sprite = "FCAN" ; SpriteSequence = "ABC" }
    | 43 -> { Type = ThingType.BurntTree ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "TRE1" ; SpriteSequence = "A" }
    | 35 -> { Type = ThingType.Candelabra ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "CBRA" ; SpriteSequence = "A" }
    | 41 -> { Type = ThingType.EvilEye ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "CEYE" ; SpriteSequence = "ABCB" }
    | 28 -> { Type = ThingType.FiveSkullsShishKebab ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "POL2" ; SpriteSequence = "A" }
    | 42 -> { Type = ThingType.FloatingSkull ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "FSKU" ; SpriteSequence = "ABC" }
    | 2028 -> { Type = ThingType.FloorLamp ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "COLU" ; SpriteSequence = "A" }
    | 53 -> { Type = ThingType.HangingLeg ; Class = Set.ofList [ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "GOR5" ; SpriteSequence = "A" }
    | 52 -> { Type = ThingType.HangingPairOfLegs ; Class = Set.ofList [ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "GOR4" ; SpriteSequence = "A" }
    | 78 -> { Type = ThingType.HangingTorsoBrainRemoved ; Class = Set.ofList [ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "HDB6" ; SpriteSequence = "A" }
    | 75 -> { Type = ThingType.HangingTorsoLookingDown ; Class = Set.ofList [ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "HDB3" ; SpriteSequence = "A" }
    | 77 -> { Type = ThingType.HangingTorsoLookingUp ; Class = Set.ofList [ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "HDB5" ; SpriteSequence = "A" }
    | 76 -> { Type = ThingType.HangingTorsoOpenSkull ; Class = Set.ofList [ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "HDB4" ; SpriteSequence = "A" }
    | 50 -> { Type = ThingType.HangingVictimArmsOut ; Class = Set.ofList [ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "GOR2" ; SpriteSequence = "A" }
    | 74 -> { Type = ThingType.HangingVictimGutsAndBrainRemoved ; Class = Set.ofList [ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "HDB2" ; SpriteSequence = "A" }
    | 73 -> { Type = ThingType.HangingVictimGutsRemoved ; Class = Set.ofList [ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "HDB1" ; SpriteSequence = "A" }
    | 51 -> { Type = ThingType.HangingVictimOneLegged ; Class = Set.ofList [ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "GOR3" ; SpriteSequence = "A" }
    | 49 -> { Type = ThingType.HangingVictimTwitching ; Class = Set.ofList [ThingClass.Obstacle;ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "GOR1" ; SpriteSequence = "ABCB" }
    | 25 -> { Type = ThingType.ImpaledHuman ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "POL1" ; SpriteSequence = "A" }
    | 54 -> { Type = ThingType.LargeBrownTree ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 32 ; Sprite = "TRE2" ; SpriteSequence = "A" }
    | 29 -> { Type = ThingType.PileOfSkullsAndCandles ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "POL3" ; SpriteSequence = "AB" }
    | 55 -> { Type = ThingType.ShortBlueFirestick ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "SMBT" ; SpriteSequence = "ABCD" }
    | 56 -> { Type = ThingType.ShortGreenFirestick ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "SMGT" ; SpriteSequence = "ABCD" }
    | 31 -> { Type = ThingType.ShortGreenPillar ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "COL2" ; SpriteSequence = "A" }
    | 36 -> { Type = ThingType.ShortGreenPillarWithBeatingHeart ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "COL5" ; SpriteSequence = "AB" }
    | 57 -> { Type = ThingType.ShortRedFirestick ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "SMRT" ; SpriteSequence = "ABCD" }
    | 33 -> { Type = ThingType.ShortRedPillar ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "COL4" ; SpriteSequence = "A" }
    | 37 -> { Type = ThingType.ShortRedPillarWithSkull ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "COL6" ; SpriteSequence = "A" }
    | 86 -> { Type = ThingType.ShortTechnoFloorLamp ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "TLP2" ; SpriteSequence = "ABCD" }
    | 27 -> { Type = ThingType.SkullOnAPole ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "POL4" ; SpriteSequence = "A" }
    | 47 -> { Type = ThingType.Stalagmite ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "SMIT" ; SpriteSequence = "A" }
    | 44 -> { Type = ThingType.TallBlueFirestick ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "TBLU" ; SpriteSequence = "ABCD" }
    | 45 -> { Type = ThingType.TallGreenFirestick ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "TGRN" ; SpriteSequence = "ABCD" }
    | 30 -> { Type = ThingType.TallGreenPillar ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "COL1" ; SpriteSequence = "A" }
    | 46 -> { Type = ThingType.TallRedFirestick ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "TRED" ; SpriteSequence = "ABCD" }
    | 32 -> { Type = ThingType.TallRedPillar ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "COL3" ; SpriteSequence = "A" }
    | 85 -> { Type = ThingType.TallTechnoFloorLamp ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "TLMP" ; SpriteSequence = "ABCD" }
    | 48 -> { Type = ThingType.TallTechnoPillar ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "ELEC" ; SpriteSequence = "A" }
    | 26 -> { Type = ThingType.TwitchingImpaledHuman ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "POL6" ; SpriteSequence = "AB" }
    | 10 -> { Type = ThingType.BloodyMess1 ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "PLAY" ; SpriteSequence = "W" }
    | 12 -> { Type = ThingType.BloodyMess2 ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "PLAY" ; SpriteSequence = "W" }
    | 34 -> { Type = ThingType.Candle ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "CAND" ; SpriteSequence = "A" }
    | 22 -> { Type = ThingType.DeadCacodemon ; Class = Set.ofList [] ; Radius = 31 ; Sprite = "HEAD" ; SpriteSequence = "L" }
    | 21 -> { Type = ThingType.DeadDemon ; Class = Set.ofList [] ; Radius = 30 ; Sprite = "SARG" ; SpriteSequence = "N" }
    | 18 -> { Type = ThingType.DeadFormerHuman ; Class = Set.ofList [] ; Radius = 20 ; Sprite = "POSS" ; SpriteSequence = "L" }
    | 19 -> { Type = ThingType.DeadFormerSergeant ; Class = Set.ofList [] ; Radius = 20 ; Sprite = "SPOS" ; SpriteSequence = "L" }
    | 20 -> { Type = ThingType.DeadImp ; Class = Set.ofList [] ; Radius = 20 ; Sprite = "TROO" ; SpriteSequence = "M" }
    | 23 -> { Type = ThingType.DeadLostSoulInvisible ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "SKUL" ; SpriteSequence = "K" }
    | 15 -> { Type = ThingType.DeadPlayer ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "PLAY" ; SpriteSequence = "N" }
    | 62 -> { Type = ThingType.HangingLeg ; Class = Set.ofList [ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "GOR5" ; SpriteSequence = "A" }
    | 60 -> { Type = ThingType.HangingPairOfLegs ; Class = Set.ofList [ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "GOR4" ; SpriteSequence = "A" }
    | 59 -> { Type = ThingType.HangingVictimArmsOut ; Class = Set.ofList [ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "GOR2" ; SpriteSequence = "A" }
    | 61 -> { Type = ThingType.HangingVictimOneLegged ; Class = Set.ofList [ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "GOR3" ; SpriteSequence = "A" }
    | 63 -> { Type = ThingType.HangingVictimTwitching ; Class = Set.ofList [ThingClass.HangsOrFloats] ; Radius = 16 ; Sprite = "GOR1" ; SpriteSequence = "ABCB" }
    | 79 -> { Type = ThingType.PoolOfBlood1 ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "POB1" ; SpriteSequence = "A" }
    | 80 -> { Type = ThingType.PoolOfBlood2 ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "POB2" ; SpriteSequence = "A" }
    | 24 -> { Type = ThingType.PoolOfBloodAndFlesh ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "POL5" ; SpriteSequence = "A" }
    | 81 -> { Type = ThingType.PoolOfBrains ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "BRS1" ; SpriteSequence = "A" }
    | 88 -> { Type = ThingType.BossBrain ; Class = Set.ofList [ThingClass.Obstacle] ; Radius = 16 ; Sprite = "BBRN" ; SpriteSequence = "+" }
    | 11 -> { Type = ThingType.DeathmatchStart ; Class = Set.ofList [] ; Radius = 20 ; Sprite = "none" ; SpriteSequence = "-" }
    | 1 -> { Type = ThingType.Player1Start ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "PLAY" ; SpriteSequence = "+" }
    | 2 -> { Type = ThingType.Player2Start ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "PLAY" ; SpriteSequence = "+" }
    | 3 -> { Type = ThingType.Player3Start ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "PLAY" ; SpriteSequence = "+" }
    | 4 -> { Type = ThingType.Player4Start ; Class = Set.ofList [] ; Radius = 16 ; Sprite = "PLAY" ; SpriteSequence = "+" }
    | 89 -> { Type = ThingType.SpawnShooter ; Class = Set.ofList [] ; Radius = 20 ; Sprite = "none2" ; SpriteSequence = "-" }
    | 87 -> { Type = ThingType.SpawnSpot ; Class = Set.ofList [] ; Radius = 0 ; Sprite = "none3" ; SpriteSequence = "-" }
    | 14 -> { Type = ThingType.TeleportLanding ; Class = Set.ofList [] ; Radius = 20 ; Sprite = "none4" ; SpriteSequence = "-" }
    | _ -> raise (ThingLoaderException $"Unknown thing type {value}")

type Thing =
  { Definition: ThingDefinition
    Position: MapPosition
    AngleFacing: float
    IsDeaf: bool
  }

let load skillLevel (wad:byte array) (lump:LumpEntry) =
  let thingSize = 10
  let xPositionOffset = 0
  let yPositionOffset = 2
  let angleFacingOffset = 4
  let thingTypeOffset = 6
  let flagsOffset = 8
  let numberOfThings = lump.Size / thingSize
  let skillLevelBit =
    match skillLevel with
    | SkillLevel.ImTooYoungToDie
    | SkillLevel.HeyNotTooRough -> 0x1s
    | SkillLevel.HurtMePlenty -> 0x2s
    | SkillLevel.UltraViolence
    | SkillLevel.Nightmare -> 0x4s
  let multiPlayerOnlyBit = 0x10s
  let isDeafBit = 0x8s
  
  {0..numberOfThings-1}
  |> Seq.map(fun thingIndex ->
    let offset = lump.Offset + thingIndex * thingSize
    let flags = getInt16 (flagsOffset + offset) wad
    let isVisibleOnLevel = (flags &&& skillLevelBit > 0s) && (flags &&& multiPlayerOnlyBit = 0s)
    (offset,flags,isVisibleOnLevel)
  )
  |> Seq.filter(fun (_,_,isVisibleOnLevel) -> isVisibleOnLevel)
  |> Seq.map(fun (offset,flags, _) ->
    let x = getInt16 (xPositionOffset + offset) wad
    let y = getInt16 (yPositionOffset + offset) wad
    let angleFacing = getInt16 (angleFacingOffset + offset) wad
    let thingType = getInt16 (thingTypeOffset + offset) wad
    { Position = { X = int x ; Y = int y }
      Definition = ThingDefinition.Create (int thingType)
      AngleFacing = float angleFacing
      IsDeaf = (flags &&& isDeafBit) > 0s
    }
  )
  |> Seq.toList