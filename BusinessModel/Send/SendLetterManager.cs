using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModel;
using DataAccessLayer;
using DBProvider;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using Aspose.Words;
using Aspose.Words.Reporting;
using Page = Aspose.Pdf.Page;

namespace BusinessModel
{
    public class SendLetterManager : DBProvider<SendLetter>
    {
        public SendLetterManager() : base(new DatabaseContext())
        {
        }
        public void RemoveAllRelations(SendLetter Old)
        {
            //Add to temp
            DBC.Set<SendLetterFile>().RemoveRange(Old.sendLetterFiles);
            DBC.Set<SendAppendage>().RemoveRange(Old.Appendages);
            SendAppendage df = new SendAppendage();
        }

        public void AddReciever(SendLetter sendLetter, Guid organizationGuid, Guid personGuid, Guid postGuid)
        {
            var orgDBset = DBC.Set<Organization>();
            var personDBset = DBC.Set<Person>();
            var postDBset = DBC.Set<Post>();
            var recieverDBset = DBC.Set<Reciever>();
            sendLetter.Recievers = new List<Reciever>();


            var existedReciever = recieverDBset.Where(x => (organizationGuid == Guid.Empty ? x.Organization == null : x.Organization.Id == organizationGuid) && (postGuid == Guid.Empty ? x.Post == null : x.Post.Id == postGuid) && (personGuid == Guid.Empty ? x.Person == null : x.Person.Id == personGuid));

            if (existedReciever.Any())
            {
                sendLetter.Recievers.Add(existedReciever.First());
                return;
            }

            Reciever reciever = new Reciever();

            if (personGuid != Guid.Empty)
            {
                reciever.Person = personDBset.Find(personGuid);
            }

            if (organizationGuid != Guid.Empty)
            {
                reciever.Organization = orgDBset.Find(organizationGuid);

                if (postGuid != Guid.Empty)
                    reciever.Post = postDBset.Find(postGuid);
            }
            sendLetter.Recievers.Add(reciever);
        }

        public void EditReciever(SendLetter sendLetter, Guid organizationGuid, Guid personGuid, Guid postGuid)
        {
            var orgDBset = DBC.Set<Organization>();
            var personDBset = DBC.Set<Person>();
            var postDBset = DBC.Set<Post>();
            var recieverDBset = DBC.Set<Reciever>();
            //clear it first
            List<Reciever> deletedList = new List<Reciever>();
            if (sendLetter.Recievers != null && sendLetter.Recievers.Count > 0)
            {
                foreach (var item in sendLetter.Recievers)
                {
                    deletedList.Add(item);
                }

                foreach (var item in deletedList)
                {
                    sendLetter.Recievers.Remove(item);
                }
            }
            else
            {
                sendLetter.Recievers = new List<Reciever>();
            }


            var existedReciever = recieverDBset.Where(x => (organizationGuid == Guid.Empty ? x.Organization == null : x.Organization.Id == organizationGuid) && (postGuid == Guid.Empty ? x.Post == null : x.Post.Id == postGuid) && (personGuid == Guid.Empty ? x.Person == null : x.Person.Id == personGuid));

            if (existedReciever.Any())
            {
                sendLetter.Recievers.Add(existedReciever.First());
                return;
            }

            Reciever reciever = new Reciever();

            if (personGuid != Guid.Empty)
            {
                reciever.Person = personDBset.Find(personGuid);
            }

            if (organizationGuid != Guid.Empty)
            {
                reciever.Organization = orgDBset.Find(organizationGuid);

                if (postGuid != Guid.Empty)
                    reciever.Post = postDBset.Find(postGuid);
            }
            sendLetter.Recievers.Add(reciever);
        }

        public void AddSenderEmployee(SendLetter sendLetter, Guid departmentGuid, Guid employeeGuid, Guid postGuid)
        {
            var departmentDBset = DBC.Set<Department>();
            var employeeDBset = DBC.Set<Employee>();
            var postDBset = DBC.Set<Post>();
            var senderEmployeeDBset = DBC.Set<SenderEmployee>();


            var existedSenderEmployee = senderEmployeeDBset.Where(x => (departmentGuid == Guid.Empty ? x.Department == null : x.Department.Id == departmentGuid) && (postGuid == Guid.Empty ? x.Post == null : x.Post.Id == postGuid) && (employeeGuid == Guid.Empty ? x.Employee == null : x.Employee.Id == employeeGuid));

            if (existedSenderEmployee.Any())
            {
                sendLetter.SenderEmployee = existedSenderEmployee.First();
                return;
            }

            sendLetter.SenderEmployee = new SenderEmployee();

            if (employeeGuid != Guid.Empty)
                sendLetter.SenderEmployee.Employee = employeeDBset.Find(employeeGuid);

            if (departmentGuid == Guid.Empty) return;
            sendLetter.SenderEmployee.Department = departmentDBset.Find(departmentGuid);

            if (postGuid != Guid.Empty)
                sendLetter.SenderEmployee.Post = postDBset.Find(postGuid);
        }

        public void EditSenderEmployee(SendLetter sendLetter, Guid departmentGuid, Guid employeeGuid, Guid postGuid)
        {
            var departmentDBset = DBC.Set<Department>();
            var employeeDBset = DBC.Set<Employee>();
            var postDBset = DBC.Set<Post>();
            var senderEmployeeDBset = DBC.Set<SenderEmployee>();


            var existedSenderEmployee = senderEmployeeDBset.Where(x => (departmentGuid == Guid.Empty ? x.Department == null : x.Department.Id == departmentGuid) && (postGuid == Guid.Empty ? x.Post == null : x.Post.Id == postGuid) && (employeeGuid == Guid.Empty ? x.Employee == null : x.Employee.Id == employeeGuid));

            if (existedSenderEmployee.Any())
            {
                sendLetter.SenderEmployee = existedSenderEmployee.First();
                return;
            }

            sendLetter.SenderEmployee = new SenderEmployee();

            if (employeeGuid != Guid.Empty)
                sendLetter.SenderEmployee.Employee = employeeDBset.Find(employeeGuid);

            if (departmentGuid == Guid.Empty) return;
            sendLetter.SenderEmployee.Department = departmentDBset.Find(departmentGuid);

            if (postGuid != Guid.Empty)
                sendLetter.SenderEmployee.Post = postDBset.Find(postGuid);
        }

        //public void AddEmployee(SendLetter sendLetter, Guid selectedEmployee)
        //{
        //    var empDBset = DBC.Set<Employee>();
        //    sendLetter.Employee = empDBset.FirstOrDefault(x => x.Id == selectedEmployee);
        //}
        public void AddUser(SendLetter sendLetter, Guid selectedUser)
        {
            var userDBset = DBC.Set<User>();
            sendLetter.User = userDBset.FirstOrDefault(x => x.Id == selectedUser);
        }

        //public void AddDepartment(SendLetter sendLetter, Guid selectedDepartment)
        //{
        //    var depDBset = DBC.Set<Department>();
        //    sendLetter.Deparment = depDBset.FirstOrDefault(x => x.Id == selectedDepartment);
        //    sendLetter.RowNumber = RowNumber(sendLetter.Deparment != null ? sendLetter.Deparment.Name : " ", dbSet.Count() + 1);
        //}
        public string AutoRowNumber(Department department)
        {
            var counter = dbSet.Count() + 1;
            string rowNumber = SetRowNumber(department != null ? department.Name : "ن", counter);
            while (dbSet.Any(x => x.RowNumber == rowNumber))
            {
                rowNumber = SetRowNumber(department != null ? department.Name : "ن", counter++);
            }
            return rowNumber;
        }

        private string SetRowNumber(string department, int id)
        {
            //return DateTime.Now.ToPersianTime().Split('/')[0] + department.Trim()[0] + id;
            return id + " / " + department.Trim()[0] + " / " + DateTime.Now.ToPersianTime().Split('/')[0];
        }
        public void AddCases(SendLetter sendLetter, List<Guid> selectedCasesGuid)
        {
            var caseDbset = DBC.Set<Case>();
            sendLetter.Cases = new List<Case>();
            foreach (var item in selectedCasesGuid)
            {
                sendLetter.Cases.Add(caseDbset.Find(item));
            }
        }
        public void EditCases(SendLetter sendLetter, List<Guid> selectedCasesGuidlist)
        {
            if (selectedCasesGuidlist != null && selectedCasesGuidlist.Count > 0)
            {
                if (sendLetter.Cases != null)
                {
                    List<Case> deletedCases = new List<Case>();
                    foreach (var item in sendLetter.Cases)
                    {
                        if (selectedCasesGuidlist.All(x => x != item.Id))
                        {
                            deletedCases.Add(item);
                        }
                        else
                        {
                            selectedCasesGuidlist.Remove(item.Id);
                        }
                    }


                    foreach (var item in deletedCases)
                    {
                        sendLetter.Cases.Remove(item);
                    }
                }
                else
                {
                    sendLetter.Cases = new List<Case>();
                }



                var caseDbset = DBC.Set<Case>();

                foreach (var item in selectedCasesGuidlist)
                {
                    sendLetter.Cases.Add(caseDbset.FirstOrDefault(x => x.Id == item));
                }
            }
            else
            {
                if (sendLetter.Cases != null)
                {
                    var deletedList = new List<Case>();

                    foreach (var item in sendLetter.Cases)
                    {
                        deletedList.Add(item);
                    }

                    foreach (var item in deletedList)
                    {
                        sendLetter.Cases.Remove(item);
                    }
                }
            }
        }

        public void GeneratePDF(SendLetter sendLetter, Guid LetterTemplate)
        {
            MemoryStream stream;
            if (sendLetter.sendLetterFiles == null ||
                sendLetter.sendLetterFiles.Count == 0)
            {
                //Create Template and Print letter to Database
                TemplateManager templateManager = new TemplateManager();
                Template template = templateManager.Read(LetterTemplate);
                stream = new MemoryStream(template.File.Content);
                if (template != null)
                {
                    Document doc = new Document(stream);
                    ReportingEngine engine = new ReportingEngine { Options = ReportBuildOptions.AllowMissingMembers };

                    PrintableSendLetter printableSendLetter = new PrintableSendLetter
                    {
                        DateTime =
                            sendLetter.LetterDateTime.ToPersianTime().toPersianNumber(),
                        Appendage =
                            (sendLetter.Appendages != null &&
                             sendLetter.Appendages.Count > 0)
                                ? "دارد"
                                : "ندارد",
                        Number = sendLetter.RowNumber.toPersianNumber(),
                        Body = HttpUtility.HtmlDecode(sendLetter.Body).toPersianNumber(),
                        SenderName = (sendLetter.SenderEmployee?.Employee != null) ? sendLetter.SenderEmployee?.Employee.FullName() : "",
                        //DepartMentName = (sendLetter.SenderEmployee?.Post != null ? sendLetter.SenderEmployee.Post.Name : "") + " " + (sendLetter.SenderEmployee?.Department != null ? sendLetter.SenderEmployee.Department.Name : "")
                        DepartMentName = sendLetter.SenderEmployee?.Post!=null?sendLetter.SenderEmployee.Post.Name:""
                    };



                    if (sendLetter.Recievers != null && sendLetter.Recievers.Count > 0 && sendLetter.Recievers.First()?.Person != null)
                    {
                        StringBuilder subtitleStbBuilder = new StringBuilder();

                        subtitleStbBuilder.Append(sendLetter.Recievers.First().Person.Gender == 0
                            ? "سرکار خانم "
                            : "جناب آقاي ");

                        subtitleStbBuilder.Append(sendLetter.Recievers.First().Person.FullName());
                        printableSendLetter.SubTitle = subtitleStbBuilder.ToString();

                    }


                    if (sendLetter.Recievers != null && sendLetter.Recievers.Count > 0 && sendLetter.Recievers.First().Organization != null)
                    {
                        StringBuilder titleStbBuilder = new StringBuilder();
                        titleStbBuilder.Append("");

                        if (sendLetter.Recievers.First().Organization != null)
                        {
                            if (sendLetter.Recievers.First().Post != null)
                            {


                                titleStbBuilder.Append(sendLetter.Recievers.First().Post.Name + " محترم ");

                                titleStbBuilder.Append(sendLetter.Recievers.First().Organization.GetFullOrganizationPath());
                            }


                            printableSendLetter.Title = titleStbBuilder.ToString();

                        }


                    }
                    // Execute the build report.
                    engine.BuildReport(doc, printableSendLetter, "s");

                    //Save letter without Appendage as pdf
                    stream = new MemoryStream();
                    doc.Save(stream, SaveFormat.Pdf);
                    sendLetter.LetterFileContent = stream.ToArray();
                }
                else
                {
                    //Should send error to User
                    throw new Exception("قالب انتخاب شده یافت نگردید");
                }
            }
            else
            {
                //Render Letter Content With Files
                Aspose.Pdf.Document contentPdf = new Aspose.Pdf.Document();
                foreach (var file in sendLetter.sendLetterFiles)
                {
                    if (file.File.ContentType.Contains("image"))
                    {
                        Aspose.Pdf.Page page = contentPdf.Pages.Add();
                        Bitmap b = new Bitmap(new MemoryStream(file.File.Content));
                        page.PageInfo.Width = b.Width;
                        page.PageInfo.Height = b.Height;
                        // Set margins so image will fit, etc.
                        page.PageInfo.Margin.Bottom = 0;
                        page.PageInfo.Margin.Top = 0;
                        page.PageInfo.Margin.Left = 0;
                        page.PageInfo.Margin.Right = 0;

                        page.CropBox = new Aspose.Pdf.Rectangle(0, 0, b.Width, b.Height);

                        Aspose.Pdf.Image image1 = new Aspose.Pdf.Image();
                        // Add the image into paragraphs collection of the section
                        page.Paragraphs.Add(image1);
                        // Set the image file stream
                        image1.ImageStream = new MemoryStream(file.File.Content);
                    }
                    else if (file.File.ContentType.Contains("pdf"))
                    {
                        //Append pdf to appendages
                        Aspose.Pdf.Document thisPDF = new Aspose.Pdf.Document(new MemoryStream(file.File.Content));
                        contentPdf.Pages.Add(thisPDF.Pages);
                    }
                    else if (file.File.ContentType.Contains("word"))
                    {
                        //Convert to pdf first
                        Document thisDoc = new Document(new MemoryStream(file.File.Content));
                        MemoryStream temp = new MemoryStream();
                        thisDoc.Save(temp, SaveFormat.Pdf);
                        //Second append it to appendages
                        Aspose.Pdf.Document thisPDF = new Aspose.Pdf.Document(temp);
                        contentPdf.Pages.Add(thisPDF.Pages);
                    }
                }
                stream = new MemoryStream();
                contentPdf.Save(stream);
                sendLetter.LetterFileContent = stream.ToArray();
            }

            //Debug
            //Aspose.Pdf.Document letterFile = new Aspose.Pdf.Document(stream);
            //letterFile.Save(Server.MapPath("~/LetterOnly.pdf"));


            //Save letter with Appendage
            if (sendLetter.Appendages != null &&
                sendLetter.Appendages.Count > 0)
            {
                Aspose.Pdf.Document appendagePdf = new Aspose.Pdf.Document();
                foreach (var file in sendLetter.Appendages)
                {
                    if (file.File.ContentType.Contains("image"))
                    {
                        Page page = appendagePdf.Pages.Add();
                        Bitmap b = new Bitmap(new MemoryStream(file.File.Content));
                        page.PageInfo.Width = b.Width;
                        page.PageInfo.Height = b.Height;
                        // Set margins so image will fit, etc.
                        page.PageInfo.Margin.Bottom = 0;
                        page.PageInfo.Margin.Top = 0;
                        page.PageInfo.Margin.Left = 0;
                        page.PageInfo.Margin.Right = 0;

                        page.CropBox = new Aspose.Pdf.Rectangle(0, 0, b.Width, b.Height);

                        Aspose.Pdf.Image image1 = new Aspose.Pdf.Image();
                        // Add the image into paragraphs collection of the section
                        page.Paragraphs.Add(image1);
                        // Set the image file stream
                        image1.ImageStream = new MemoryStream(file.File.Content);
                    }
                    else if (file.File.ContentType.Contains("pdf"))
                    {
                        //Append pdf to appendages
                        Aspose.Pdf.Document thisPDF = new Aspose.Pdf.Document(new MemoryStream(file.File.Content));
                        appendagePdf.Pages.Add(thisPDF.Pages);
                    }
                    else if (file.File.ContentType.Contains("word"))
                    {
                        //Convert to pdf first
                        Document thisDoc = new Document(new MemoryStream(file.File.Content));
                        MemoryStream temp = new MemoryStream();
                        thisDoc.Save(temp, SaveFormat.Pdf);
                        //Second append it to appendages
                        Aspose.Pdf.Document thisPDF = new Aspose.Pdf.Document(temp);
                        appendagePdf.Pages.Add(thisPDF.Pages);
                    }
                }
                //Return PDF from letter
                Aspose.Pdf.Document letterPdf = new Aspose.Pdf.Document(stream);

                stream = new MemoryStream();
                appendagePdf.Save(stream);
                sendLetter.AppendageContent = stream.ToArray();
                //Debug
                //appendagePdf.Save(Server.MapPath("~/AppendagesOnly.pdf"));

                letterPdf.Pages.Add(appendagePdf.Pages);
                stream = new MemoryStream();
                letterPdf.Save(stream);
                //Debug
                //letterPdf.Save(Server.MapPath("~/letterWithAppendages.pdf"));

                sendLetter.LetterFileAppendageContent = stream.ToArray();
            }
        }

        public SendLetter GetParent(Guid child)
        {
            var parent = DBC.Database.SqlQuery<Guid>("SELECT ParentSendletter_Id FROM SendLetters WHERE Id={0}", child).FirstOrDefault<Guid>();
            return Read(parent);
        }

        public ReceivedLetter GetParentOut(Guid child)
        {
            var parent =
                DBC.Database.SqlQuery<Guid>("SELECT ParentReceivedLetterOut_Id from SendLetters where Id={0}", child)
                    .FirstOrDefault();
            ReceiveLetterManager receivedLetterManager = new ReceiveLetterManager();
            return receivedLetterManager.Read(parent);
        }

        public void UpdateSendLetterFiles(SendLetter sendLetter,
            List<HttpPostedFileBase> fileUploadLetters, List<Guid> FilesID)
        {
            if (sendLetter.sendLetterFiles != null)
            {
                List<SendLetterFile> deletedList = new List<SendLetterFile>();
                foreach (var item in sendLetter.sendLetterFiles.ToList())
                {
                    if (FilesID.All(x => x != item.Id))
                    {
                        deletedList.Add(item);
                    }
                }

                //Delete Items
                var dbSetremovefiles = DBC.Set<SendLetterFile>();
                foreach (var item in deletedList)
                {
                    dbSetremovefiles.Remove(item);
                }


            }

            int i = 0;
            foreach (var item in fileUploadLetters)
            {
                if (item != null && item.ContentLength > 0)
                {
                    var target = new MemoryStream();
                    SendLetterFile sendLetterFile = new SendLetterFile();
                    item.InputStream.CopyTo(target);
                    sendLetterFile.File = new ArchiveFile
                    {
                        Content = target.ToArray(),
                        Name = item.FileName,
                        ContentType = item.ContentType
                    };
                    try
                    {
                        sendLetter.sendLetterFiles[i] = sendLetterFile;
                    }
                    catch (Exception)
                    {
                        sendLetter.sendLetterFiles.Add(sendLetterFile);


                    }

                }
                i++;
            }
        }

        public void UpdateSendLetterAppendages(SendLetter sendLetter,
        List<HttpPostedFileBase> fileUploadAppendages, List<Guid> AppendagesID)
        {
            if (sendLetter.Appendages != null)
            {
                List<SendAppendage> deletedList = new List<SendAppendage>();
                foreach (var item in sendLetter.Appendages.ToList())
                {
                    if (AppendagesID.All(x => x != item.Id))
                    {
                        deletedList.Add(item);
                    }
                }

                //Delete Items
                var dbSetremovefiles = DBC.Set<SendAppendage>();
                foreach (var item in deletedList)
                {
                    dbSetremovefiles.Remove(item);
                }


            }

            int i = 0;
            foreach (var item in fileUploadAppendages)
            {
                if (item != null && item.ContentLength > 0)
                {
                    var target = new MemoryStream();
                    SendAppendage sendAppendage = new SendAppendage();
                    item.InputStream.CopyTo(target);
                    sendAppendage.File = new ArchiveFile
                    {
                        Content = target.ToArray(),
                        Name = item.FileName,
                        ContentType = item.ContentType
                    };
                    try
                    {
                        sendLetter.Appendages[i] = sendAppendage;
                    }
                    catch (Exception)
                    {
                        sendLetter.Appendages.Add(sendAppendage);


                    }

                }
                i++;
            }
        }

        public void GeneratePDF(SendLetter sendLetter)
        {
            MemoryStream stream = null;

            if (sendLetter.sendLetterFiles != null &&
                sendLetter.sendLetterFiles.Count > 0)
            {
                //Render Letter Content With Files
                Aspose.Pdf.Document contentPdf = new Aspose.Pdf.Document();
                foreach (var file in sendLetter.sendLetterFiles)
                {
                    if (file.File.ContentType.Contains("image"))
                    {
                        Aspose.Pdf.Page page = contentPdf.Pages.Add();
                        Bitmap b = new Bitmap(new MemoryStream(file.File.Content));
                        page.PageInfo.Width = b.Width;
                        page.PageInfo.Height = b.Height;
                        // Set margins so image will fit, etc.
                        page.PageInfo.Margin.Bottom = 0;
                        page.PageInfo.Margin.Top = 0;
                        page.PageInfo.Margin.Left = 0;
                        page.PageInfo.Margin.Right = 0;

                        page.CropBox = new Aspose.Pdf.Rectangle(0, 0, b.Width, b.Height);

                        Aspose.Pdf.Image image1 = new Aspose.Pdf.Image();
                        // Add the image into paragraphs collection of the section
                        page.Paragraphs.Add(image1);
                        // Set the image file stream
                        image1.ImageStream = new MemoryStream(file.File.Content);
                    }
                    else if (file.File.ContentType.Contains("pdf"))
                    {
                        //Append pdf to appendages
                        Aspose.Pdf.Document thisPDF = new Aspose.Pdf.Document(new MemoryStream(file.File.Content));
                        contentPdf.Pages.Add(thisPDF.Pages);
                    }
                    else if (file.File.ContentType.Contains("word"))
                    {
                        //Convert to pdf first
                        Document thisDoc = new Document(new MemoryStream(file.File.Content));
                        MemoryStream temp = new MemoryStream();
                        thisDoc.Save(temp, SaveFormat.Pdf);
                        //Second append it to appendages
                        Aspose.Pdf.Document thisPDF = new Aspose.Pdf.Document(temp);
                        contentPdf.Pages.Add(thisPDF.Pages);
                    }
                }
                stream = new MemoryStream();
                contentPdf.Save(stream);
                sendLetter.LetterFileContent = stream.ToArray();
            }

            //Debug
            //Aspose.Pdf.Document letterFile = new Aspose.Pdf.Document(stream);
            //letterFile.Save(Server.MapPath("~/LetterOnly.pdf"));


            //Save letter with Appendage
            if (sendLetter.Appendages != null &&
                sendLetter.Appendages.Count > 0)
            {
                Aspose.Pdf.Document appendagePdf = new Aspose.Pdf.Document();
                foreach (var file in sendLetter.Appendages)
                {
                    if (file.File.ContentType.Contains("image"))
                    {
                        Page page = appendagePdf.Pages.Add();
                        Bitmap b = new Bitmap(new MemoryStream(file.File.Content));
                        page.PageInfo.Width = b.Width;
                        page.PageInfo.Height = b.Height;
                        // Set margins so image will fit, etc.
                        page.PageInfo.Margin.Bottom = 0;
                        page.PageInfo.Margin.Top = 0;
                        page.PageInfo.Margin.Left = 0;
                        page.PageInfo.Margin.Right = 0;

                        page.CropBox = new Aspose.Pdf.Rectangle(0, 0, b.Width, b.Height);

                        Aspose.Pdf.Image image1 = new Aspose.Pdf.Image();
                        // Add the image into paragraphs collection of the section
                        page.Paragraphs.Add(image1);
                        // Set the image file stream
                        image1.ImageStream = new MemoryStream(file.File.Content);
                    }
                    else if (file.File.ContentType.Contains("pdf"))
                    {
                        //Append pdf to appendages
                        Aspose.Pdf.Document thisPDF = new Aspose.Pdf.Document(new MemoryStream(file.File.Content));
                        appendagePdf.Pages.Add(thisPDF.Pages);
                    }
                    else if (file.File.ContentType.Contains("word"))
                    {
                        //Convert to pdf first
                        Document thisDoc = new Document(new MemoryStream(file.File.Content));
                        MemoryStream temp = new MemoryStream();
                        thisDoc.Save(temp, SaveFormat.Pdf);
                        //Second append it to appendages
                        Aspose.Pdf.Document thisPDF = new Aspose.Pdf.Document(temp);
                        appendagePdf.Pages.Add(thisPDF.Pages);
                    }
                }

                //Return PDF from letter
                if (stream != null)
                {
                    Aspose.Pdf.Document letterPdf = new Aspose.Pdf.Document(stream);

                    stream = new MemoryStream();
                    appendagePdf.Save(stream);
                    sendLetter.AppendageContent = stream.ToArray();
                    //Debug
                    //appendagePdf.Save(Server.MapPath("~/AppendagesOnly.pdf"));

                    letterPdf.Pages.Add(appendagePdf.Pages);
                    stream = new MemoryStream();
                    letterPdf.Save(stream);
                    //Debug
                    //letterPdf.Save(Server.MapPath("~/letterWithAppendages.pdf"));

                    sendLetter.LetterFileAppendageContent = stream.ToArray();
                }
                else
                {

                    stream = new MemoryStream();
                    appendagePdf.Save(stream);
                    sendLetter.AppendageContent = stream.ToArray();
                    sendLetter.LetterFileAppendageContent = stream.ToArray();
                }
            }
        }

        public void AddParent(SendLetter sendLetter, Guid? parentletterId)
        {
            switch (sendLetter.ParentType)
            {
                case ParentType.Receive:
                    if (parentletterId != null &&
                        parentletterId != Guid.Empty)
                    {
                        var receiveLetterDbset = DBC.Set<ReceivedLetter>();
                        sendLetter.ParentReceivedLetterOut =
                            receiveLetterDbset.Find((Guid)parentletterId);
                        if (sendLetter.ParentSendLetter != null)
                        {
                            sendLetter.ParentSendLetter = null;
                        }
                    }
                    else
                    {
                        if (sendLetter.ParentSendLetter != null)
                        {
                            sendLetter.ParentSendLetter = null;
                        }
                        if (sendLetter.ParentReceivedLetterOut != null)
                        {
                            sendLetter.ParentReceivedLetterOut = null;
                        }
                    }
                    break;
                case ParentType.Send:
                    if (parentletterId != null &&
                        parentletterId != Guid.Empty)
                    {
                        sendLetter.ParentSendLetter =
                        Read((Guid)parentletterId);
                        if (sendLetter.ParentReceivedLetterOut != null)
                        {
                            sendLetter.ParentReceivedLetterOut = null;
                        }
                    }
                    else
                    {
                        if (sendLetter.ParentSendLetter != null)
                        {
                            sendLetter.ParentSendLetter = null;
                        }
                        if (sendLetter.ParentReceivedLetterOut != null)
                        {
                            sendLetter.ParentReceivedLetterOut = null;
                        }
                    }
                    break;
                default:
                    break;
            }
        }



        //Logic to find Childrens of a parent

        public Tuple<List<ReceivedLetter>, List<SendLetter>> GetChildItems(Guid parentId, Tuple<List<ReceivedLetter>, List<SendLetter>> emptylists)
        {
            SendLetter sendLetter = Read(x => x.Id == parentId, null,
                 "ParentSendLetterOut,ParentSendLetter").First();
            if (sendLetter != null)
            {
                emptylists.Item2.Add(sendLetter);
                if (sendLetter.ParentSendLetterOut != null && sendLetter.ParentSendLetterOut.Count > 0)
                {
                    foreach (var row2 in sendLetter.ParentSendLetterOut)
                    {
                        emptylists.Item1.Add(row2);
                        ReceiveLetterManager myReceiveLetterManager = new ReceiveLetterManager();
                        myReceiveLetterManager.GetChildItems(row2.Id, emptylists);
                    }
                }
                GetChildItem(parentId, emptylists);
                //emptylists.Item1.AddRange(sendLetter.ParentSendLetterOut);
            }
            return emptylists;

        }
        private Tuple<List<ReceivedLetter>, List<SendLetter>> GetChildItem(Guid parentId, Tuple<List<ReceivedLetter>, List<SendLetter>> emptylists)
        {
            var gettedRow = GetRowsByParent(parentId);


            if (gettedRow.Count > 0) // If this item have childs then i print them
            {
                //Label1.Text += "<ul>";
                foreach (var row in gettedRow)
                {
                    if (row.ParentSendLetterOut != null && row.ParentSendLetterOut.Count > 0)
                    {
                        foreach (var row2 in row.ParentSendLetterOut)
                        {
                            emptylists.Item1.Add(row2);
                            ReceiveLetterManager myReceiveLetterManager = new ReceiveLetterManager();
                            myReceiveLetterManager.GetChildItems(row2.Id, emptylists);
                        }
                        emptylists.Item2.Add(row);
                    }
                    GetChildItems(row.Id, emptylists); // Search for childs that belong to this item
                }
            }
            return emptylists;

        }

        private List<SendLetter> GetRowsByParent(Guid parentId)
        {
            //List<SendLetter> DbMenuTable = Read(x => x.ParentSendLetter != null && x.ParentSendLetter.Id == parentId, null,
            //    "ParentSendLetterOut,ParentSendLetter").ToList();
            //return DbMenuTable;

            List<SendLetter> DbMenuTable = Read("1==1",
              "ParentSendLetterOut,ParentSendLetter").ToList();
            return DbMenuTable.Where(item => item.ParentSendLetter?.Id == parentId).ToList();
        }


    }
}
