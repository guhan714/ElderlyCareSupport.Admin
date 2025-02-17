namespace ElderlyCareSupport.Admin.SQL.SqlQueries;

public static class TaskQueries
{
    public static string GetAllTaskDetails =>
        @"SELECT Tc.TaskId, Tc.TaskName, Tc.TaskDescription, Tc.StartDate, Tc.EndDate, Tc.TaskStatusId, Tc.ElderlyPersonId, Tc.CreatedDate, Tc.UpdatedDate, Ec.ID, Ec.FIRSTNAME, Ec.LASTNAME, Ec.EMAIL, Ec.PHONENUMBER, Ec.ADDRESS, Ec.CITY
                                        FROM Task Tc INNER JOIN Task_Assignment Ta
                                        ON Tc.ElderlyPersonId = Ta.UserId
                                        INNER JOIN ElderCareAccount Ec 
                                        ON Ec.Id = Ta.AssignedVolunteerId
                                                        WHERE (TaskStatusId IS NULL OR CAST(TaskStatusId AS NVARCHAR) LIKE '%' + @SearchTerm + '%')
                                                        ORDER BY {0} {1}
                                                        OFFSET @OffSet ROWS FETCH NEXT @PageSize ROWS ONLY;";
}

