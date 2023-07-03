using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models
{

    /// <summary>
    /// 登入狀態
    /// </summary>
    public enum LoginStatusType
    {
        [ColumnAttribute("成功")]
        SUCCESS = 1,
        [ColumnAttribute("失敗")]
        FAIL = 0
    }

    /// <summary>
    /// 資料開放權限
    /// </summary>
    public enum DataRangeType
    {
        [ColumnAttribute("自己部門與底下部門")]
        OWN_DEPARTMENT_BOTTOM_DEPARTMENT = 3,
        [ColumnAttribute("自己部門")]
        OWN_DEPARTMENT = 2,
        [ColumnAttribute("只有自己")]
        OWN = 1,
        [ColumnAttribute("無")]
        NONE = 0
    }

    /// <summary>
    /// 業務類型
    /// </summary>
    public enum BusinessType
    {
        [ColumnAttribute("秘書處總務組")]
        PROPERTY = 5,
        [ColumnAttribute("技術合作處")]
        TECHNOLOGY = 4,
        [ColumnAttribute("秘書處採購組")]
        PURCHASE = 3,
        [ColumnAttribute("會計室")]
        ACCOUNTING = 2,
        [ColumnAttribute("技術團")]
        TECHNICAL_TEAM = 1,
        [ColumnAttribute("無")]
        NONE = 0
    }

    /// <summary>
    /// 啟用狀態
    /// </summary>
    public enum SearchType
    {
        [ColumnAttribute("全部")]
        ALL = 0,//全部
        [ColumnAttribute("啟用")]
        ENABLED = 1,//啟用
        [ColumnAttribute("未啟用")]
        DISABLED = 2,//未啟用
    }

    /// <summary>
    /// 系統功能使用權限
    /// </summary>
    public enum RolePrivilegeType
    {
        [ColumnAttribute("新增")]
        ADD = 0, // 新增
        [ColumnAttribute("編輯")]
        EDIT = 1, // 新增
        [ColumnAttribute("瀏覽")]
        VIEW = 2, // 新增
        [ColumnAttribute("刪除")]
        REMOVED = 3, // 新增
        [ColumnAttribute("報表")]
        REPORT = 4, // 新增
        [ColumnAttribute("審核")]
        AUDIT = 5 // 新增

    }

}