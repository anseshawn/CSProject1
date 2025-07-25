using Project1.Areas.Community.DTO;
using Project1.Contexts.DbConnection;
using System.Data;
using System.Data.SqlClient;
using static System.Collections.Specialized.BitVector32;

namespace Project1.Areas.Blog.Services
{
    public class DataService
    {
        private readonly String connStr = AspClassDbConnection.ConnectionString;

        public async Task<List<BoardDTO>> GetBlogList(String CatCls, Int32 Skip, Int32 Take, String? Section, String? SearchStr, String u_id)
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
                str_sql += "\t" + " CatCls = '" + CatCls + "' " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " EnterUser = N'"+ u_id + "' " + "\r\n";
                if (!string.IsNullOrEmpty(Section) && !string.IsNullOrEmpty(SearchStr))
                {
                    str_sql += "\t" + " AND " + "\r\n";
                    str_sql += "\t" + " " + Section + " LIKE N'%" + SearchStr + "%' " + "\r\n";
                }
                str_sql += "\t" + " ORDER BY idx DESC " + "\r\n";
                str_sql += "\t" + " OFFSET " + Skip + " ROWS " + "\r\n";
                str_sql += "\t" + " FETCH NEXT " + Take + " ROWS ONLY " + "\r\n";
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
    }
}
