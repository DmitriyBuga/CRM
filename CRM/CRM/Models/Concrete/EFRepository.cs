using CRM.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CRM.Models.Concrete
{
   
    public class EFRepository : IRepository
    {
        private EFDbContext dbContext;
        public EFRepository(EFDbContext context)
        {
            dbContext = context;
        }
        public IQueryable<Users> Users
        {
            get { return dbContext.Users; }
        }
        public IQueryable<Managers> Managers
        {
            get { return dbContext.Managers; }
        }
        public IQueryable<Tasks> Tasks
        {
            get { return dbContext.Tasks; }
        }
        public IQueryable<Tickets> Tickets
        {
            get { return dbContext.Tickets; }
        }
        public IQueryable<Customers> Customers
        {
            get { return dbContext.Customers; }
        }
        public IQueryable<Role> Roles
        {
            get { return dbContext.Roles; }
        }
        public IQueryable<Images> Images
        {
            get { return dbContext.Images; }
        }
        public void CreateNewUser(Users user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }
        public void DeleteRecord<T>(T dbEntry)
        {
            DbSet dbSet = dbContext.Set(typeof(T));
            dbSet.Remove(dbEntry);
            dbContext.SaveChanges();
        }
        public T CreateRecord<T>(T dbEntry)
        {
            DbSet dbSet = dbContext.Set(typeof(T));
            dbSet.Add(dbEntry);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //throw;
            }
            return dbEntry;
        }
        public void UpdateRecord<T>(T dbEntry) where T : class
        {
            //dbContext.Entry<T>(dbEntry).State = EntityState.Modified;
            dbContext.SaveChanges();

        }
        
    }
}