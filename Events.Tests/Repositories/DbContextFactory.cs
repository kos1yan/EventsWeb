using DataAccessLayer.DbContext;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Tests.Repositories
{
    public class DbContextFactory
    {
        public static EventContext GetDatabaseContext(string name)
        {
            var options = new DbContextOptionsBuilder<EventContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;

            var databaseContext = new EventContext(options);
            databaseContext.Database.EnsureCreated();
            
            if (databaseContext.Events.Count() <= 10)
            {
                databaseContext.Events.AddRange(
                    new Event
                    {
                        Id = new Guid("e0e44b16-757f-48d9-ac03-61fa3662559c"),
                        Name = "Name1",
                        Description = "Description1",
                        Adress = "Adress1",
                        Date = "Date1",
                        MaxMemberCount = 15,
                        MemberCount = 1,
                        CategoryId = 1
                    },
                    new Event
                    {
                        Id = new Guid("ec3a6eb9-726e-4e69-aa5c-78f2d1b75bb2"),
                        Name = "Name2",
                        Description = "Description2",
                        Adress = "Adress2",
                        Date = "Date2",
                        MaxMemberCount = 15,
                        MemberCount = 1,
                        CategoryId = 1
                    },
                    new Event()
                    {
                        Id = new Guid("c69ef73d-dd75-49af-962f-c193eb670709"),
                        Name = "Name3",
                        Description = "Description3",
                        Adress = "Adress3",
                        Date = "Date3",
                        MaxMemberCount = 15,
                        MemberCount = 1,
                        CategoryId = 2
                    },
                    new Event()
                    {
                        Id = new Guid("c1d07e8e-4b1a-4fb8-80ae-5db1a9d3ed56"),
                        Name = "4",
                        Description = "Description4",
                        Adress = "Adress4",
                        Date = "Date4",
                        MaxMemberCount = 15,
                        MemberCount = 0,
                        CategoryId = 1   
                    }
                );
            }

            if (databaseContext.Members.Count() <= 0)
            {
                databaseContext.Members.AddRange(
                   new Member
                   {
                       Id = new Guid("d8541560-f9ff-4484-b6cd-544498669cc7"),
                       Name = "Name1",
                       Surname = "Surname1",
                       DateOfBirth = "DateOfBirth1",
                       Email = "Email1",
                       EventId = new Guid("e0e44b16-757f-48d9-ac03-61fa3662559c"),
                       UserId = "dd1513b5-6f40-49f4-b90d-18f6efd50a76"
                   },
                   new Member
                   {
                       Id = new Guid("93798f67-1fc8-429d-b158-75f92fa0b244"),
                       Name = "Name1",
                       Surname = "Surname1",
                       DateOfBirth = "DateOfBirth1",
                       Email = "Email1",
                       EventId = new Guid("e0e44b16-757f-48d9-ac03-61fa3662559c"),
                       UserId = "e5e81733-0788-4fde-9212-5369defa8503"
                   },
                   new Member
                   {
                       Id = new Guid("85c284f6-386d-4341-acc1-e85a3b1daa84"),
                       Name = "Name1",
                       Surname = "Surname1",
                       DateOfBirth = "DateOfBirth1",
                       Email = "Email1",
                       EventId = new Guid("c1d07e8e-4b1a-4fb8-80ae-5db1a9d3ed56"),
                       UserId = "e6f1b229-fbdd-424e-bb12-81a4a4d341f3"
                   }
               );
            }

            if (databaseContext.Categories.Count() <= 0)
            {
                databaseContext.Categories.AddRange(
                   new Category
                   {
                       Id = 1,
                       Name = "Sport"
                   },
                   new Category
                   {
                       Id = 2,
                       Name = "Travel"
                   }
               );
            }

            databaseContext.SaveChanges();

            return databaseContext;
        }
    }
}

