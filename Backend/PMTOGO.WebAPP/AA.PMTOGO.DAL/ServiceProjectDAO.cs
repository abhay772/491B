using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Models.Entities;
using System.Data.SqlClient;

namespace AA.PMTOGO.DAL;

public class ServiceProjectDAO : IServiceProjectDAO
{
    private readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.ProjectDB;Trusted_Connection=True";

    public Result SaveProject(string Username, double EvalChange, double OriginalEval, ProjectDetail projectDetail)
    {
        var result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "SELECT ProjectName FROM Projects WHERE username = @Username";

            var command = new SqlCommand(sqlQuery, connection);

            command.Parameters.AddWithValue("@Username", Username);

            List<string> projectNames = new List<string>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string projectName = (string)reader["ProjectName"];
                    projectNames.Add(projectName);
                }
            }

            if (projectNames.Contains(projectDetail.ProjectName))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Invalid Project Name";
                return result;
            } 


            if (projectNames.Count < 3)
            {
                string startDateString = projectDetail.StartDate.ToString("yyyy-MM-dd");
                string endDateString = projectDetail.EndDate.ToString("yyyy-MM-dd");
                string timeString = projectDetail.ServiceTime.ToString("HH:mm");

                sqlQuery = $@"
                            INSERT INTO Projects
                            (ProjectName, ServiceIDs, StartDate, EndDate, ServiceTime, Budget, EvaluationChange, OriginalEvaluation, Username)
                            VALUES
                            (@ProjectName, @ServiceIDs, @StartDate, @EndDate, @ServiceTime, @Budget, @EvalChange, @OriginalEval, @Username)";

                command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@ProjectName", projectDetail.ProjectName);
                command.Parameters.AddWithValue("@ServiceIDs", string.Join(",", projectDetail.ServiceIDs!));
                command.Parameters.AddWithValue("@StartDate", startDateString);
                command.Parameters.AddWithValue("@EndDate", endDateString);
                command.Parameters.AddWithValue("@ServiceTime", timeString);
                command.Parameters.AddWithValue("@Budget", projectDetail.Budget);
                command.Parameters.AddWithValue("@EvalChange", EvalChange);
                command.Parameters.AddWithValue("@OriginalEval", OriginalEval);
                command.Parameters.AddWithValue("@Username", Username);



                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows == 1)
                {
                    result.IsSuccessful = true;
                    return result;
                }
            }
        }

        result.IsSuccessful = false;
        result.ErrorMessage = "Unable to add Project";
        return result;
    }

    public Result LoadProjects(string username)
    {
        var result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = @"
            SELECT
                ProjectName, ServiceIDs, StartDate, EndDate, ServiceTime, Budget, EvaluationChange, OriginalEvaluation
            FROM
                Projects
            WHERE
                Username = @username";

            var command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@Username", username);

            using (var reader = command.ExecuteReader())
            {
                List<Project> projects = new List<Project>();

                while (reader.Read())
                {
                    Project saveProjectDTO = new Project()
                    {
                        ProjectDetail = new ProjectDetail
                        {
                            ProjectName = reader["ProjectName"].ToString()!,
                            ServiceIDs = reader["ServiceIDs"].ToString()!.Split(',').Select(int.Parse).ToList(),
                            StartDate = DateOnly.Parse(reader["StartDate"].ToString()!),
                            EndDate = DateOnly.Parse(reader["EndDate"].ToString()!),
                            ServiceTime = TimeOnly.Parse(reader["ServiceTime"].ToString()!),
                            Budget = double.Parse(reader["Budget"].ToString()!),
                        },

                        EvalChange = double.Parse(reader["EvaluationChange"].ToString()!),
                        OriginalEval = double.Parse(reader["OriginalEvaluation"].ToString()!)
                    };

                    projects.Add(saveProjectDTO);
                }

                if (projects.Count > 0)
                {
                    result.Payload = projects;
                    result.IsSuccessful = true;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "No projects found";
                }
            }
        }

        return result;
    }

    public Result DeleteProject(string Username, string projectName)
    {
        var result = new Result();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string sqlQuery = "DELETE FROM Projects WHERE Username = @Username AND ProjectName = @ProjectName";

            using (var command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@Username", Username);
                command.Parameters.AddWithValue("@ProjectName", projectName);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 1)
                {
                    result.IsSuccessful = true;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Unable to delete project";
                }
            }
        }

        return result;
    }

}
