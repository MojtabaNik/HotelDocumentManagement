using DataAccessLayer;
using DBProvider;
using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    public class UserManager : DBProvider<User>
    {
        public UserManager() : base(new DatabaseContext())
        {

        }

        public void ClearOrganizationFilters(User currentUser)
        {

        }

        public void AddFilterOrganization(Guid userId, List<Guid> organizations)
        {
            User currentUser = dbSet.Find(userId);
            var organDbset = DBC.Set<Organization>();

            foreach (var item in organDbset.Where(c => organizations.Contains(c.Id)).ToList())
            {
                currentUser.FilterOrganizations.Add(item);
            }


            foreach (var item in currentUser.FilterOrganizations.Where(s => !organizations.Contains(s.Id)).ToList())
            {
                currentUser.FilterOrganizations.Remove(item);
            }
        }

        public void AddFilterPerson(Guid userId, List<Guid> peopleList)
        {
            User currentUser = dbSet.Find(userId);
            var personDbset = DBC.Set<Person>();

            foreach (var item in personDbset.Where(c => peopleList.Contains(c.Id)).ToList())
            {
                currentUser.FilterPeople.Add(item);
            }


            foreach (var item in currentUser.FilterPeople.Where(s => !peopleList.Contains(s.Id)).ToList())
            {
                currentUser.FilterPeople.Remove(item);
            }
        }


        public bool Authentication(User user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.password))
            {
                throw new Exception("نام کاربری یا کلمه عبور نمی تواند خالی باشد.");
            }
            List<User> foundUsers = Read(x => x.UserName.ToLower() == user.UserName.ToLower());
            User dbUser = foundUsers.Count != 0 ? foundUsers.First() : null;

            if (dbUser == null)
            {
                throw new Exception("نام کاربری یافت نشد.");
            }
            return dbUser.password == user.password.ToPassword();
        }

        public List<Organization> GetUserShowableOrganizations(User user)
        {
            return DBC.Set<Organization>().ToList().Where(item => user.FilterOrganizations.All(x => x.Id != item.Id)).ToList();
        }

        public List<Person> GetUserShowablePeopleList(User user)
        {
            PersonManager personManager = new PersonManager();
            return personManager.NoneOrganization().Where(item => user.FilterPeople.All(x => x.Id != item.Id)).ToList();
        }
    }
}

