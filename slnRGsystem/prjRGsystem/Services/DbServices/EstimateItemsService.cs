using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Manager.DbManager;
using prjRGsystem.Models.DbModels;
using System.Text;

namespace prjRGsystem.Services.DbServices
{
    public class EstimateItemsService: EstimateItemsManager
    {
        public EstimateItemsService(RGPropertyContext _db) : base(_db)
        {
        }
    }
}
