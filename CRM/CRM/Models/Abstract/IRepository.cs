using CRM.Models.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models.Abstract
{
    public interface IRepository
    {
        MiniCRMEntities DbContext { get; }
        IQueryable<Statuses> Statuses { get; }
        IQueryable<Users> Users { get; }
        IQueryable<Tickets> Tickets { get; }
        IQueryable<Customers> Customers { get; }
        IQueryable<Managers> Managers { get; }
        IQueryable<Tasks> Tasks { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<Images> Images { get; }
        void CreateNewUser(Users user);
        void DeleteRecord<T>(T dbEntry);
        T CreateRecord<T>(T dbEntry);
        void UpdateRecord<T>(T dbEntry) where T : class;
        void SaveChanges<T>() where T : class;

    }
}
