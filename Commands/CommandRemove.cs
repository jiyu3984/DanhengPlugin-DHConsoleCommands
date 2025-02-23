using EggLink.DanhengServer.Command;
using EggLink.DanhengServer.Command.Command;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Kcp;

namespace DanhengPlugin.DHConsoleCommands.Commands;

[CommandInfo("remove", "remove a character or unequipped relics", "Usage: /remove <avatarId/relics/equipment>")]
public class CommandRemove : ICommand
{
    [CommandMethod("0 relics")]
    public async ValueTask RemoveRelics(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        List<ItemData> itemsToRemove = player.InventoryManager!.Data.RelicItems?.FindAll(x => !x.Locked && x.EquipAvatar == 0).ToList() ?? [];
        List<(int ItemId, int Count, int UniqueId)> removeData = [.. itemsToRemove.Select(x => (x.ItemId, x.Count, x.UniqueId))];
        await player.InventoryManager.RemoveItems(removeData, true);

        string output = string.Join("\n", removeData.Select(x => $"{x.UniqueId}: {x.ItemId}x{x.Count}"));
        await arg.SendMsg(I18NManager.Translate("DHConsoleCommands.RemoveRelicsSuccess") + $"\n{output}");
    }

    [CommandMethod("0 equipment")]
    public async ValueTask RemoveEquipment(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        List<ItemData> itemsToRemove = player.InventoryManager!.Data.EquipmentItems?.FindAll(x => !x.Locked && x.EquipAvatar == 0).ToList() ?? [];
        List<(int ItemId, int Count, int UniqueId)> removeData = [.. itemsToRemove.Select(x => (x.ItemId, x.Count, x.UniqueId))];
        await player.InventoryManager.RemoveItems(removeData, true);

        string output = string.Join("\n", removeData.Select(x => $"{x.UniqueId}: {x.ItemId}x{x.Count}"));
        await arg.SendMsg(I18NManager.Translate("DHConsoleCommands.RemoveEquipmentSuccess") + $"\n{output}");
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

        var path = avatar.GetCurPathInfo();
        // remove relics
        for (int i = 0; i < 6; i++)
        {
            path.Relic.TryGetValue(i, out var itemId);
            if (itemId == 0) continue;
            var oldItem = player.InventoryManager!.Data.RelicItems.Find(x => x.UniqueId == itemId);
            if (oldItem != null)
            {
                oldItem.EquipAvatar = 0;
                await player.SendPacket(new PacketPlayerSyncScNotify(oldItem));
            }
        }
        // remove light cone
        if (path.EquipId != 0)
        {
            var oldItem = player.InventoryManager!.Data.EquipmentItems.Find(x => x.UniqueId == path.EquipId);
            if (oldItem != null)
            {
                oldItem.EquipAvatar = 0;
                await player.SendPacket(new PacketPlayerSyncScNotify(oldItem));
            }
        }

        avatar.PathInfoes.Remove(path.PathId);
        if (avatar.PathInfoes.Count == 0)
        {
            player.AvatarManager!.AvatarData.Avatars.Remove(avatar);
        }

        await arg.SendMsg(I18NManager.Translate("DHConsoleCommands.RemoveAvatarSuccess"));
    }

}