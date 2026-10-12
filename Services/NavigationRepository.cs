using Dapper;
using Microsoft.Data.SqlClient;

namespace mvcStore.Services;

// 負責從資料庫查詢登入使用者與選單資料。
// public sealed class NavigationRepository
public sealed class NavigationRepository
{
    public readonly IConfiguration _configuration;

    public NavigationRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    // 讀取連線字串，建立資料庫連線物件。
    // Dapper 執行查詢時會開啟連線。
    private SqlConnection Connect()
    {
        string? connectionString = _configuration.GetConnectionString("mvcstore");
        if (connectionString == null)
        {
            throw new InvalidOperationException("Missing ConnectionStrings:mvcstore.");
        }
        SqlConnection connection = new SqlConnection(connectionString);
        return connection;
    }



    // 依帳號與密碼查詢啟用中的使用者；找不到時回傳 null。
    public async Task<LoginUser> myLogin(string userId, string password)
    {
        // 1. 建立連線；離開方法時，using 會自動釋放連線資源。
        using SqlConnection connection = Connect();

        // 2. 準備 SQL。欄位別名對應 LoginUser 的屬性名稱。
        string sql = """
               SELECT mno AS UserId, mname AS UserName, code_user AS CodeUser
               FROM z_org_user
               WHERE mno = @userId AND pass_user = @password AND is_enabled = 1        
        """;

        // 3. 準備參數，對應 SQL 中的 @userId 與 @password。
        var parameters = new
        {
            userId = userId,
            password = password
        };

        // 4. 執行查詢，等待結果後回傳使用者資料。
        LoginUser? user = await connection.QueryFirstOrDefaultAsync<LoginUser>(sql, parameters);

        return user;
    }

    // 依使用者代碼查詢啟用中的選單，並依 no_sort 排序。
    public async Task<List<MenuRow>> Menu(string codeUser)
    {
        // 1. 建立資料庫連線。
        using SqlConnection connection = Connect();

        // 2. 準備查詢選單的 SQL。
        string sql = """
            SELECT rowid, mno, mname, parentno, no_sort, controllername, actionname, routename
            FROM z_sys_program
            WHERE is_enabled = 1 AND code_no = @codeUser
            ORDER BY no_sort
            """;

        // 3. 準備參數，對應 SQL 中的 @codeUser。
        var parameters = new
        {
            codeUser = codeUser
        };

        // 4. 執行查詢，取得多筆選單資料。
        IEnumerable<MenuRow> rows = await connection.QueryAsync<MenuRow>(sql, parameters);

        // 5. 將查詢結果轉成 List 後回傳。
        List<MenuRow> menuItems = rows.ToList();

        return menuItems;
    }
}
// 登入查詢回傳的使用者資料。
public sealed class LoginUser
{
    public string UserId { get; set; } = "";
    public string? UserName { get; set; }
    public string? CodeUser { get; set; }
}
// 選單查詢回傳的資料；屬性名稱對應資料庫欄位。
public sealed class MenuRow
{
    public int rowid { get; set; }
    public string? mno { get; set; }
    public string? mname { get; set; }
    public string? parentno { get; set; }
    public string? no_sort { get; set; }
    public string? controllername { get; set; }
    public string? actionname { get; set; }
    public string? routename { get; set; }
}
