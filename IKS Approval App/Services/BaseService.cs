using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using DataLayerLibrary;

namespace IKS_Approval_App.Services
{
    public class BaseService
    {
        public readonly DBHelper ApprovalDB;
        public readonly string ConnectionString;
        public readonly string DbProvider;

        public BaseService()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            DbProvider = ConfigurationManager.AppSettings["DefaultDBProvider"];
            ApprovalDB = new DBHelper(ConnectionString, DbProvider);
        }
    }
}