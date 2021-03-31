namespace DataAccessLayer.Migrations
{
    using EntityModel;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccessLayer.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataAccessLayer.DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //Initialize Must Rows
            if (!context.Cases.Any())
            {
                context.Cases.Add(new Case() { Name = "پرونده ها", ParentCase = null });
            }

            if (!context.Departements.Any())
            {
                context.Departements.Add(new Department() { Name = "دپارتمان ها", ParentDepartement = null });
            }

            if (!context.Organization.Any())
            {
                context.Organization.Add(new Organization() { Name = "سازمان ها", ParentOrganization = null });
            }

            //Add Some person
            if (!context.Persons.Any(c => c.FirstName == "Mojtaba"))
            {
                context.Persons.Add(new Person() { FirstName = "Mojtaba", LastName = "Nikonejad", Avatar = new ArchiveFile() });
            }


            //Add a user
            if (!context.Users.Any(c => c.UserName == "mojtaba"))
            {
                context.Users.Add(new User() { FirstName = "مجتبی", LastName = "نیکونژاد", UserName = "mojtaba", password = "347B4BDF9F4D38CF20795A71355D0432", Avatar = new ArchiveFile(), BirthDay = DateTime.Now, IsAdmin = true });
            }

            context.SaveChanges();
        }
    }
}
