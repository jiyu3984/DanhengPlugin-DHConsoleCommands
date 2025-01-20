using EggLink.DanhengServer.Command;
using EggLink.DanhengServer.Command.Command;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.Internationalization;

namespace DanhengPlugin.DHConsoleCommands.Commands;

[CommandInfo("remove", "remove a character or unequipped relics", "Usage: /remove <avatarId/relics>")]
public class CommandRemove : ICommand
{
    [CommandMethod("relics")]
    public async ValueTask RemoveRelics(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        int removedCount = 0;
        foreach (var item in player.InventoryManager!.Data.RelicItems)
        {
            if (item.EquipAvatar == 0)
            {
                player.InventoryManager.Data.RelicItems.Remove(item);
                item.Count = 0;
                removedCount++;
                await player.SendPacket(new PacketPlayerSyncScNotify(item));
            }
        }

        await arg.SendMsg(I18NManager.Translate("DHConsoleCommands.RemoveRelicsSuccess"));
        await arg.SendMsg(removedCount.ToString());
    }

    [CommandMethod("help")]
    public async ValueTask RemoveHelp(CommandArg arg)
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
        await arg.SendMsg($@"All avatar ids: [{string.Join(", ", avatarIds)}]");
    }

    [CommandDefault]
    public async ValueTask RemoveAvatar(CommandArg arg)
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
        // get avatar data
        var avatar = player.AvatarManager!.GetAvatar(avatarId);
        if (avatar == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Avatar.AvatarNotFound"));
            return;
        }
        // remove relics
        for (int i = 0; i < 6; i++)
        {
            avatar.PathInfoes[avatarId].Relic.TryGetValue(i, out var itemId);
            if (itemId == 0) continue;
            var oldItem = player.InventoryManager!.Data.RelicItems.Find(x => x.UniqueId == itemId);
            if (oldItem != null)
            {
                oldItem.EquipAvatar = 0;
                await player.SendPacket(new PacketPlayerSyncScNotify(oldItem));
            }
        }
        // remove light cone
        if (avatar.PathInfoes[avatarId].EquipId != 0)
        {
            var oldItem = player.InventoryManager!.Data.EquipmentItems.Find(x => x.UniqueId == avatar.PathInfoes[avatarId].EquipId);
            if (oldItem != null)
            {
                oldItem.EquipAvatar = 0;
                await player.SendPacket(new PacketPlayerSyncScNotify(oldItem));
            }
        }

        avatar.PathInfoes.Remove(avatarId);
        player.AvatarManager!.AvatarData.Avatars.Remove(avatar);

        await arg.SendMsg(I18NManager.Translate("DHConsoleCommands.RemoveAvatarSuccess"));
    }

}