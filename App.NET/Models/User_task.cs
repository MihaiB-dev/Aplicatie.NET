namespace App.NET.Models
{
    public class User_task
    {
        public int Id { get; set; }
        public string User_id { get; set; }
        public int Task_id { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual Task_table? Task { get; set; }
    }
}
