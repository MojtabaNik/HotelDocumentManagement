using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DBProvider;
using EntityModel;

namespace BusinessModel
{
    public class PersonManager : DBProvider<Person>
    {
        public PersonManager() : base(new DatabaseContext())
        {
        }
        public List<Person> NoneOrganization() => Read(x => x.PostOrganizations.Count == 0);

        public void EditPostOrganization(Guid personId, List<Guid> organizations, List<Guid> post)
        {
            Person currentPerson = GetPersonWithAllRelations(personId);

            var organDbset = DBC.Set<Organization>();
            var postDbset = DBC.Set<Post>();
            var postOrganizationDbset = DBC.Set<Post_Organization>();

            //Remove All Post_Organizations
            var deletedList = new List<Post_Organization>();
            foreach (var item in currentPerson.PostOrganizations)
            {
                deletedList.Add(item);
            }

            foreach (var item in deletedList)
            {
                currentPerson.PostOrganizations.Remove(item);
            }

            //Add Changes again
            int counter = 0;
            foreach (var guid in organizations)
            {
                //This make without organization to be removed
                if (guid != Guid.Empty)
                {
                    var item = organDbset.First(c => c.Id == guid);
                    //if organization not found this would remove it from list and nothing will happen
                    if (item == null)
                    {
                        continue;
                    }

                    Post_Organization postOrganization = new Post_Organization
                    {
                        Organization = item

                    };

                    //Set Post or without posts
                    try
                    {
                        Guid a = post[counter];
                        postOrganization.Post = postDbset.First(x => x.Id == a);
                    }
                    catch
                    {
                        postOrganization.Post = null;
                    }

                    //Check redundancy objects to not add it to db
                    if (postOrganization.Post != null)
                    {
                        if (
                            !currentPerson.PostOrganizations.Any(
                                x =>
                                    x.Organization.Id == postOrganization.Organization.Id && x.Post != null &&
                                    x.Post.Id == postOrganization.Post.Id))
                        {
                            var existedPostOrganization =
                                postOrganizationDbset.Where(
                                    x =>
                                        x.Organization.Id == postOrganization.Organization.Id &&
                                        x.Post.Id == postOrganization.Post.Id);
                            currentPerson.PostOrganizations.Add(existedPostOrganization.Any()
                               ? existedPostOrganization.First()
                               : postOrganization);
                        }
                    }
                    else
                    {
                        if (
                            !currentPerson.PostOrganizations.Any(
                                x =>
                                    x.Organization.Id == postOrganization.Organization.Id &&
                                    x.Post == null))
                        {
                            var existedPostOrganization =
                                postOrganizationDbset.Where(
                                    x =>
                                        x.Organization.Id == postOrganization.Organization.Id &&
                                        x.Post == null);
                            currentPerson.PostOrganizations.Add(existedPostOrganization.Any()
                                ? existedPostOrganization.First()
                                : postOrganization);
                        }
                    }
                }
                counter++;
            }
        }

        public void AddPostOrganization(Person currentPerson, List<Guid> organizations, List<Guid> post)
        {
            currentPerson.PostOrganizations = new List<Post_Organization>();
            var organDbset = DBC.Set<Organization>();
            var postDbset = DBC.Set<Post>();
            var postOrganizationDbset = DBC.Set<Post_Organization>();

            int counter = 0;
            foreach (var guid in organizations)
            {
                //This make without organization to be removed
                if (guid != Guid.Empty)
                {
                    var item = organDbset.First(c => c.Id == guid);
                    //if organization not found this would remove it from list and nothing will happen
                    if (item == null)
                    {
                        continue;
                    }

                    Post_Organization postOrganization = new Post_Organization
                    {
                        Organization = item

                    };

                    //Set Post or without posts
                    try
                    {
                        Guid a = post[counter];
                        postOrganization.Post = postDbset.First(x => x.Id == a);
                    }
                    catch
                    {
                        postOrganization.Post = null;
                    }

                    //Check redundancy objects to not add it to db
                    if (postOrganization.Post != null)
                    {
                        if (
                            !currentPerson.PostOrganizations.Any(
                                x =>
                                    x.Organization.Id == postOrganization.Organization.Id && x.Post != null &&
                                    x.Post.Id == postOrganization.Post.Id))
                        {
                            var existedPostOrganization =
                                postOrganizationDbset.Where(
                                    x =>
                                        x.Organization.Id == postOrganization.Organization.Id &&
                                        x.Post.Id == postOrganization.Post.Id);
                            currentPerson.PostOrganizations.Add(existedPostOrganization.Any()
                               ? existedPostOrganization.First()
                               : postOrganization);
                        }
                    }
                    else
                    {
                        if (
                            !currentPerson.PostOrganizations.Any(
                                x =>
                                    x.Organization.Id == postOrganization.Organization.Id &&
                                    x.Post == null))
                        {
                            var existedPostOrganization =
                                postOrganizationDbset.Where(
                                    x =>
                                        x.Organization.Id == postOrganization.Organization.Id &&
                                        x.Post == null);
                            currentPerson.PostOrganizations.Add(existedPostOrganization.Any()
                                ? existedPostOrganization.First()
                                : postOrganization);
                        }
                    }
                }
                counter++;
            }
        }

        public List<Person> GetOrganizationPersons(Organization org, Guid? Post)
        {
            return Post == null || Post == Guid.Empty
                ? dbSet.Where(x => x.PostOrganizations.Any(c => c.Organization.Id == org.Id && c.Post == null)).ToList()
                : dbSet.Where(x => x.PostOrganizations.Any(c => c.Organization.Id == org.Id && c.Post.Id == Post))
                    .ToList();
        }

        public List<Tuple<Person, Post, Organization>> GetOrganizationPersons(Organization org)
        {
            return (from item in dbSet.Where(x => x.PostOrganizations.Any(c => c.Organization.Id == org.Id)).ToList() let postOrganizations = item.PostOrganizations.Where(x => x.Organization.Id == org.Id).ToList() where postOrganizations.Count > 0 from postOrganization in postOrganizations select postOrganization.Post != null ? new Tuple<Person, Post, Organization>(item, postOrganization.Post, org) : new Tuple<Person, Post, Organization>(item, new Post() {Name = "بدون سمت"}, org)).ToList();
        }

        private Person GetPersonWithAllRelations(Guid id)
        {
            return dbSet.Where(x => x.Id == id).Include(z => z.PostOrganizations.Select(u => u.Organization)).Include(z => z.PostOrganizations.Select(u => u.Post)).First();
        }

    }
}
