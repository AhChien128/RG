namespace prjRGsystem.Managers.DbManagers
{
    public class SysFileManager
    {
        

        private readonly IWebHostEnvironment webHostEnvironment;

        public SysFileManager(IWebHostEnvironment _webHostEnvironment)
        {
            webHostEnvironment = _webHostEnvironment;
        }
        public static MemoryStream GetAsposeLicense(IWebHostEnvironment webHostEnvironment)
        {
            string contentRootPath = webHostEnvironment.ContentRootPath;
            string licPath = "Files\\Aspose\\";
            string licensePath = (contentRootPath + licPath + "Aspose.Total.lic");
            return new MemoryStream(System.IO.File.ReadAllBytes(licensePath));
        }
    }
}
