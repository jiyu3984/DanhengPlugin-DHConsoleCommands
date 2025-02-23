using System.Text;
using EggLink.DanhengServer.Command;
using EggLink.DanhengServer.Command.Command;
using EggLink.DanhengServer.Internationalization;
using DanhengPlugin.DHConsoleCommands.Data;
using EggLink.DanhengServer.Enums.Avatar;
using EggLink.DanhengServer.Proto;


namespace DanhengPlugin.DHConsoleCommands.Commands;

[CommandInfo("fetch", "fetch data about a player", "Usage: /fetch <owned/avatar/inventory/player> ...")]
public class CommandFetch : ICommand
{

    [CommandMethod("0 owned")]
    public async ValueTask fetchOwnedCharacters(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        List<int> avatarIds = [];
        foreach (var avatar in player.AvatarManager!.AvatarData.Avatars)
        {
            avatarIds.Add(avatar.AvatarId);
        }
        avatarIds.Sort();
        await arg.SendMsg($@"{string.Join(", ", avatarIds)}");
    }

    [CommandMethod("0 player")]
    public async ValueTask fetchPlayer(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        await arg.SendMsg($@"level: {player.Data.Level}, gender: {(player.Data.CurrentGender == Gender.Man ? 1 : 2)}");
    }

    [CommandMethod("avatar")]
    public async ValueTask fetchAvatar(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }
        if (arg.BasicArgs.Count == 0)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }
        var avatarId = arg.GetInt(0);
        if (avatarId == 0)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Avatar.AvatarNotFound"));
            return;
        }
        var avatar = player.AvatarManager!.GetAvatar(avatarId);
        if (avatar == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Avatar.AvatarNotFound"));
            return;
        }
        var path = avatar.GetCurPathInfo();
        StringBuilder output = new StringBuilder();
        output.AppendLine($@"[Character] path: {path.PathId}, level: {avatar.Level}, rank: {path.Rank}");
        output.AppendLine($@"[Talent] {string.Join("|", [.. avatar.SkillTree.Select(x => $"{x.Key}: {x.Value}")])}");
        if (path.EquipId != 0)
        {
            var item = player.InventoryManager!.Data.EquipmentItems.Find(x => x.UniqueId == path.EquipId);
            output.AppendLine($@"[Equip] id: {item?.ItemId}, level: {item?.Level}, rank: {item?.Rank}");
        }
        for (int i = 1; i <= 6; i++)
        {
            path.Relic.TryGetValue(i, out var relicUniqueId);
            if (relicUniqueId == 0) continue;
            var relic = player.InventoryManager!.Data.RelicItems.Find(x => x.UniqueId == relicUniqueId);
            if (relic == null) continue;
            var subAffixes = string.Join("|", relic.SubAffixes.Select(x => $"{getAffixName(i, false, x.Id)}-{x.Count - 1}+{x.Step}"));
            output.AppendLine($@"[Relic {i}] id: {relic.ItemId}, level: {relic.Level}, mainAffix: {getAffixName(i, true, relic.MainAffix)}, subAffixes: {subAffixes}");
        }
        await arg.SendMsg(output.ToString());
    }

    private static string getAffixName(int relicPosition, bool isMainAffix, int affixId)
    {
        RelicTypeEnum relicType = (RelicTypeEnum)relicPosition;
        if (isMainAffix)
        {
            return PluginConstants.RelicMainAffix[relicType].First(x => x.Value == affixId).Key.ToString();
        }
        return PluginConstants.RelicSubAffix.First(x => x.Value == affixId).Key.ToString();
    }

    [CommandMethod("0 inventory")]
    public async ValueTask fetchInventory(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        await arg.SendMsg($@"{string.Join("\n", [.. player.InventoryManager!.Data.MaterialItems.Select(x => $"{x.ItemId}: {x.Count}")])}");
    }

}