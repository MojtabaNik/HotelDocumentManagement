namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RowId = c.Long(nullable: false),
                        TableName = c.String(),
                        Action = c.Int(nullable: false),
                        Description = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserName = c.String(),
                        password = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                        Role = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.Int(nullable: false),
                        BirthDay = c.DateTime(),
                        Summary = c.String(),
                        Address = c.String(),
                        MobileNumber = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        Avatar_Name = c.String(),
                        Avatar_Content = c.Binary(),
                        Avatar_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        ParentOrganization_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.ParentOrganization_Id)
                .Index(t => t.ParentOrganization_Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.Int(nullable: false),
                        BirthDay = c.DateTime(),
                        Summary = c.String(),
                        Address = c.String(),
                        MobileNumber = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        Avatar_Name = c.String(),
                        Avatar_Content = c.Binary(),
                        Avatar_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Post_Organization",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Organization_Id = c.Guid(),
                        Post_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Organization_Id)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReceivedLetters",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ParentType = c.Int(nullable: false),
                        RowNumber = c.String(),
                        Description = c.String(),
                        LetterDateTime = c.DateTime(nullable: false),
                        ArchiveDateTime = c.DateTime(nullable: false),
                        LastModifyDateTime = c.DateTime(nullable: false),
                        Assortment_LetterType = c.Int(nullable: false),
                        Assortment_Security = c.Int(nullable: false),
                        Assortment_Nature = c.Int(nullable: false),
                        Assortment_Priority = c.Int(nullable: false),
                        Assortment_ContentType = c.Int(nullable: false),
                        Assortment_Situation = c.Int(nullable: false),
                        Assortment_Legalvalidity = c.Int(nullable: false),
                        AppendageContent = c.Binary(),
                        LetterFileContent = c.Binary(),
                        LetterFileAppendageContent = c.Binary(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Sender_Id = c.Guid(),
                        ParentReceiveLetter_Id = c.Guid(),
                        ParentSendLetterOut_Id = c.Guid(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Senders", t => t.Sender_Id)
                .ForeignKey("dbo.ReceivedLetters", t => t.ParentReceiveLetter_Id)
                .ForeignKey("dbo.SendLetters", t => t.ParentSendLetterOut_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Sender_Id)
                .Index(t => t.ParentReceiveLetter_Id)
                .Index(t => t.ParentSendLetterOut_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ReceiveAppendages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        File_Name = c.String(),
                        File_Content = c.Binary(),
                        File_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        ReceiveLetter_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReceivedLetters", t => t.ReceiveLetter_Id)
                .Index(t => t.ReceiveLetter_Id);
            
            CreateTable(
                "dbo.Cases",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        ParentCase_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cases", t => t.ParentCase_Id)
                .Index(t => t.ParentCase_Id);
            
            CreateTable(
                "dbo.SendLetters",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ParentType = c.Int(nullable: false),
                        RowNumber = c.String(),
                        Description = c.String(),
                        Body = c.String(),
                        LetterDateTime = c.DateTime(nullable: false),
                        ArchiveDateTime = c.DateTime(nullable: false),
                        LastModifyDateTime = c.DateTime(nullable: false),
                        Assortment_LetterType = c.Int(nullable: false),
                        Assortment_Security = c.Int(nullable: false),
                        Assortment_Nature = c.Int(nullable: false),
                        Assortment_Priority = c.Int(nullable: false),
                        Assortment_ContentType = c.Int(nullable: false),
                        Assortment_Situation = c.Int(nullable: false),
                        Assortment_Legalvalidity = c.Int(nullable: false),
                        AppendageContent = c.Binary(),
                        LetterFileContent = c.Binary(),
                        LetterFileAppendageContent = c.Binary(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        ParentReceivedLetterOut_Id = c.Guid(),
                        ParentSendLetter_Id = c.Guid(),
                        Department_Id = c.Guid(),
                        SenderEmployee_Id = c.Guid(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReceivedLetters", t => t.ParentReceivedLetterOut_Id)
                .ForeignKey("dbo.SendLetters", t => t.ParentSendLetter_Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .ForeignKey("dbo.SenderEmployees", t => t.SenderEmployee_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.ParentReceivedLetterOut_Id)
                .Index(t => t.ParentSendLetter_Id)
                .Index(t => t.Department_Id)
                .Index(t => t.SenderEmployee_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.SendAppendages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        File_Name = c.String(),
                        File_Content = c.Binary(),
                        File_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        SendLetter_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SendLetters", t => t.SendLetter_Id)
                .Index(t => t.SendLetter_Id);
            
            CreateTable(
                "dbo.TempReceivedLetters",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ParentType = c.Int(nullable: false),
                        RowNumber = c.String(),
                        Description = c.String(),
                        LetterDateTime = c.DateTime(nullable: false),
                        ArchiveDateTime = c.DateTime(nullable: false),
                        ModifyDateTime = c.DateTime(nullable: false),
                        Assortment_LetterType = c.Int(nullable: false),
                        Assortment_Security = c.Int(nullable: false),
                        Assortment_Nature = c.Int(nullable: false),
                        Assortment_Priority = c.Int(nullable: false),
                        Assortment_ContentType = c.Int(nullable: false),
                        Assortment_Situation = c.Int(nullable: false),
                        Assortment_Legalvalidity = c.Int(nullable: false),
                        AppendageContent = c.Binary(),
                        LetterFileContent = c.Binary(),
                        LetterFileAppendageContent = c.Binary(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        ParentReceiveLetter_Id = c.Guid(),
                        ParentSendLetterOutTemp_Id = c.Guid(),
                        ReceivedLetter_Id = c.Guid(nullable: false),
                        Sender_Id = c.Guid(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReceivedLetters", t => t.ParentReceiveLetter_Id)
                .ForeignKey("dbo.SendLetters", t => t.ParentSendLetterOutTemp_Id)
                .ForeignKey("dbo.ReceivedLetters", t => t.ReceivedLetter_Id)
                .ForeignKey("dbo.Senders", t => t.Sender_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.ParentReceiveLetter_Id)
                .Index(t => t.ParentSendLetterOutTemp_Id)
                .Index(t => t.ReceivedLetter_Id)
                .Index(t => t.Sender_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Senders",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Organization_Id = c.Guid(),
                        Person_Id = c.Guid(),
                        Post_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Organization_Id)
                .Index(t => t.Person_Id)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.TempReceiveAppendages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        File_Name = c.String(),
                        File_Content = c.Binary(),
                        File_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        TempReceiveLetter_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TempReceivedLetters", t => t.TempReceiveLetter_Id)
                .Index(t => t.TempReceiveLetter_Id);
            
            CreateTable(
                "dbo.TempReceiveLetterFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        File_Name = c.String(),
                        File_Content = c.Binary(),
                        File_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        TempReceiveLetter_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TempReceivedLetters", t => t.TempReceiveLetter_Id)
                .Index(t => t.TempReceiveLetter_Id);
            
            CreateTable(
                "dbo.Recievers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Organization_Id = c.Guid(),
                        Person_Id = c.Guid(),
                        Post_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Organization_Id)
                .Index(t => t.Person_Id)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.TempSendLetters",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ParentType = c.Int(nullable: false),
                        RowNumber = c.String(),
                        PastRowNumber = c.String(),
                        Description = c.String(),
                        Body = c.String(),
                        LetterDateTime = c.DateTime(nullable: false),
                        ArchiveDateTime = c.DateTime(nullable: false),
                        ModifyDateTime = c.DateTime(nullable: false),
                        Assortment_LetterType = c.Int(nullable: false),
                        Assortment_Security = c.Int(nullable: false),
                        Assortment_Nature = c.Int(nullable: false),
                        Assortment_Priority = c.Int(nullable: false),
                        Assortment_ContentType = c.Int(nullable: false),
                        Assortment_Situation = c.Int(nullable: false),
                        Assortment_Legalvalidity = c.Int(nullable: false),
                        AppendageContent = c.Binary(),
                        LetterFileContent = c.Binary(),
                        LetterFileAppendageContent = c.Binary(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        ParentReceivedLetterOutTemp_Id = c.Guid(),
                        ParentSendLetter_Id = c.Guid(),
                        Department_Id = c.Guid(),
                        SenderEmployee_Id = c.Guid(),
                        SendLetter_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReceivedLetters", t => t.ParentReceivedLetterOutTemp_Id)
                .ForeignKey("dbo.SendLetters", t => t.ParentSendLetter_Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .ForeignKey("dbo.SenderEmployees", t => t.SenderEmployee_Id)
                .ForeignKey("dbo.SendLetters", t => t.SendLetter_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.ParentReceivedLetterOutTemp_Id)
                .Index(t => t.ParentSendLetter_Id)
                .Index(t => t.Department_Id)
                .Index(t => t.SenderEmployee_Id)
                .Index(t => t.SendLetter_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.TempSendAppendages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        File_Name = c.String(),
                        File_Content = c.Binary(),
                        File_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        TempSendLetter_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TempSendLetters", t => t.TempSendLetter_Id)
                .Index(t => t.TempSendLetter_Id);
            
            CreateTable(
                "dbo.SenderEmployees",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Department_Id = c.Guid(),
                        Employee_Id = c.Guid(),
                        Post_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Department_Id)
                .Index(t => t.Employee_Id)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        ParentDepartement_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.ParentDepartement_Id)
                .Index(t => t.ParentDepartement_Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.Int(nullable: false),
                        BirthDay = c.DateTime(),
                        Summary = c.String(),
                        Address = c.String(),
                        MobileNumber = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        Avatar_Name = c.String(),
                        Avatar_Content = c.Binary(),
                        Avatar_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Department_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.Post_Department",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Department_Id = c.Guid(),
                        Post_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Department_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Department_Id)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.TempSendLetterFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        File_Name = c.String(),
                        File_Content = c.Binary(),
                        File_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        TempSendLetter_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TempSendLetters", t => t.TempSendLetter_Id)
                .Index(t => t.TempSendLetter_Id);
            
            CreateTable(
                "dbo.SendLetterFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        File_Name = c.String(),
                        File_Content = c.Binary(),
                        File_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        SendLetter_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SendLetters", t => t.SendLetter_Id)
                .Index(t => t.SendLetter_Id);
            
            CreateTable(
                "dbo.ReceiveLetterFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        File_Name = c.String(),
                        File_Content = c.Binary(),
                        File_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        ReceiveLetter_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReceivedLetters", t => t.ReceiveLetter_Id)
                .Index(t => t.ReceiveLetter_Id);
            
            CreateTable(
                "dbo.SystemLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Exception = c.String(),
                        ThrowExceptionDataTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        File_Name = c.String(),
                        File_Content = c.Binary(),
                        File_ContentType = c.String(),
                        Active = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrganizationUsers",
                c => new
                    {
                        Organization_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organization_Id, t.User_Id })
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Organization_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Post_OrganizationPerson",
                c => new
                    {
                        Post_Organization_Id = c.Guid(nullable: false),
                        Person_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Post_Organization_Id, t.Person_Id })
                .ForeignKey("dbo.Post_Organization", t => t.Post_Organization_Id, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.Person_Id, cascadeDelete: true)
                .Index(t => t.Post_Organization_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.PersonUsers",
                c => new
                    {
                        Person_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_Id, t.User_Id })
                .ForeignKey("dbo.People", t => t.Person_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Person_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.CaseReceivedLetters",
                c => new
                    {
                        Case_Id = c.Guid(nullable: false),
                        ReceivedLetter_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Case_Id, t.ReceivedLetter_Id })
                .ForeignKey("dbo.Cases", t => t.Case_Id, cascadeDelete: true)
                .ForeignKey("dbo.ReceivedLetters", t => t.ReceivedLetter_Id, cascadeDelete: true)
                .Index(t => t.Case_Id)
                .Index(t => t.ReceivedLetter_Id);
            
            CreateTable(
                "dbo.SendLetterCases",
                c => new
                    {
                        SendLetter_Id = c.Guid(nullable: false),
                        Case_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.SendLetter_Id, t.Case_Id })
                .ForeignKey("dbo.SendLetters", t => t.SendLetter_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cases", t => t.Case_Id, cascadeDelete: true)
                .Index(t => t.SendLetter_Id)
                .Index(t => t.Case_Id);
            
            CreateTable(
                "dbo.TempReceivedLetterCases",
                c => new
                    {
                        TempReceivedLetter_Id = c.Guid(nullable: false),
                        Case_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.TempReceivedLetter_Id, t.Case_Id })
                .ForeignKey("dbo.TempReceivedLetters", t => t.TempReceivedLetter_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cases", t => t.Case_Id, cascadeDelete: true)
                .Index(t => t.TempReceivedLetter_Id)
                .Index(t => t.Case_Id);
            
            CreateTable(
                "dbo.RecieverSendLetters",
                c => new
                    {
                        Reciever_Id = c.Guid(nullable: false),
                        SendLetter_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reciever_Id, t.SendLetter_Id })
                .ForeignKey("dbo.Recievers", t => t.Reciever_Id, cascadeDelete: true)
                .ForeignKey("dbo.SendLetters", t => t.SendLetter_Id, cascadeDelete: true)
                .Index(t => t.Reciever_Id)
                .Index(t => t.SendLetter_Id);
            
            CreateTable(
                "dbo.TempSendLetterCases",
                c => new
                    {
                        TempSendLetter_Id = c.Guid(nullable: false),
                        Case_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.TempSendLetter_Id, t.Case_Id })
                .ForeignKey("dbo.TempSendLetters", t => t.TempSendLetter_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cases", t => t.Case_Id, cascadeDelete: true)
                .Index(t => t.TempSendLetter_Id)
                .Index(t => t.Case_Id);
            
            CreateTable(
                "dbo.TempSendLetterRecievers",
                c => new
                    {
                        TempSendLetter_Id = c.Guid(nullable: false),
                        Reciever_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.TempSendLetter_Id, t.Reciever_Id })
                .ForeignKey("dbo.TempSendLetters", t => t.TempSendLetter_Id, cascadeDelete: true)
                .ForeignKey("dbo.Recievers", t => t.Reciever_Id, cascadeDelete: true)
                .Index(t => t.TempSendLetter_Id)
                .Index(t => t.Reciever_Id);
            
            CreateTable(
                "dbo.Post_DepartmentEmployee",
                c => new
                    {
                        Post_Department_Id = c.Guid(nullable: false),
                        Employee_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Post_Department_Id, t.Employee_Id })
                .ForeignKey("dbo.Post_Department", t => t.Post_Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Post_Department_Id)
                .Index(t => t.Employee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReceivedLetters", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ReceiveLetterFiles", "ReceiveLetter_Id", "dbo.ReceivedLetters");
            DropForeignKey("dbo.ReceivedLetters", "ParentSendLetterOut_Id", "dbo.SendLetters");
            DropForeignKey("dbo.ReceivedLetters", "ParentReceiveLetter_Id", "dbo.ReceivedLetters");
            DropForeignKey("dbo.SendLetters", "User_Id", "dbo.Users");
            DropForeignKey("dbo.SendLetterFiles", "SendLetter_Id", "dbo.SendLetters");
            DropForeignKey("dbo.TempSendLetters", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TempSendLetterFiles", "TempSendLetter_Id", "dbo.TempSendLetters");
            DropForeignKey("dbo.TempSendLetters", "SendLetter_Id", "dbo.SendLetters");
            DropForeignKey("dbo.TempSendLetters", "SenderEmployee_Id", "dbo.SenderEmployees");
            DropForeignKey("dbo.SendLetters", "SenderEmployee_Id", "dbo.SenderEmployees");
            DropForeignKey("dbo.SenderEmployees", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.SenderEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.SenderEmployees", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.TempSendLetters", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.SendLetters", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.Departments", "ParentDepartement_Id", "dbo.Departments");
            DropForeignKey("dbo.Employees", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.Post_Department", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Post_DepartmentEmployee", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.Post_DepartmentEmployee", "Post_Department_Id", "dbo.Post_Department");
            DropForeignKey("dbo.Post_Department", "Department_Id", "dbo.Departments");
            DropForeignKey("dbo.TempSendLetterRecievers", "Reciever_Id", "dbo.Recievers");
            DropForeignKey("dbo.TempSendLetterRecievers", "TempSendLetter_Id", "dbo.TempSendLetters");
            DropForeignKey("dbo.TempSendLetters", "ParentSendLetter_Id", "dbo.SendLetters");
            DropForeignKey("dbo.TempSendLetters", "ParentReceivedLetterOutTemp_Id", "dbo.ReceivedLetters");
            DropForeignKey("dbo.TempSendLetterCases", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.TempSendLetterCases", "TempSendLetter_Id", "dbo.TempSendLetters");
            DropForeignKey("dbo.TempSendAppendages", "TempSendLetter_Id", "dbo.TempSendLetters");
            DropForeignKey("dbo.RecieverSendLetters", "SendLetter_Id", "dbo.SendLetters");
            DropForeignKey("dbo.RecieverSendLetters", "Reciever_Id", "dbo.Recievers");
            DropForeignKey("dbo.Recievers", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Recievers", "Person_Id", "dbo.People");
            DropForeignKey("dbo.Recievers", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.TempReceivedLetters", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TempReceiveLetterFiles", "TempReceiveLetter_Id", "dbo.TempReceivedLetters");
            DropForeignKey("dbo.TempReceiveAppendages", "TempReceiveLetter_Id", "dbo.TempReceivedLetters");
            DropForeignKey("dbo.TempReceivedLetters", "Sender_Id", "dbo.Senders");
            DropForeignKey("dbo.ReceivedLetters", "Sender_Id", "dbo.Senders");
            DropForeignKey("dbo.Senders", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Senders", "Person_Id", "dbo.People");
            DropForeignKey("dbo.Senders", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.TempReceivedLetters", "ReceivedLetter_Id", "dbo.ReceivedLetters");
            DropForeignKey("dbo.TempReceivedLetters", "ParentSendLetterOutTemp_Id", "dbo.SendLetters");
            DropForeignKey("dbo.TempReceivedLetters", "ParentReceiveLetter_Id", "dbo.ReceivedLetters");
            DropForeignKey("dbo.TempReceivedLetterCases", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.TempReceivedLetterCases", "TempReceivedLetter_Id", "dbo.TempReceivedLetters");
            DropForeignKey("dbo.SendLetters", "ParentSendLetter_Id", "dbo.SendLetters");
            DropForeignKey("dbo.SendLetters", "ParentReceivedLetterOut_Id", "dbo.ReceivedLetters");
            DropForeignKey("dbo.SendLetterCases", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.SendLetterCases", "SendLetter_Id", "dbo.SendLetters");
            DropForeignKey("dbo.SendAppendages", "SendLetter_Id", "dbo.SendLetters");
            DropForeignKey("dbo.CaseReceivedLetters", "ReceivedLetter_Id", "dbo.ReceivedLetters");
            DropForeignKey("dbo.CaseReceivedLetters", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.Cases", "ParentCase_Id", "dbo.Cases");
            DropForeignKey("dbo.ReceiveAppendages", "ReceiveLetter_Id", "dbo.ReceivedLetters");
            DropForeignKey("dbo.PersonUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.PersonUsers", "Person_Id", "dbo.People");
            DropForeignKey("dbo.Post_Organization", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Post_OrganizationPerson", "Person_Id", "dbo.People");
            DropForeignKey("dbo.Post_OrganizationPerson", "Post_Organization_Id", "dbo.Post_Organization");
            DropForeignKey("dbo.Post_Organization", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.OrganizationUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.OrganizationUsers", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Organizations", "ParentOrganization_Id", "dbo.Organizations");
            DropForeignKey("dbo.ActivityLogs", "User_Id", "dbo.Users");
            DropIndex("dbo.Post_DepartmentEmployee", new[] { "Employee_Id" });
            DropIndex("dbo.Post_DepartmentEmployee", new[] { "Post_Department_Id" });
            DropIndex("dbo.TempSendLetterRecievers", new[] { "Reciever_Id" });
            DropIndex("dbo.TempSendLetterRecievers", new[] { "TempSendLetter_Id" });
            DropIndex("dbo.TempSendLetterCases", new[] { "Case_Id" });
            DropIndex("dbo.TempSendLetterCases", new[] { "TempSendLetter_Id" });
            DropIndex("dbo.RecieverSendLetters", new[] { "SendLetter_Id" });
            DropIndex("dbo.RecieverSendLetters", new[] { "Reciever_Id" });
            DropIndex("dbo.TempReceivedLetterCases", new[] { "Case_Id" });
            DropIndex("dbo.TempReceivedLetterCases", new[] { "TempReceivedLetter_Id" });
            DropIndex("dbo.SendLetterCases", new[] { "Case_Id" });
            DropIndex("dbo.SendLetterCases", new[] { "SendLetter_Id" });
            DropIndex("dbo.CaseReceivedLetters", new[] { "ReceivedLetter_Id" });
            DropIndex("dbo.CaseReceivedLetters", new[] { "Case_Id" });
            DropIndex("dbo.PersonUsers", new[] { "User_Id" });
            DropIndex("dbo.PersonUsers", new[] { "Person_Id" });
            DropIndex("dbo.Post_OrganizationPerson", new[] { "Person_Id" });
            DropIndex("dbo.Post_OrganizationPerson", new[] { "Post_Organization_Id" });
            DropIndex("dbo.OrganizationUsers", new[] { "User_Id" });
            DropIndex("dbo.OrganizationUsers", new[] { "Organization_Id" });
            DropIndex("dbo.ReceiveLetterFiles", new[] { "ReceiveLetter_Id" });
            DropIndex("dbo.SendLetterFiles", new[] { "SendLetter_Id" });
            DropIndex("dbo.TempSendLetterFiles", new[] { "TempSendLetter_Id" });
            DropIndex("dbo.Post_Department", new[] { "Post_Id" });
            DropIndex("dbo.Post_Department", new[] { "Department_Id" });
            DropIndex("dbo.Employees", new[] { "Department_Id" });
            DropIndex("dbo.Departments", new[] { "ParentDepartement_Id" });
            DropIndex("dbo.SenderEmployees", new[] { "Post_Id" });
            DropIndex("dbo.SenderEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.SenderEmployees", new[] { "Department_Id" });
            DropIndex("dbo.TempSendAppendages", new[] { "TempSendLetter_Id" });
            DropIndex("dbo.TempSendLetters", new[] { "User_Id" });
            DropIndex("dbo.TempSendLetters", new[] { "SendLetter_Id" });
            DropIndex("dbo.TempSendLetters", new[] { "SenderEmployee_Id" });
            DropIndex("dbo.TempSendLetters", new[] { "Department_Id" });
            DropIndex("dbo.TempSendLetters", new[] { "ParentSendLetter_Id" });
            DropIndex("dbo.TempSendLetters", new[] { "ParentReceivedLetterOutTemp_Id" });
            DropIndex("dbo.Recievers", new[] { "Post_Id" });
            DropIndex("dbo.Recievers", new[] { "Person_Id" });
            DropIndex("dbo.Recievers", new[] { "Organization_Id" });
            DropIndex("dbo.TempReceiveLetterFiles", new[] { "TempReceiveLetter_Id" });
            DropIndex("dbo.TempReceiveAppendages", new[] { "TempReceiveLetter_Id" });
            DropIndex("dbo.Senders", new[] { "Post_Id" });
            DropIndex("dbo.Senders", new[] { "Person_Id" });
            DropIndex("dbo.Senders", new[] { "Organization_Id" });
            DropIndex("dbo.TempReceivedLetters", new[] { "User_Id" });
            DropIndex("dbo.TempReceivedLetters", new[] { "Sender_Id" });
            DropIndex("dbo.TempReceivedLetters", new[] { "ReceivedLetter_Id" });
            DropIndex("dbo.TempReceivedLetters", new[] { "ParentSendLetterOutTemp_Id" });
            DropIndex("dbo.TempReceivedLetters", new[] { "ParentReceiveLetter_Id" });
            DropIndex("dbo.SendAppendages", new[] { "SendLetter_Id" });
            DropIndex("dbo.SendLetters", new[] { "User_Id" });
            DropIndex("dbo.SendLetters", new[] { "SenderEmployee_Id" });
            DropIndex("dbo.SendLetters", new[] { "Department_Id" });
            DropIndex("dbo.SendLetters", new[] { "ParentSendLetter_Id" });
            DropIndex("dbo.SendLetters", new[] { "ParentReceivedLetterOut_Id" });
            DropIndex("dbo.Cases", new[] { "ParentCase_Id" });
            DropIndex("dbo.ReceiveAppendages", new[] { "ReceiveLetter_Id" });
            DropIndex("dbo.ReceivedLetters", new[] { "User_Id" });
            DropIndex("dbo.ReceivedLetters", new[] { "ParentSendLetterOut_Id" });
            DropIndex("dbo.ReceivedLetters", new[] { "ParentReceiveLetter_Id" });
            DropIndex("dbo.ReceivedLetters", new[] { "Sender_Id" });
            DropIndex("dbo.Post_Organization", new[] { "Post_Id" });
            DropIndex("dbo.Post_Organization", new[] { "Organization_Id" });
            DropIndex("dbo.Organizations", new[] { "ParentOrganization_Id" });
            DropIndex("dbo.ActivityLogs", new[] { "User_Id" });
            DropTable("dbo.Post_DepartmentEmployee");
            DropTable("dbo.TempSendLetterRecievers");
            DropTable("dbo.TempSendLetterCases");
            DropTable("dbo.RecieverSendLetters");
            DropTable("dbo.TempReceivedLetterCases");
            DropTable("dbo.SendLetterCases");
            DropTable("dbo.CaseReceivedLetters");
            DropTable("dbo.PersonUsers");
            DropTable("dbo.Post_OrganizationPerson");
            DropTable("dbo.OrganizationUsers");
            DropTable("dbo.Templates");
            DropTable("dbo.SystemLogs");
            DropTable("dbo.ReceiveLetterFiles");
            DropTable("dbo.SendLetterFiles");
            DropTable("dbo.TempSendLetterFiles");
            DropTable("dbo.Post_Department");
            DropTable("dbo.Employees");
            DropTable("dbo.Departments");
            DropTable("dbo.SenderEmployees");
            DropTable("dbo.TempSendAppendages");
            DropTable("dbo.TempSendLetters");
            DropTable("dbo.Recievers");
            DropTable("dbo.TempReceiveLetterFiles");
            DropTable("dbo.TempReceiveAppendages");
            DropTable("dbo.Senders");
            DropTable("dbo.TempReceivedLetters");
            DropTable("dbo.SendAppendages");
            DropTable("dbo.SendLetters");
            DropTable("dbo.Cases");
            DropTable("dbo.ReceiveAppendages");
            DropTable("dbo.ReceivedLetters");
            DropTable("dbo.Posts");
            DropTable("dbo.Post_Organization");
            DropTable("dbo.People");
            DropTable("dbo.Organizations");
            DropTable("dbo.Users");
            DropTable("dbo.ActivityLogs");
        }
    }
}
