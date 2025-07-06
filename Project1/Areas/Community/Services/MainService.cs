using Microsoft.AspNetCore.Mvc.RazorPages;
using Project1.Areas.Community.DTO;
using Project1.Contexts.DbConnection;
using System.Data;
using System.Data.SqlClient;

namespace Project1.Areas.Community.Services
{
    public class MainService
    {
        private readonly String connStr = AspClassDbConnection.ConnectionString;

        public async Task<List<BoardDTO>> GetBoardList(string catCls, Int32 Pag, Int32 PageSize, String? Section, String? SearchStr)
        {
            try
            {
                String str_sql = "";
                str_sql += "\t" + " SELECT " + "\r\n";
                str_sql += "\t" + " idx " + "\r\n";
                str_sql += "\t" + " , B_title " + "\r\n";
                str_sql += "\t" + " , B_content " + "\r\n";
                str_sql += "\t" + " , B_author " + "\r\n";
                str_sql += "\t" + " , EnterDateTime " + "\r\n";
                str_sql += "\t" + " , EnterUser " + "\r\n";
                str_sql += "\t" + " FROM " + "\r\n";
                str_sql += "\t" + " TJ_Board " + "\r\n";
                str_sql += "\t" + " WHERE " + "\r\n";
                str_sql += "\t" + " DelFlg IS NULL " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " CatCls = '"+ catCls + "' " + "\r\n";
                if (!string.IsNullOrEmpty(Section) && !string.IsNullOrEmpty(SearchStr))
                {
                    str_sql += "\t" + " AND " + "\r\n";
                    str_sql += "\t" + " " + Section + " LIKE N'%" + SearchStr + "%' " + "\r\n";
                }
                str_sql += "\t" + " ORDER BY idx DESC " + "\r\n";
                if (Pag != -1)
                {
                    str_sql += "\t" + " OFFSET " + Pag + " * " + PageSize + " ROWS " + "\r\n";
                    str_sql += "\t" + " FETCH NEXT " + PageSize + " ROWS ONLY " + "\r\n";
                }
                str_sql += "\t" + " ; " + "\r\n";
                System.Diagnostics.Debug.WriteLine(str_sql);
                List<BoardDTO> list = new();

                using (SqlConnection sqlConnection = new SqlConnection(connStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand(str_sql, sqlConnection))
                    {
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                BoardDTO board = new();
                                board.idx = reader.IsDBNull("idx") ? 0 : reader.GetInt32("idx");
                                board.B_title = reader.IsDBNull("B_title") ? String.Empty : reader.GetString("B_title");
                                board.B_content = reader.IsDBNull("B_content") ? String.Empty : reader.GetString("B_content");
                                board.B_author = reader.IsDBNull("B_author") ? String.Empty : reader.GetString("B_author");
                                board.EnterDateTime = reader.GetDateTimeOffset(reader.GetOrdinal("EnterDateTime"));
                                board.EnterUser = reader.IsDBNull("EnterUser") ? String.Empty : reader.GetString("EnterUser");
                                list.Add(board);
                            }
                        }
                        await sqlCommand.DisposeAsync();
                    }
                    await sqlConnection.CloseAsync();
                }
                return list;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetBoardListCount(string CatCls, string? Section, string? SearchStr)
        {
            try
            {
                String str_sql = "";
                str_sql += "\t" + " SELECT " + "\r\n";
                str_sql += "\t" + " COUNT(*) " + "\r\n";
                str_sql += "\t" + " AS totRecCnt " + "\r\n";
                str_sql += "\t" + " FROM " + "\r\n";
                str_sql += "\t" + " TJ_Board " + "\r\n";
                str_sql += "\t" + " WHERE " + "\r\n";
                str_sql += "\t" + " DelFlg IS NULL " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " CatCls = '"+ CatCls + "' " + "\r\n";
                if (!string.IsNullOrEmpty(Section) && !string.IsNullOrEmpty(SearchStr))
                {
                    str_sql += "\t" + " AND " + "\r\n";
                    str_sql += "\t" + " "+ Section + " LIKE N'%"+ SearchStr + "%' " + "\r\n";
                }
                str_sql += "\t" + " ; " + "\r\n";

                int totRecCnt = 0;
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    await connection.OpenAsync();

                    using (SqlCommand sqlCommand = new SqlCommand(str_sql, connection))
                    {
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                totRecCnt = reader.GetInt32("totRecCnt");
                            }
                            await reader.CloseAsync();
                        }
                        await sqlCommand.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
                return totRecCnt;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
