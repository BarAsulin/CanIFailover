using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using NetVimClient;
using CanIFailover.SQLObjects;

namespace CanIFailover
{
    public class SQLServer
    {

        private readonly SqlConnection sqlConnection = new SqlConnection();
        private string pipe { get; set; }
        private string user { get; set; } = "sazerto";
        private string password { get; set; } = "zertodata";
        private string initCatalog { get; set; } = "zvm_db";
        

        public SQLServer()
        {
            connectToDatabase();
        }
        private void connectToDatabase()
        {
            pipe = GetLocalPipe().GetAwaiter().GetResult();
            sqlConnection.ConnectionString = "Server=" + pipe + ";UID=" + user + ";PWD=" + password + ";Initial Catalog=" + initCatalog;
            sqlConnection.Open();
        }
        public static async Task<string> GetLocalPipe()
        {
            StreamReader streamReader = new StreamReader(FilesystemSearch.GetStorageProperties());
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;
            using (XmlReader reader = XmlReader.Create(streamReader, settings))
            {
                while (await reader.ReadAsync())
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        if (await reader.GetValueAsync() == "m_server") // if it's the m_server field get the next value which is the pipe.
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                reader.Read(); // progress to the next element which is the pipe.
                            }
                            return await reader.GetValueAsync();
                        }
                    }
                }
            }
            throw new Exception("Failed to find zvm database pipe");
        }

        public DataTable QuerySingleTable(string query)
        {
            SqlCommand command = sqlConnection.CreateCommand();
            command.CommandText = query;
            command.CommandTimeout = 15;
            command.CommandType = CommandType.Text;
            var dataAdapter = new SqlDataAdapter(command);
            var data = new DataSet();
            dataAdapter.Fill(data);
            DataTable table = data.Tables[0];
            return table;
        }

        public string GetvCenterIP()
        {
            DataTable table = QuerySingleTable(DBConstantQuerys.GetVCenterSettingStorageObjectHostname);
            return table.Rows[0].Cast<vCenterSettingsStorageObject>().Hostname;
        }
        public List<VMResourceInfoStorageObject> GetVMResourceInfoStorageObjects()
        {
            DataTable table = QuerySingleTable(DBConstantQuerys.GetVMResourceInfoStorageObjectLast5Days);
            if(table!=null)
            {
                List<VMResourceInfoStorageObject> returnList = new List<VMResourceInfoStorageObject>(table.Rows.Count);
                foreach (DataRow row in table.Rows)
                {
                    returnList.Add(row.Cast<VMResourceInfoStorageObject>());
                }
                return returnList;
            }
            throw new Exception("Failed to query DB on GetVMResourceInfoStorageObjects");
        }
        public List<VMDataStorageObject> GetVMDataStorageObjects(string vCenterGuid)
        {
            vCenterGuid = "'" + vCenterGuid + "'"; 
            DataTable table = QuerySingleTable(DBConstantQuerys.GetVMDataStorageObject + vCenterGuid);
            if (table != null)
            {
                List<VMDataStorageObject> returnList = new List<VMDataStorageObject>(table.Rows.Count);
                foreach (DataRow row in table.Rows)
                {
                    returnList.Add(row.Cast<VMDataStorageObject>());
                }
                return returnList;
            }
            throw new Exception("Failed to query DB on GetVMDataStorageObject");
        }
        public List<VraInfoHostIdentifierInternalHostName> GetVrasInternalHostname()
        {
            DataTable table = QuerySingleTable(DBConstantQuerys.GetVRAsInternalHostname);
            if (table != null)
            {
                List<VraInfoHostIdentifierInternalHostName> returnList = new List<VraInfoHostIdentifierInternalHostName>(table.Rows.Count);
                foreach (DataRow row in table.Rows)
                {
                    returnList.Add(row.Cast<VraInfoHostIdentifierInternalHostName>());
                }
                return returnList;
            }
            throw new Exception();
        }
        public List<IdentifierMapperHost> GetIdentifierMapperHosts()
        {
            DataTable table = QuerySingleTable(DBConstantQuerys.GetIdentifierMapperHosts);
            if (table != null)
            {
                List<IdentifierMapperHost> returnList = new List<IdentifierMapperHost>(table.Rows.Count);
                foreach (DataRow row in table.Rows)
                {
                    returnList.Add(row.Cast<IdentifierMapperHost>());
                }
                return returnList;
            }
            throw new Exception();
        }
    }
}
