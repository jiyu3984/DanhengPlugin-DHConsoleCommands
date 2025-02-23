# DanhengPlugin-DHConsoleCommands

Raw commands to be used by [DHConsole](https://github.com/Anyrainel/DHConsole). No i18n support.

## Usage

Download the latest release from [release](https://github.com/Anyrainel/DanhengPlugin-DHConsoleCommands/releases/latest).
Then place it in the `Plugins` folder in your Danheng server directory.

## Commands

- build (adapted from [CharacterBuilder](https://github.com/EggLinks/DanhengPlugin-CharacterBuilder))
  - `build recommend` build recommended relics for current character
  - `build all` build all characters (not used in console)
  - `build <avatarId>` build specified character (not used in console)
- equip
  - `equip item <avatarId> <itemId> l<level> r<rank>` equip item to character
  - `equip relic <avatarId> <relicId> <mainAffixId> <subAffixId*4>:<level*4>` equip relic to character
- remove (adapted from [Clean](https://github.com/AfricanCh/DanhengPlugin-Clean))
  - `remove relics` remove all unequipped relics
  - `remove equipment` remove all unequipped equipment
  - `remove <avatarId>` remove specified avatar (not used in console)
- fetch
  - `fetch owned` show all owned character ids
  - `fetch avatar <avatarId>` show character info
  - `fetch inventory` show all items
  - `fetch player` show player info
- gametext
  - `gametext avatar #<language>` return character id to translation for certain language
  - `gametext item #<language>` return item id to translation for certain language
  - `gametext mainmission #<language>` return main mission id to translation for certain language
  - `gametext submission #<language>` return submission id to translation for certain language
- debug
  - `debug item` show item to character equip status
  - `debug relic` show relic to character equip status
  - `debug avataritem` show character to item equip status
  - `debug avatarrelic` show character to relic equip status
