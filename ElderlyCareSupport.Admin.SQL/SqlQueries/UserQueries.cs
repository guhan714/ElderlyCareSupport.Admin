namespace ElderlyCareSupport.Admin.SQL.SqlQueries;

public static class UserQueries
{
    public static string GetAllUsersWithPaging { get; } = @"
                                                                  SELECT FirstName, LastName, Email, Password, PhoneNumber, Address, City, Country, Region, PostalCode, IsActive, UserType, Gender FROM ElderCareAccount
                                                                  WHERE (@Search IS NULL OR FirstName LIKE '%' + @Search + '%')
                                                                  ORDER BY {0} {1}
                                                                  OFFSET @offSet ROWS FETCH NEXT @pageSize ROWS ONLY;
                                                          ";

    public static string GetUserById { get; } = @"
                                                    SELECT FirstName, LastName, Email, Gender, PhoneNumber, Address, City, Country, Region, PostalCode, IsActive, USER FROM ElderCareAccount
                                                    WHERE Email = @Email
                                                  ";
}