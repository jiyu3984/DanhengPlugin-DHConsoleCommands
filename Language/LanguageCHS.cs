using EggLink.DanhengServer.Enums.Language;
using EggLink.DanhengServer.Internationalization;

namespace DanhengPlugin.DHConsoleCommands.Language;

[PluginLanguage(ProgramLanguageTypeEnum.CHS)]
public class LanguageCHS
{
    public PluginLanguage DHConsoleCommands => new();
}

public class PluginLanguage
{
    public string LoadedDHConsoleCommands => "已加载角色构建器插件！";
    public string UnloadedDHConsoleCommands => "角色构建器插件已卸载！";
    public string NoRecommend => "Excel中不存在推荐遗器";
    public string AffixError => "词条加载错误";
    public string AffixCountError => "词条数量错误";
    public string BuildSuccess => "构建成功！";
    public string RemoveAvatarSuccess => "成功移除角色";
    public string RemoveRelicsSuccess => "成功移除多余遗器";
    public string EquipSuccess => "成功装备";
}