using prjRGsystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "使用者登入紀錄資料表")]
    public class SysLogLogin : AbstractBusinessAppEntity, ILoggerEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysLogLogin";

        [Display(Name = "登入帳號")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? accessUserId { get; set; }

        [Display(Name = "登入IP")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? loginIp { get; set; }

        [NotMapped]
        public LoginStatusType? loginStatusTypeE { get; set; }
        [Display(Name = "登入狀態,成功SUCCESS,失敗FAIL")]
        public string? loginStatus
        {
            get { return loginStatusTypeE.ToString(); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    loginStatusTypeE = null;
                }
                else
                {
                    if (!Enum.TryParse((string)value, out LoginStatusType res))
                        throw new ApplicationException(string.Format("Can't convert '{0}' to type [{1}]", value, typeof(LoginStatusType)));
                    loginStatusTypeE = res;
                }
            }
        }

        [Display(Name = "備註")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? remark { get; set; }

    }
}
