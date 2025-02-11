namespace ElderlyCareSupport.Admin.SQL.SqlQueries;

public static class TaskQueries
{
    public static string GetAllTaskDetails =>
        @"SELECT Tc.TaskId, Tc.TaskName, Tc.TaskDescription, Tc.StartDate, Tc.EndDate, Tc.TaskStatusId, Tc.ElderlyPersonId, Tc.CreatedDate, Tc.UpdatedDate, Ec.ID, Ec.FIRSTNAME, Ec.LASTNAME, Ec.EMAIL, Ec.PHONENUMBER, Ec.ADDRESS, Ec.CITY
                                FROM Task Tc INNER JOIN ElderCareAccount Ec 
                                                ON Tc.ElderlyPersonId = Ec.Id
                                                WHERE (@SearchTerm = (TRUE OR FALSE) OR @SearchTerm LIKE '%' + @SearchValue  + '%')
                                                ORDER BY {0} {1}
                                                OFFSET @OffSet FETCH NEXT @PageSize ROWS ONLY";
}