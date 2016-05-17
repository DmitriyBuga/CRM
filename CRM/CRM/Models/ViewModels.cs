using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class CommonDirJSON
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class TicketsModel
    {
        public List<ViewTickets> tickets { get; set; }
        public List<CommonDirJSON> users { get; set; }
        public List<CommonDirJSON> customers { get; set; }
        public List<CommonDirJSON> statuses { get; set; }
        public List<CommonDirJSON> tasks { get; set; }
        public List<CommonDirJSON> managers { get; set; }
    }
    public class ViewManagers
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int cust_id { get; set; }
    }
    public class ViewCustomers
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string adress { get; set; }
    }
    public class ViewUsers
    {
        public int id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string position { get; set; }
        public Nullable<int> department_id { get; set; }
        public int role_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
    }
    public class ViewTickets
    {
        public int id { get; set; }
        public int department_id { get; set; }
        public int user_id { get; set; }
        public int status_id { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public Nullable<System.DateTime> mail_data { get; set; }
        public string reference { get; set; }
        public string responce { get; set; }
        public int? parent_id { get; set; }
        public int task_id { get; set; }
        public int manager_id { get; set; }
        public int cust_id { get; set; }
        public int level { get; set; }
        public ViewTickets children { get; set; }
    }
}