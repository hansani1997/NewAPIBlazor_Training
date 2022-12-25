using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Data.SQL92.Definition;
using BlueLotus360.Data.SQL92.Extenstions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlueLotus360.Data.SQL92.Repository
{
    internal abstract class BaseRepository
    {

        protected IDbDataParameter CreateAndAddParameter(IDbCommand command, string name, object value)
        {
            IDbDataParameter dbDataParameter = command.CreateParameter();
            dbDataParameter.ParameterName = name;
            dbDataParameter.Value = value;
            command.Parameters.Add(dbDataParameter);
            return dbDataParameter;
        }

        public DataTable GetSearchIdTable(IList<int> idList)
        {
            return this.GetSearchIdTable(idList.ToArray<int>());
        }

        protected DataTable GetSearchIdTable(int[] idList)
        {
            string[] feildList = { "SearchId" };

            DataTable Dt = GetDataTable(feildList);
            DataRow dataRow;
            if (idList == null)
            {
                idList = new int[] { };
            }
            foreach (int i in idList)
            {
                dataRow = Dt.NewRow();
                dataRow["SearchId"] = i;
                Dt.Rows.Add(dataRow);

            }

            return Dt;



        }

        public DataTable GetDataTable(string[] fields)
        {
            DataTable Dt = new DataTable();
            Dt.Clear();
            foreach (string s in fields)
            {
                Dt.Columns.Add(s);

            }
            return Dt;

        }
        protected T GetColumn<T>(string columnName, IDataReader dataReader)
        {
            try
            {
                if (dataReader[columnName] != DBNull.Value)
                {
                    return (T)Convert.ChangeType(dataReader[columnName], typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        protected DataTable GetPayementModeRecieptTable(IList<PaymentModeWiseAmount> reciepts)
        {
            string[] feildList = { "PmtModeKy","Amt","PmtDocNo",
                                    "PmtDocDt" };


            DataTable Dt = GetDataTable(feildList);


            foreach (var item in reciepts)
            {
                DataRow dataRow = Dt.NewRow();
                dataRow["PmtModeKy"] = item.PaymentMode.CodeKey;
                dataRow["Amt"] = item.Amount;
                dataRow["PmtDocNo"] = item.PayementDocumentNumber;
                dataRow["PmtDocDt"] = item.PayementDocumentDate;
                Dt.Rows.Add(dataRow);

            }

            return Dt;


        }

        protected  ISQLDataLayer _dataLayer { get; private set; }
        public BaseRepository(ISQLDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }
        
    }
}
