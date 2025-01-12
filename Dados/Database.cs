using Microsoft.Data.SqlClient;
using Dapper;

namespace LI4.Dados;

public class Database
{
    private SqlConnection Connection;
    private static Database? _Instance;

    private Database()
    {
        this.Connection = new SqlConnection("Server=localhost;User Id=SA;Password=V3ry$3cur3Pa$$w0rd;Database=WeaponsRUs;Encrypt=false");
    }

    public static Database Instance
    {
        get
        {
            if (Database._Instance == null)
                Database._Instance = new Database();
            return Database._Instance;
        }
    }

    public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
    {
        return (await this.Connection.QueryAsync<T>(sql, parameters)).ToList();
    }

    public async Task SaveData<T>(string sql, T parameters)
    {
        await this.Connection.ExecuteAsync(sql, parameters);
    }
}
