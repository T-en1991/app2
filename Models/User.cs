namespace app2.Models;

public class User
{
    public int id { get; set; }
    public string name { get; set; }
    public DateTime create_time { get; set; }
    public DateTime update_time { get; set; }
    public bool is_delete { get; set; }
    public string password { get; set; }
}