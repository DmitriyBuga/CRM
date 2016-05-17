using CRM.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models.Concrete
{
    public class CommonData
    {
        IRepository repository;
        public CommonData(IRepository repo)
        {
            repository = repo;
        }
        public List<CommonDirJSON> GetStatuses()
        {
            List<CommonDirJSON> statuses = new List<CommonDirJSON>();
            foreach (Statuses reg in repository.Statuses)
                statuses.Add(new CommonDirJSON() { id = reg.id, name = reg.name });
            return statuses;
        }
        public List<CommonDirJSON> GetUsers()
        {
            List<CommonDirJSON> users = new List<CommonDirJSON>();
            foreach (Users reg in repository.Users)
                users.Add(new CommonDirJSON() { id = reg.id, name = reg.name });
            return users;
        }
        public List<CommonDirJSON> GetTasks()
        {
            List<CommonDirJSON> regions = new List<CommonDirJSON>();
            foreach (Tasks reg in repository.Tasks)
                regions.Add(new CommonDirJSON() { id = reg.id, name = reg.name });
            return regions;
        }
        public List<CommonDirJSON> GetManagers()
        {
            List<CommonDirJSON> regions = new List<CommonDirJSON>();
            foreach (Managers reg in repository.Managers)
                regions.Add(new CommonDirJSON() { id = reg.id, name = reg.name });
            return regions;

        }
        public List<CommonDirJSON> GetCustomers()
        {
            List<CommonDirJSON> regions = new List<CommonDirJSON>();
            foreach (Customers reg in repository.Customers)
                regions.Add(new CommonDirJSON() { id = reg.id, name = reg.name });
            return regions;

        }

    }
}