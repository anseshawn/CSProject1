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

        public async Task<Int32> SetBoardContent(string? CatCls, string? B_title, string? B_content, string? u_id)
        {
            try
            {
                Int32 result = 0;
                String str_sql = "";
                str_sql += "\t" + " INSERT INTO " + "\r\n";
                str_sql += "\t" + " TJ_Board " + "\r\n";
                str_sql += "\t" + " ( " + "\r\n";
                str_sql += "\t" + " CatCls " + "\r\n";
                str_sql += "\t" + " , B_title " + "\r\n";
                str_sql += "\t" + " , B_content " + "\r\n";
                str_sql += "\t" + " , B_author " + "\r\n";
                str_sql += "\t" + " , EnterDateTime " + "\r\n";
                str_sql += "\t" + " , EnterUser " + "\r\n";
                str_sql += "\t" + " ) " + "\r\n";
                str_sql += "\t" + " VALUES " + "\r\n";
                str_sql += "\t" + " ( " + "\r\n";
                str_sql += "\t" + " N'" + CatCls + "' " + "\r\n";
                str_sql += "\t" + " , N'" + B_title + "' " + "\r\n";
                str_sql += "\t" + " , N'" + B_content + "' " + "\r\n";
                str_sql += "\t" + " , (SELECT u_name FROM TM_Member WHERE u_id = N'" + u_id + "') " + "\r\n";
                str_sql += "\t" + " , GETDATE() " + "\r\n";
                str_sql += "\t" + " , N'" + u_id + "' " + "\r\n";
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

        public async Task<BoardDTO> GetBoardContent(Int32 idx, String u_id)
        {
            try
            {
                String str_sql = "";
                str_sql += "\t" + " SELECT " + "\r\n";
                str_sql += "\t" + " B_title " + "\r\n";
                str_sql += "\t" + " , B_content " + "\r\n";
                str_sql += "\t" + " , B_author " + "\r\n";
                str_sql += "\t" + " , EnterDateTime " + "\r\n";
                str_sql += "\t" + " , EnterUser " + "\r\n";
                str_sql += "\t" + " FROM " + "\r\n";
                str_sql += "\t" + " TJ_Board " + "\r\n";
                str_sql += "\t" + " WHERE " + "\r\n";
                str_sql += "\t" + " idx = " + idx + " " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " EnterUser = N'" + u_id + "' " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " DelFlg IS NULL " + "\r\n";
                str_sql += "\t" + " ; " + "\r\n";
                System.Diagnostics.Debug.WriteLine(str_sql);

                BoardDTO? board = new();
                using (SqlConnection sqlConnection = new SqlConnection(connStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand(str_sql, sqlConnection))
                    {
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                board.B_title = reader.IsDBNull("B_title") ? String.Empty : reader.GetString("B_title");
                                board.B_content = reader.IsDBNull("B_content") ? String.Empty : reader.GetString("B_content");
                                board.B_author = reader.IsDBNull("B_author") ? String.Empty : reader.GetString("B_author");
                                board.EnterDateTime = reader.GetDateTimeOffset(reader.GetOrdinal("EnterDateTime"));
                                board.EnterUser = reader.IsDBNull("EnterUser") ? String.Empty : reader.GetString("EnterUser");
                            }
                        }
                        await sqlCommand.DisposeAsync();
                    }
                    await sqlConnection.CloseAsync();
                }

                return board;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine("InnerException : " + ex.InnerException.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteBoardContent(string? CatCls, int idx, string? u_id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                await sqlConnection.OpenAsync();
                SqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    Int32 result = 0;
                    String str_sql = "";
                    str_sql += "\t" + " UPDATE " + "\r\n";
                    str_sql += "\t" + " TJ_Comment " + "\r\n";
                    str_sql += "\t" + " SET " + "\r\n";
                    str_sql += "\t" + " DelFlg = 'D' " + "\r\n";
                    str_sql += "\t" + " , ModifyDateTime = GETDATE() " + "\r\n";
                    str_sql += "\t" + " WHERE " + "\r\n";
                    str_sql += "\t" + " BoardCatCls = '" + CatCls + "' " + "\r\n";
                    str_sql += "\t" + " AND " + "\r\n";
                    str_sql += "\t" + " BoardIdx = " + idx + " " + "\r\n";
                    str_sql += "\t" + " AND " + "\r\n";
                    str_sql += "\t" + " DelFlg IS NULL " + "\r\n";
                    str_sql += "\t" + " ; " + "\r\n";
                    System.Diagnostics.Debug.WriteLine(str_sql);

                    using (SqlCommand sqlCommand = new SqlCommand(str_sql, sqlConnection, transaction))
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                        await sqlCommand.DisposeAsync();
                    }

                    str_sql = "";
                    str_sql += "\t" + " UPDATE " + "\r\n";
                    str_sql += "\t" + " TJ_Board " + "\r\n";
                    str_sql += "\t" + " SET " + "\r\n";
                    str_sql += "\t" + " DelFlg = 'D' " + "\r\n";
                    str_sql += "\t" + " , ModifyDateTime = GETDATE() " + "\r\n";
                    str_sql += "\t" + " , ModifyUser = N'" + u_id + "' " + "\r\n";
                    str_sql += "\t" + " WHERE " + "\r\n";
                    str_sql += "\t" + " CatCls = '" + CatCls + "' " + "\r\n";
                    str_sql += "\t" + " AND " + "\r\n";
                    str_sql += "\t" + " idx = " + idx + " " + "\r\n";
                    str_sql += "\t" + " AND " + "\r\n";
                    str_sql += "\t" + " DelFlg IS NULL " + "\r\n";
                    str_sql += "\t" + " ; " + "\r\n";
                    System.Diagnostics.Debug.WriteLine(str_sql);

                    using (SqlCommand sqlCommand = new SqlCommand(str_sql, sqlConnection, transaction))
                    {
                        result = await sqlCommand.ExecuteNonQueryAsync();
                        await sqlCommand.DisposeAsync();
                    }
                    transaction.Commit();

                    await sqlConnection.CloseAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine("InnerException : " + ex.InnerException.Message);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<int> UpdateBoardContent(string? CatCls, int idx, string? B_title, string? B_content, string u_id)
        {
            try
            {
                Int32 result = 0;
                String str_sql = "";
                str_sql += "\t" + " UPDATE " + "\r\n";
                str_sql += "\t" + " TJ_Board " + "\r\n";
                str_sql += "\t" + " SET " + "\r\n";
                str_sql += "\t" + " B_title = N'" + B_title + "' " + "\r\n";
                str_sql += "\t" + " , B_content = N'" + B_content + "' " + "\r\n";
                str_sql += "\t" + " , ModifyDateTime = GETDATE() " + "\r\n";
                str_sql += "\t" + " , ModifyUser = N'" + u_id + "' " + "\r\n";
                str_sql += "\t" + " WHERE " + "\r\n";
                str_sql += "\t" + " CatCls = '" + CatCls + "' " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " idx = " + idx + " " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " EnterUser = N'" + u_id + "' " + "\r\n";
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

        public async Task<List<CommentDTO>> GetCommentList(String CatCls, Int32? BoardIdx)
        {
            try
            {
                String str_sql = "";
                str_sql += "\t" + " SELECT " + "\r\n";
                str_sql += "\t" + " idx " + "\r\n";
                str_sql += "\t" + " , ParentIdx " + "\r\n";
                str_sql += "\t" + " , C_order " + "\r\n";
                str_sql += "\t" + " , C_depth " + "\r\n";
                str_sql += "\t" + " , CASE WHEN DelFlg = 'D' " + "\r\n";
                str_sql += "\t" + " THEN " + "\r\n";
                str_sql += "\t" + " CASE WHEN C_depth = 1 " + "\r\n";
                str_sql += "\t" + " THEN N'삭제된 댓글입니다' " + "\r\n";
                str_sql += "\t" + " ELSE N'삭제된 답글입니다' " + "\r\n";
                str_sql += "\t" + " END " + "\r\n";
                str_sql += "\t" + " ELSE C_content " + "\r\n";
                str_sql += "\t" + " END AS C_content " + "\r\n";
                str_sql += "\t" + " , C_author " + "\r\n";
                str_sql += "\t" + " , EnterDateTime " + "\r\n";
                str_sql += "\t" + " , EnterUser " + "\r\n";
                str_sql += "\t" + " FROM " + "\r\n";
                str_sql += "\t" + " TJ_Comment " + "\r\n";
                str_sql += "\t" + " WHERE " + "\r\n";
                str_sql += "\t" + " BoardCatCls = '" + CatCls + "' " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " BoardIdx = " + BoardIdx + " " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " DelFlg IS NULL " + "\r\n";
                str_sql += "\t" + " AND " + "\r\n";
                str_sql += "\t" + " C_report < 5 " + "\r\n";
                str_sql += "\t" + " ORDER BY C_order, C_depth, idx " + "\r\n";
                str_sql += "\t" + " ; " + "\r\n";
                System.Diagnostics.Debug.WriteLine(str_sql);

                List<CommentDTO>? list = new();
                using (SqlConnection sqlConnection = new SqlConnection(connStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand(str_sql, sqlConnection))
                    {
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                CommentDTO comment = new();
                                comment.idx = reader.GetInt32("idx");
                                comment.ParentIdx = reader.IsDBNull("ParentIdx") ? null : reader.GetInt32("ParentIdx");
                                comment.C_order = reader.IsDBNull("C_order") ? 0 : reader.GetInt32("C_order");
                                comment.C_depth = reader.IsDBNull("C_depth") ? 0 : reader.GetInt32("C_depth");
                                comment.C_content = reader.IsDBNull("C_content") ? String.Empty : reader.GetString("C_content");
                                comment.C_author = reader.IsDBNull("C_author") ? String.Empty : reader.GetString("C_author");
                                comment.EnterDateTime = reader.GetDateTimeOffset(reader.GetOrdinal("EnterDateTime"));
                                comment.EnterUser = reader.IsDBNull("EnterUser") ? String.Empty : reader.GetString("EnterUser");
                                list.Add(comment);
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
                if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine("InnerException : " + ex.InnerException.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
