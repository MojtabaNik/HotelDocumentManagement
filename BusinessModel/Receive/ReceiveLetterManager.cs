using DataAccessLayer;
using DBProvider;
using EntityModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Aspose.Pdf;
using Document = Aspose.Words.Document;
using SaveFormat = Aspose.Words.SaveFormat;

namespace BusinessModel
{
    public class ReceiveLetterManager : DBProvider<ReceivedLetter>
    {
        public ReceiveLetterManager() : base(new DatabaseContext())
        {
        }
        public void AddCases(ReceivedLetter receivedLetter, List<Guid> selectedCasesGuidlist)
        {
            receivedLetter.Cases = new List<Case>();
            var caseDbset = DBC.Set<Case>();
            foreach (var item in selectedCasesGuidlist)
            {
                receivedLetter.Cases.Add(caseDbset.FirstOrDefault(x => x.Id == item));
            }
        }

        public void EditCases(ReceivedLetter receivedLetter, List<Guid> selectedCasesGuidlist)
        {
            if (selectedCasesGuidlist != null && selectedCasesGuidlist.Count > 0)
            {
                if (receivedLetter.Cases != null)
                {
                    List<Case> deletedCases = new List<Case>();
                    foreach (var item in receivedLetter.Cases)
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
                        receivedLetter.Cases.Remove(item);
                    }
                }
                else
                {
                    receivedLetter.Cases = new List<Case>();
                }



                var caseDbset = DBC.Set<Case>();

                foreach (var item in selectedCasesGuidlist)
                {
                    receivedLetter.Cases.Add(caseDbset.FirstOrDefault(x => x.Id == item));
                }
            }
            else
            {
                if (receivedLetter.Cases != null)
                {
                    var deletedList = new List<Case>();

                    foreach (var item in receivedLetter.Cases)
                    {
                        deletedList.Add(item);
                    }

                    foreach (var item in deletedList)
                    {
                        receivedLetter.Cases.Remove(item);
                    }
                }
            }
        }

        public void AddSender(ReceivedLetter receivedLetter, Guid organizationGuid, Guid personGuid, Guid postGuid)
        {
            var orgDBset = DBC.Set<Organization>();
            var personDBset = DBC.Set<Person>();
            var postDBset = DBC.Set<Post>();
            var senderDBset = DBC.Set<Sender>();


            var existedSender = senderDBset.Where(x => (organizationGuid == Guid.Empty ? x.Organization == null : x.Organization.Id == organizationGuid) && (postGuid == Guid.Empty ? x.Post == null : x.Post.Id == postGuid) && (personGuid == Guid.Empty ? x.Person == null : x.Person.Id == personGuid));

            if (existedSender.Any())
            {
                receivedLetter.Sender = existedSender.First();
                return;
            }

            receivedLetter.Sender = new Sender();

            if (personGuid != Guid.Empty)
                receivedLetter.Sender.Person = personDBset.Find(personGuid);

            if (organizationGuid == Guid.Empty) return;
            receivedLetter.Sender.Organization = orgDBset.Find(organizationGuid);

            if (postGuid != Guid.Empty)
                receivedLetter.Sender.Post = postDBset.Find(postGuid);
        }

        public void EditSender(ReceivedLetter receivedLetter, Guid organizationGuid, Guid personGuid, Guid postGuid)
        {
            var orgDBset = DBC.Set<Organization>();
            var personDBset = DBC.Set<Person>();
            var postDBset = DBC.Set<Post>();
            var senderDBset = DBC.Set<Sender>();


            var existedSender = senderDBset.Where(x => (organizationGuid == Guid.Empty ? x.Organization == null : x.Organization.Id == organizationGuid) && (postGuid == Guid.Empty ? x.Post == null : x.Post.Id == postGuid) && (personGuid == Guid.Empty ? x.Person == null : x.Person.Id == personGuid));

            if (existedSender.Any())
            {
                receivedLetter.Sender = existedSender.First();
                return;
            }

            receivedLetter.Sender = new Sender();

            if (personGuid != Guid.Empty)
                receivedLetter.Sender.Person = personDBset.Find(personGuid);

            if (organizationGuid == Guid.Empty) return;
            receivedLetter.Sender.Organization = orgDBset.Find(organizationGuid);

            if (postGuid != Guid.Empty)
                receivedLetter.Sender.Post = postDBset.Find(postGuid);
        }

        public void AddUser(ReceivedLetter receivedLetter, Guid selectedUser)
        {
            var userDBset = DBC.Set<User>();
            receivedLetter.User = userDBset.FirstOrDefault(x => x.Id == selectedUser);
        }
        public ReceivedLetter GetParent(Guid child)
        {
            var parent =
                DBC.Database.SqlQuery<Guid>("SELECT ParentReceiveLetter_Id from ReceivedLetters where Id={0}", child)
                    .FirstOrDefault<Guid>();
            return Read(parent);
        }
        public SendLetter GetParentOut(Guid child)
        {
            var parent =
                DBC.Database.SqlQuery<Guid>("SELECT ParentSendLetterOut_Id from ReceivedLetters where Id={0}", child)
                    .FirstOrDefault();
            SendLetterManager sendLetterManager = new SendLetterManager();
            return sendLetterManager.Read(parent);
        }

        public void UpdateReceiveLetterFiles(ReceivedLetter receivedLetter,
            List<HttpPostedFileBase> fileUploadLetters, List<Guid> FilesID)
        {
            if (receivedLetter.ReceiveLetterFiles != null)
            {
                List<ReceiveLetterFile> deletedList = new List<ReceiveLetterFile>();
                foreach (var item in receivedLetter.ReceiveLetterFiles.ToList())
                {
                    if (FilesID.All(x => x != item.Id))
                    {
                        deletedList.Add(item);
                    }
                }

                //Delete Items
                var dbSetremovefiles = DBC.Set<ReceiveLetterFile>();
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
                    ReceiveLetterFile receiveLetterFile = new ReceiveLetterFile();
                    item.InputStream.CopyTo(target);
                    receiveLetterFile.File = new ArchiveFile
                    {
                        Content = target.ToArray(),
                        Name = item.FileName,
                        ContentType = item.ContentType
                    };
                    try
                    {
                        receivedLetter.ReceiveLetterFiles[i] = receiveLetterFile;
                    }
                    catch (Exception)
                    {
                        receivedLetter.ReceiveLetterFiles.Add(receiveLetterFile);


                    }

                }
                i++;
            }
        }

        public void UpdateReceiveLetterAppendages(ReceivedLetter receivedLetter,
        List<HttpPostedFileBase> fileUploadAppendages, List<Guid> AppendagesID)
        {
            if (receivedLetter.Appendages != null)
            {
                List<ReceiveAppendage> deletedList = new List<ReceiveAppendage>();
                foreach (var item in receivedLetter.Appendages.ToList())
                {
                    if (AppendagesID.All(x => x != item.Id))
                    {
                        deletedList.Add(item);
                    }
                }

                //Delete Items
                var dbSetremovefiles = DBC.Set<ReceiveAppendage>();
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
                    ReceiveAppendage receiveAppendage = new ReceiveAppendage();
                    item.InputStream.CopyTo(target);
                    receiveAppendage.File = new ArchiveFile
                    {
                        Content = target.ToArray(),
                        Name = item.FileName,
                        ContentType = item.ContentType
                    };
                    try
                    {
                        receivedLetter.Appendages[i] = receiveAppendage;
                    }
                    catch (Exception)
                    {
                        receivedLetter.Appendages.Add(receiveAppendage);


                    }

                }
                i++;
            }
        }

        public void GeneratePDF(ReceivedLetter receivedLetter)
        {
            MemoryStream stream = null;

            if (receivedLetter.ReceiveLetterFiles != null &&
                receivedLetter.ReceiveLetterFiles.Count > 0)
            {
                //Render Letter Content With Files
                Aspose.Pdf.Document contentPdf = new Aspose.Pdf.Document();
                foreach (var file in receivedLetter.ReceiveLetterFiles)
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
                receivedLetter.LetterFileContent = stream.ToArray();
            }
            else
            {
                receivedLetter.LetterFileContent = new byte[0];
            }

            //Debug
            //Aspose.Pdf.Document letterFile = new Aspose.Pdf.Document(stream);
            //letterFile.Save(Server.MapPath("~/LetterOnly.pdf"));


            //Save letter with Appendage
            if (receivedLetter.Appendages != null &&
                receivedLetter.Appendages.Count > 0)
            {
                Aspose.Pdf.Document appendagePdf = new Aspose.Pdf.Document();
                foreach (var file in receivedLetter.Appendages)
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
                if (stream != null && receivedLetter.LetterFileContent.Length > 0)
                {
                    Aspose.Pdf.Document letterPdf = new Aspose.Pdf.Document(stream);

                    stream = new MemoryStream();
                    appendagePdf.Save(stream);
                    receivedLetter.AppendageContent = stream.ToArray();
                    //Debug
                    //appendagePdf.Save(Server.MapPath("~/AppendagesOnly.pdf"));

                    letterPdf.Pages.Add(appendagePdf.Pages);
                    stream = new MemoryStream();
                    letterPdf.Save(stream);
                    //Debug
                    //letterPdf.Save(Server.MapPath("~/letterWithAppendages.pdf"));

                    receivedLetter.LetterFileAppendageContent = stream.ToArray();
                }
                else
                {

                    stream = new MemoryStream();
                    appendagePdf.Save(stream);
                    receivedLetter.AppendageContent = stream.ToArray();
                    receivedLetter.LetterFileAppendageContent = stream.ToArray();
                }
            }
            else
            {
                receivedLetter.LetterFileAppendageContent = new byte[0];
                receivedLetter.AppendageContent = new byte[0];
            }
        }

        public void SetParent(ReceivedLetter receivedLetter, Guid? parentletterId)
        {
        
            switch (receivedLetter.ParentType)
            {
                case ParentType.Receive:
                    if (parentletterId != null &&
                        parentletterId != Guid.Empty)
                    {
                        receivedLetter.ParentReceiveLetter =
                            Read((Guid) parentletterId);
                        if (receivedLetter.ParentSendLetterOut != null)
                        {
                            receivedLetter.ParentSendLetterOut = null;
                        }
                    }
                    else
                    {
                        if (receivedLetter.ParentReceiveLetter != null)
                        {
                            receivedLetter.ParentReceiveLetter = null;
                        }
                        if (receivedLetter.ParentSendLetterOut != null)
                        {
                            receivedLetter.ParentSendLetterOut = null;
                        }
                    }
                    break;
                case ParentType.Send:
                    if (parentletterId != null &&
                        parentletterId != Guid.Empty)
                    {
                        var sendLetterDbset = DBC.Set<SendLetter>();
                        receivedLetter.ParentSendLetterOut =
                            sendLetterDbset.Find((Guid) parentletterId);
                        if (receivedLetter.ParentReceiveLetter != null)
                        {
                            receivedLetter.ParentReceiveLetter = null;
                        }
                    }
                    else
                    {
                        if (receivedLetter.ParentReceiveLetter != null)
                        {
                            receivedLetter.ParentReceiveLetter = null;
                        }
                        if (receivedLetter.ParentSendLetterOut != null)
                        {
                            receivedLetter.ParentSendLetterOut = null;
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

            ReceivedLetter receivedLetter = Read(x => x.Id == parentId, null,
                 "ParentReceivedLetterOut,ParentReceiveLetter").First();
            if (receivedLetter != null)
            {
                emptylists.Item1.Add(receivedLetter);
                if (receivedLetter.ParentReceivedLetterOut != null &&
                    receivedLetter.ParentReceivedLetterOut.Count > 0)
                {
                    foreach (var row2 in receivedLetter.ParentReceivedLetterOut)
                    {
                        emptylists.Item2.Add(row2);
                        SendLetterManager mySendLetterManager = new SendLetterManager();
                        mySendLetterManager.GetChildItems(row2.Id, emptylists);
                    }
                }
                GetChildItem(parentId, emptylists);
                //emptylists.Item2.AddRange(receivedLetter.ParentReceivedLetterOut);
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
                    emptylists.Item1.Add(row);
                    if (row.ParentReceivedLetterOut != null &&
                        row.ParentReceivedLetterOut.Count > 0)
                    {
                        foreach (var row2 in row.ParentReceivedLetterOut)
                        {
                            emptylists.Item2.Add(row2);
                            SendLetterManager mySendLetterManager = new SendLetterManager();
                            mySendLetterManager.GetChildItems(row2.Id, emptylists);
                        }
                    }

                    GetChildItems(row.Id, emptylists); // Search for childs that belong to this item
                }
            }

            return emptylists;

        }

        private List<ReceivedLetter> GetRowsByParent(Guid parentId)
        {
            List<ReceivedLetter> DbMenuTable = Read("1==1",
                "ParentReceivedLetterOut,ParentReceiveLetter").ToList();
            return DbMenuTable.Where(item => item.ParentReceiveLetter?.Id == parentId).ToList();
        }
    }
}

