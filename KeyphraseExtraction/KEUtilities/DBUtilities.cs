using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;

namespace KeyphraseExtraction.KEUtilities
{
	/// <summary>
	/// This class contains all the necessary functions to query or manipulate
	/// database data stored in an SQL database.
	/// 
	/// If you use this file as-as you will need to inlude the connection string
	/// in your application settings using "ConnectionString" as the setting name
	/// </summary>
    public abstract class DBUtilities
	{
        static MySqlConnection sqlCon;
        static MySqlTransaction transaction;
		#region Private Read Methods

		/// <summary>
		/// Retrieves the connection string from the calling application .config file.
		/// The connection string must be in an application setting called 
		/// "ConnectionString".
		/// </summary>
		/// <returns></returns>
        private static string GetConnectionString(string dbName)
		{
			string connectionString = "";

			XmlDocument xd = new XmlDocument();
			xd.Load(Application.ExecutablePath + ".config");

			XmlNodeList nodeList = xd.GetElementsByTagName("connectionStrings");

			foreach (XmlNode node in nodeList)
			{
                foreach (XmlNode conNode in node.ChildNodes)
                {
                    if (conNode.Attributes[1].Name.Equals("name") && conNode.Attributes[1].Value.Equals(dbName))
                        connectionString = conNode.Attributes[0].Value;
                }
			}

			return connectionString;
		}

		#endregion

		#region Direct Access Functions

        public static MySqlConnection OpenConnection(string dbName)
        {
             try
            {
                sqlCon = new MySqlConnection(GetConnectionString(dbName));
                sqlCon.Open();
                transaction = sqlCon.BeginTransaction();
                return sqlCon;
            }
            catch (MySqlException se)
            {
                Console.WriteLine(se.Message);                
            }
             return null;            
        }
        public static void CloseConnection()
        {
            try
            {
                transaction.Commit();

                sqlCon.Close();
                sqlCon.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Execute a SELECT query directly on the server
        /// </summary>
        /// <param name="commandText">The SELECT statement text</param>
        /// <returns></returns>
        public static ArrayList ExecuteReaderQuery(string commandText)
        {
            ArrayList results = new ArrayList();
            MySqlCommand command;
            MySqlDataReader reader;

            try
            {
                command = new MySqlCommand(commandText, sqlCon);                
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Dictionary<string, string> resLine =
                        new Dictionary<string, string>(reader.FieldCount);

                    for (int i = 0; i < reader.FieldCount; i++)
                        resLine.Add(reader.GetName(i), reader[i].ToString());

                    results.Add(resLine);
                }
                reader.Close();
            }
            catch (MySqlException se)
            {
                Console.WriteLine(se.Message);                
            }

            return results;

        }

        /// <summary>
        /// Execute a non-reader (update, insert, delete) query directly on the server
        /// </summary>
        /// <param name="commandText">The SQL statement text</param>
        /// <returns></returns>
        public static int ExecuteNonReader(string commandText)
        {
            int retVal = -1;

            if (String.IsNullOrEmpty(commandText))
                return retVal;

            
            MySqlCommand command;

            try
            {
                command = new MySqlCommand(commandText, sqlCon);
                command.Transaction = transaction;
                retVal = command.ExecuteNonQuery();
            }
            catch (MySqlException se)
            {
                System.Windows.Forms.MessageBox.Show(se.Message +
                    "\n\nCommand was: " + commandText);
                // Attempt to roll back the transaction. 
                try
                {
                    transaction.Rollback();                    
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred 
                    // on the server that would cause the rollback to fail, such as 
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
            }
            return retVal;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnData">key: column name, value: inserted value</param>
        /// <returns></returns>
        public static string InsertQueryString(string tableName, Dictionary<string,string> columnData)
        {
            string queryString = "Insert {0} ({1}) values ({2});";
            string columnName = string.Empty;
            string columnValue = string.Empty;
            foreach (var pair in columnData)
            {
                columnName += pair.Key + ",";
                columnValue += "'"+pair.Value + "',";
            }
            // remove the last of ","
            queryString = string.Format(queryString, tableName, columnName.Remove(columnName.Length - 1), columnValue.Remove(columnValue.Length - 1));

            return queryString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnData">key: column name, value: inserted value</param>
        /// <returns></returns>
        public static string UpdateQueryString(string tableName, Dictionary<string, string> setFields, Dictionary<string,string> whereFields)
        {
            string queryString = "Update {0} set {1} where {2};";
            string setField = string.Empty;
            string whereField = string.Empty;
            foreach (var pair in setFields)
            {
                setField += pair.Key + "='" + pair.Value + "',";
            }

            foreach (var pair in whereFields)
            {
                whereField += pair.Key + "='" + pair.Value + "',";
            }
            // remove the last of ","
            queryString = string.Format(queryString, tableName, setField.Remove(setField.Length - 1), whereField.Remove(whereField.Length - 1));

            return queryString;
        }

		#endregion

	}
}
