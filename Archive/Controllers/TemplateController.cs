using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Archive.Filters;
using Archive.ViewModel;
using Archive.ViewModel.Templates;
using Aspose.Words;
using Aspose.Words.Reporting;
using Aspose.Words.Saving;
using BusinessModel;
using DBProvider;
using EntityModel;
using PrintableSendLetter = EntityModel.PrintableSendLetter;

namespace Archive.Controllers
{
    [AdminFilter]
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult Index()
        {
            TemplateViewModel templateViewModel = new TemplateViewModel { PageId = "Template" };
            TemplateManager templateManager = new TemplateManager();
            templateViewModel.Templates = new List<TemplateVM>();
            foreach (var template in templateManager.Read(null))
            {
                Stream stream = new MemoryStream(template.File.Content);
                TemplateVM templateVm = new TemplateVM
                {
                    Name = template.File.Name,
                    Content = getSampleLetter(stream),
                    Id = template.Id
                };
                templateViewModel.Templates.Add(templateVm);
            }

            return View(templateViewModel);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddTemplateViewModel addTemplateViewModel = new AddTemplateViewModel { PageId = "Template" };

            return View(addTemplateViewModel);
        }

        [HttpPost]
        public ActionResult Add(AddTemplateViewModel addTemplateViewModel)
        {
            addTemplateViewModel.PageId = "Template";
            if (addTemplateViewModel.fileUpload != null && addTemplateViewModel.fileUpload.ContentLength > 0)
            {
                if (addTemplateViewModel.fileUpload.FileName.Split('.')[1] != "docx")
                {
                    ModelState.AddModelError("error", "فرمت فایل قالب باید ورد باشد.");
                    return View(addTemplateViewModel);
                }

                //addTemplateViewModel.template.File = new ArchiveFile();
                //Get Content of image
                MemoryStream target = new MemoryStream();
                addTemplateViewModel.fileUpload.InputStream.CopyTo(target);
                addTemplateViewModel.template.File.Content = target.ToArray();
                //addTemplateViewModel.template.File.Name = addTemplateViewModel.fileUpload.FileName;
                addTemplateViewModel.template.File.ContentType = addTemplateViewModel.fileUpload.ContentType;
            }
            else
            {
                ModelState.AddModelError("error", "فایل قالب وارد نشده است.");
                return View(addTemplateViewModel);
            }

            TemplateManager templateManager = new TemplateManager();
            templateManager.Add(addTemplateViewModel.template);
            templateManager.saveChanges();

            return RedirectToAction("Index");

        }


        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            EditTemplateViewModel editTemplateViewModel = new EditTemplateViewModel { PageId = "Template" };
            TemplateManager templateManager = new TemplateManager();
            editTemplateViewModel.template = templateManager.Read(id);

            if (editTemplateViewModel.template != null)
            {
                editTemplateViewModel.PreViewImage = 
                getSampleLetter(new MemoryStream(editTemplateViewModel.template.File.Content));

                return View(editTemplateViewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(EditTemplateViewModel editTemplateViewModel)
        {
            editTemplateViewModel.PageId = "Template";
            if (editTemplateViewModel.fileUpload != null && editTemplateViewModel.fileUpload.ContentLength > 0)
            {
                if (editTemplateViewModel.fileUpload.FileName.Split('.')[1] != "docx")
                {
                    ModelState.AddModelError("error", "فرمت فایل قالب باید ورد باشد.");
                    return View(editTemplateViewModel);
                }

                //addTemplateViewModel.template.File = new ArchiveFile();
                //Get Content of image
                MemoryStream target = new MemoryStream();
                editTemplateViewModel.fileUpload.InputStream.CopyTo(target);
                editTemplateViewModel.template.File.Content = target.ToArray();
                //addTemplateViewModel.template.File.Name = addTemplateViewModel.fileUpload.FileName;
                editTemplateViewModel.template.File.ContentType = editTemplateViewModel.fileUpload.ContentType;
            }
            else
            {
                ModelState.AddModelError("error", "فایل قالب وارد نشده است.");
                return View(editTemplateViewModel);
            }

            TemplateManager templateManager = new TemplateManager();
            templateManager.Update(editTemplateViewModel.template);
            templateManager.saveChanges();

            return RedirectToAction("Index");

        }


        public ActionResult Delete(Guid id)
        {
            TemplateManager templateManager = new TemplateManager();
            Template template = templateManager.Read(id);
            if (template != null)
            {
                templateManager.Remove(template);
                templateManager.saveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public string PreView()
        {
            MemoryStream outStream = new MemoryStream();
            string img = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                //Get First Uploaded file
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                                                            //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;

                img = getSampleLetter(file.InputStream);
                break;
            }

            return img;
        }


        private string getSampleLetter(Stream st)
        {
            Document doc = new Document(st);

            //Process Document
            ReportingEngine engine = new ReportingEngine { Options = ReportBuildOptions.AllowMissingMembers };
            PrintableSendLetter printableSendLetter = new PrintableSendLetter
            {
                DateTime = DateTime.Now.ToPersianTime().toPersianNumber(),
                Appendage = "ندارد",
                Number = "1/1395/ت".toPersianNumber(),
                Body = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد. کتابهای زیادی در شصت و سه درصد گذشته، حال و آینده شناخت فراوان جامعه و متخصصان را می طلبد تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی و فرهنگ پیشرو در زبان فارسی ایجاد کرد. در این صورت می توان امید داشت که تمام و دشواری موجود در ارائه راهکارها و شرایط سخت تایپ به پایان رسد وزمان مورد نیاز شامل حروفچینی دستاوردهای اصلی و جوابگوی سوالات پیوسته اهل دنیای موجود طراحی اساسا مورد استفاده قرار گیرد.".toPersianNumber(),
                SenderName = "مجتبی نیکونژاد",
                DepartMentName = "گروه نرم افزاری ایکس بیت"
            };


            printableSendLetter.SubTitle = "جناب آقای افشین تهرانی";


            printableSendLetter.Title = "مدیریت محترم هتل آریوبرزن";

            // Execute the build report.
            engine.BuildReport(doc, printableSendLetter, "s");


            ImageSaveOptions imageSaveOptions = new ImageSaveOptions(SaveFormat.Png)
            {
                UseHighQualityRendering = true
            };

            MemoryStream outStream = new MemoryStream();
            doc.Save(outStream, imageSaveOptions);

            var base64Data = Convert.ToBase64String(outStream.ToArray());
            return "data:image/png;base64," + base64Data;
        }
    }
}