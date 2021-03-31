using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityModel;
using DBProvider;

namespace BusinessModel
{
    public class EmployeeManager : DBProvider<Employee>
    {
        public EmployeeManager() : base(new DatabaseContext())
        {
        }

        public void EditPostDepartment(Guid employeeId, List<Guid> departments, List<Guid> post)
        {
            Employee currentEmployee = GetEmployeeWithAllRelations(employeeId);

            var departmentDbset = DBC.Set<Department>();
            var postDbset = DBC.Set<Post>();
            var postDepartmentDbset = DBC.Set<Post_Department>();

            //Remove All Post_Departments
            var deletedList = new List<Post_Department>();
            foreach (var item in currentEmployee.PostDepartments)
            {
                deletedList.Add(item);
            }

            foreach (var item in deletedList)
            {
                currentEmployee.PostDepartments.Remove(item);
            }

            //Add Changes again
            int counter = 0;
            foreach (var guid in departments)
            {
                //This make without department to be removed
                if (guid != Guid.Empty)
                {
                    var item = departmentDbset.First(c => c.Id == guid);
                    //if department not found this would remove it from list and nothing will happen
                    if (item == null)
                    {
                        continue;
                    }

                    Post_Department postDepartment = new Post_Department
                    {
                        Department = item

                    };

                    //Set Post or without posts
                    try
                    {
                        Guid a = post[counter];
                        postDepartment.Post = postDbset.First(x => x.Id == a);
                    }
                    catch
                    {
                        postDepartment.Post = null;
                    }

                    //Check redundancy objects to not add it to db
                    if (postDepartment.Post != null)
                    {
                        if (
                            !currentEmployee.PostDepartments.Any(
                                x =>
                                    x.Department.Id == postDepartment.Department.Id && x.Post != null &&
                                    x.Post.Id == postDepartment.Post.Id))
                        {
                            var existedPostDepartment =
                                postDepartmentDbset.Where(
                                    x =>
                                        x.Department.Id == postDepartment.Department.Id &&
                                        x.Post.Id == postDepartment.Post.Id);
                            currentEmployee.PostDepartments.Add(existedPostDepartment.Any()
                               ? existedPostDepartment.First()
                               : postDepartment);
                        }
                    }
                    else
                    {
                        if (
                            !currentEmployee.PostDepartments.Any(
                                x =>
                                    x.Department.Id == postDepartment.Department.Id &&
                                    x.Post == null))
                        {
                            var existedPostDepartment =
                                postDepartmentDbset.Where(
                                    x =>
                                        x.Department.Id == postDepartment.Department.Id &&
                                        x.Post == null);
                            currentEmployee.PostDepartments.Add(existedPostDepartment.Any()
                                ? existedPostDepartment.First()
                                : postDepartment);
                        }
                    }
                }
                counter++;
            }
        }
        //Employees with any department
        public List<Employee> NoneDepartment() => Read(x => x.PostDepartments.Count == 0);

        //Add departments for current employee
        public void AddPostDepartment(Employee currentEmployee, List<Guid> departments, List<Guid> post)
        {
            currentEmployee.PostDepartments = new List<Post_Department>();
            var departmentDbset = DBC.Set<Department>();
            var postDbset = DBC.Set<Post>();
            var postDepartmentDbset = DBC.Set<Post_Department>();

            int counter = 0;
            foreach (var guid in departments)
            {
                //This make without department to be removed
                if (guid != Guid.Empty)
                {
                    var item = departmentDbset.First(c => c.Id == guid);
                    //if department not found this would remove it from list and nothing will happen
                    if (item == null)
                    {
                        continue;
                    }

                    Post_Department postDepartment = new Post_Department
                    {
                        Department = item

                    };

                    //Set Post or without posts
                    try
                    {
                        Guid a = post[counter];
                        postDepartment.Post = postDbset.First(x => x.Id == a);
                    }
                    catch
                    {
                        postDepartment.Post = null;
                    }

                    //Check redundancy objects to not add it to db
                    if (postDepartment.Post != null)
                    {
                        if (
                            !currentEmployee.PostDepartments.Any(
                                x =>
                                    x.Department.Id == postDepartment.Department.Id && x.Post != null &&
                                    x.Post.Id == postDepartment.Post.Id))
                        {
                            var existedPostDepartment =
                                postDepartmentDbset.Where(
                                    x =>
                                        x.Department.Id == postDepartment.Department.Id &&
                                        x.Post.Id == postDepartment.Post.Id);
                            currentEmployee.PostDepartments.Add(existedPostDepartment.Any()
                               ? existedPostDepartment.First()
                               : postDepartment);
                        }
                    }
                    else
                    {
                        if (
                            !currentEmployee.PostDepartments.Any(
                                x =>
                                    x.Department.Id == postDepartment.Department.Id &&
                                    x.Post == null))
                        {
                            var existedPostDepartment =
                                postDepartmentDbset.Where(
                                    x =>
                                        x.Department.Id == postDepartment.Department.Id &&
                                        x.Post == null);
                            currentEmployee.PostDepartments.Add(existedPostDepartment.Any()
                                ? existedPostDepartment.First()
                                : postDepartment);
                        }
                    }
                }
                counter++;
            }
        }

        public List<Employee> GetDepartmentEmployees(Department department, Guid? Post)
        {
            return Post == null || Post == Guid.Empty
                ? dbSet.Where(x => x.PostDepartments.Any(c => c.Department.Id == department.Id && c.Post == null)).ToList()
                : dbSet.Where(x => x.PostDepartments.Any(c => c.Department.Id == department.Id && c.Post.Id == Post))
                    .ToList();
        }
        public List<Tuple<Employee, Post, Department>> GetDepartmentEmployees(Department department)
        {
            return (from item in dbSet.Where(x => x.PostDepartments.Any(c => c.Department.Id == department.Id)).ToList() let postDepartments = item.PostDepartments.Where(x => x.Department.Id == department.Id).ToList() where postDepartments.Count > 0 from postDepartment in postDepartments select postDepartment.Post != null ? new Tuple<Employee, Post, Department>(item, postDepartment.Post, department) : new Tuple<Employee, Post, Department>(item, new Post() { Name = "بدون سمت" }, department)).ToList();
        }


        private Employee GetEmployeeWithAllRelations(Guid id)
        {
            return dbSet.Where(x => x.Id == id).Include(z => z.PostDepartments.Select(u => u.Department)).Include(z => z.PostDepartments.Select(u => u.Post)).First();
        }
    }
}
