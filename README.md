# DanhengPlugin-DHConsoleCommands

Raw commands to be used by [DHConsole](https://github.com/Anyrainel/DHConsole). No i18n support.

## Usage

Download the latest release from [release](https://github.com/Anyrainel/DanhengPlugin-DHConsoleCommands/releases/latest).
Then place it in the `Plugins` folder in your Danheng server directory.

## Commands

- build (adapted from [CharacterBuilder](https://github.com/EggLinks/DanhengPlugin-CharacterBuilder))
  - `build recommend <avatarId>` recommended relics for a character without making changes
  - `build all` build all characters (unused in console)
  - `build <avatarId>` build specified character (unused in console)
- equip
  - `equip item <avatarId> <itemId> l<level> r<rank>` equip item to character
  - `equip relic <avatarId> <relicId> <mainAffixId> <subAffixId*4>:<level*4>` equip relic to character
- remove (adapted from [Clean](https://github.com/AfricanCh/DanhengPlugin-Clean))
  - `remove relics` remove all unequipped relics
  - `remove equipment` remove all unequipped equipment
  - `remove avatar <avatarId>` remove specified character, then kick you back to login screen (unused in console)
- fetch
  - `fetch owned` show all owned character ids
  - `fetch avatar <avatarId>` show character info
  - `fetch inventory` show all items
  - `fetch player` show player info
  - `fetch scene` show props in the current scene
- gametext
  - `gametext avatar #<language>` return character id to translation for certain language
  - `gametext item #<language>` return item id to translation for certain language
  - `gametext mainmission #<language>` return main mission id to translation for certain language
  - `gametext submission #<language>` return submission id to translation for certain language
- debuglink
  - `debuglink item` show item to character equip status
  - `debuglink relic` show relic to character equip status
  - `debuglink avataritem` show character to item equip status
  - `debuglink avatarrelic` show character to relic equip status
