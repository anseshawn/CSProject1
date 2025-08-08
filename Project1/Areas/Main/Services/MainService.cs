using Project1.Areas.Main.DTO;
using Project1.Contexts.DbConnection;
using System.Data;
using System.Data.SqlClient;

namespace Project1.Areas.Main.Services
{
    public class MainService
    {
        private readonly String connStr = AspClassDbConnection.ConnectionString;
        public async Task<Int32> NewMemberJoin(MemberDTO memberDTO)
        {
            try
            {
                Int32 result = 0;
                String str_sql = "";
                str_sql += "\t" + " INSERT INTO " + "\r\n";
                str_sql += "\t" + " TM_Member " + "\r\n";
                str_sql += "\t" + " ( " + "\r\n";
                str_sql += "\t" + " u_name " + "\r\n";
                str_sql += "\t" + " , u_id " + "\r\n";
                str_sql += "\t" + " , u_pwd " + "\r\n";
                str_sql += "\t" + " ) " + "\r\n";
                str_sql += "\t" + " VALUES " + "\r\n";
                str_sql += "\t" + " ( " + "\r\n";
                str_sql += "\t" + " N'"+ memberDTO.u_name +"' " + "\r\n";
                str_sql += "\t" + " , N'"+ memberDTO.u_id +"' " + "\r\n";
                str_sql += "\t" + " , N'"+ memberDTO.u_pwd +"' " + "\r\n";
                str_sql += "\t" + " ) " + "\r\n";
                str_sql += "\t" + " ; " + "\r\n";
                System.Diagnostics.Debug.WriteLine(str_sql);

                using (SqlConnection sqlConnection = new SqlConnection(connStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand(str_sql, sqlConnection))
                    {
                        result = await sqlCommand.ExecuteNonQueryAsync();
                        await sqlCommand.DisposeAsync();
                    }
                    await sqlConnection.CloseAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine("InnerException : " + ex.InnerException.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<MemberDTO?> GetMemberInfo(string u_id)
        {
            try
            {
                String str_sql = "";
                str_sql += "\t" + " SELECT " + "\r\n";
                str_sql += "\t" + " u_name " + "\r\n";
                str_sql += "\t" + " , u_id " + "\r\n";
                str_sql += "\t" + " , u_pwd " + "\r\n";
                str_sql += "\t" + " FROM " + "\r\n";
                str_sql += "\t" + " TM_Member " + "\r\n";
                str_sql += "\t" + " WHERE " + "\r\n";
                str_sql += "\t" + " u_id = N'"+ u_id + "' " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " DelFlg IS NULL " + "\r\n";
                str_sql += "\t" + " ; " + "\r\n";
                System.Diagnostics.Debug.WriteLine(str_sql);

                MemberDTO? member = null;
                using (SqlConnection sqlConnection = new SqlConnection(connStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand(str_sql, sqlConnection))
                    {
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                member = new ();
                                member.u_name = reader.IsDBNull("u_name") ? String.Empty : reader.GetString("u_name");
                                member.u_id = reader.IsDBNull("u_id") ? String.Empty : reader.GetString("u_id");
                                member.u_pwd = reader.IsDBNull("u_pwd") ? String.Empty : reader.GetString("u_pwd");
                            }
                        }
                        await sqlCommand.DisposeAsync();
                    }
                    await sqlConnection.CloseAsync();
                }

                return member;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine("InnerException : " + ex.InnerException.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<MemberDTO?> GetMemberInfoByName(string u_name)
        {
            try
            {
                String str_sql = "";
                str_sql += "\t" + " SELECT " + "\r\n";
                str_sql += "\t" + " u_name " + "\r\n";
                str_sql += "\t" + " , u_id " + "\r\n";
                str_sql += "\t" + " , u_pwd " + "\r\n";
                str_sql += "\t" + " FROM " + "\r\n";
                str_sql += "\t" + " TM_Member " + "\r\n";
                str_sql += "\t" + " WHERE " + "\r\n";
                str_sql += "\t" + " u_name = N'" + u_name + "' " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " DelFlg IS NULL " + "\r\n";
                str_sql += "\t" + " ; " + "\r\n";
                System.Diagnostics.Debug.WriteLine(str_sql);

                MemberDTO? member = null;
                using (SqlConnection sqlConnection = new SqlConnection(connStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand(str_sql, sqlConnection))
                    {
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                member = new();
                                member.u_name = reader.IsDBNull("u_name") ? String.Empty : reader.GetString("u_name");
                                member.u_id = reader.IsDBNull("u_id") ? String.Empty : reader.GetString("u_id");
                                member.u_pwd = reader.IsDBNull("u_pwd") ? String.Empty : reader.GetString("u_pwd");
                            }
                        }
                        await sqlCommand.DisposeAsync();
                    }
                    await sqlConnection.CloseAsync();
                }

                return member;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine("InnerException : " + ex.InnerException.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
