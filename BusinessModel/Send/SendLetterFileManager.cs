using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using EntityModel;
using DataAccessLayer;
using DBProvider;

namespace BusinessModel
{
    public class SendLetterFileManager : DBProvider<SendLetterFile>
    {
        public SendLetterFileManager() : base(new DatabaseContext())
        {
        }

        public void RemoveRange(SendLetterFile sendLetterFile)
        {
            DataAccessLayer.DatabaseContext db = new DatabaseContext();

            //var sendLetterFile = new SendLetterFile { Id = sendLetterId };

            //   db.SendLetterFiles.Attach(sendLetterFile);
            bool oldValidateOnSaveEnabled = db.Configuration.ValidateOnSaveEnabled;

            try
            {
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SendLetterFiles.Attach(sendLetterFile);
                db.Entry(sendLetterFile).State = EntityState.Deleted;
                db.SaveChanges();
            }
            finally
            {
                db.Configuration.ValidateOnSaveEnabled = oldValidateOnSaveEnabled;
            }

            //db.SendLetterFiles.Remove(sendLetterFile);
            //db.Entry(sendLetterId).State = EntityState.Deleted;
            //db.SaveChanges();
            //DBC.Set<SendLetterFile>().RemoveRange(slf);
            //DBC.SaveChanges();

        }


        public void RemoveAll(IList<SendLetterFile> sendLetterFiles)
        {
            dbSet.RemoveRange(sendLetterFiles);
            //saveChanges();
        }
    }
}
